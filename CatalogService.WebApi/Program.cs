using CatalogService.DI;
using CatalogService.WebApi.Extensions;
using CatalogService.WebApi.Middlewares;
using CorrelationId;
using CorrelationId.DependencyInjection;
using MessageQueue;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersAndOData();

builder.Services.AddDbContext(builder.Configuration.GetConnectionString("CatalogService"));
builder.Services.AddScoped<IHelpUrlBuilder, HelpUrlBuilder>();
builder.Services.AddMQConnectionProvider(builder.Configuration.GetConnectionString("MessageQueue"));
builder.Services.AddRabbitMQ(builder.Configuration.GetValue<string>("MessageQueue:Name"));
builder.Services.AddServices();
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddDefaultCorrelationId(options =>
{
    options.CorrelationIdGenerator = () => Guid.NewGuid().ToString("N");
    options.RequestHeader = "X-Correlation-ID";
    options.ResponseHeader = "X-Correlation-ID";
    options.AddToLoggingScope = true;
    options.LoggingScopeKey = "CorrelationId";
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCorrelationId();
app.UseMiddleware<CorrelationContextLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
