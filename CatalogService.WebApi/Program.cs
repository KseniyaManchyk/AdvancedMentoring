using CatalogService.DAL;
using CatalogService.DI;
using CatalogService.Domain.Interfaces;
using CatalogService.WebApi.Extensions;
using CatalogService.WebApi.MQ;
using RabbitMQ;
using RabbitMQ.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersAndOData();

builder.Services.AddDbContext(builder.Configuration.GetConnectionString("CatalogService"));
builder.Services.AddMQConnectionProvider(builder.Configuration.GetConnectionString("MessageQueue"));
builder.Services.AddScoped<IHelpUrlBuilder, HelpUrlBuilder>();
builder.Services.AddScoped<IMessageProducer>(s => new MessageProducer(
    s.GetService<IRabbitMQConnectionProvider>(),
    builder.Configuration.GetValue<string>("MessageQueue:Name")
    ));
builder.Services.AddServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
