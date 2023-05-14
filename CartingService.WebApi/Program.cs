using CartingService.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using CartingService.WebApi.MQ;
using MessageQueue;
using MessageQueue.Interfaces;
using CorrelationId;
using CartingService.WebApi.Middlewares;
using CorrelationId.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.ApplicationInsights.Extensibility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddServices(builder.Configuration.GetConnectionString("CartsService"));
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddDefaultCorrelationId(options =>
{
    options.CorrelationIdGenerator = () => Guid.NewGuid().ToString("N");
    options.RequestHeader = "X-Correlation-ID";
    options.ResponseHeader = "X-Correlation-ID";
    options.AddToLoggingScope = true;
    options.LoggingScopeKey = "CorrelationId";
});

builder.Services.AddMQConnectionProvider(builder.Configuration.GetConnectionString("MessageQueue"));
builder.Services.AddSingleton<IMessageConsumer>(serviceProvider => new MessageConsumer(
    serviceProvider.GetService<IRabbitMQConnectionProvider>(),
    serviceProvider,
    serviceProvider.GetService<ILogger<MessageConsumer>>(),
    builder.Configuration.GetValue<string>("MessageQueue:Name")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Host.UseSerilog((ctx, services, lc) => lc.WriteTo.Console().WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.UseAuthorization();

app.UseCorrelationId();
app.UseMiddleware<CorrelationContextLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.UseRabbitMQ();

app.Run();
