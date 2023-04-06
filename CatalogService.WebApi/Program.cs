using CatalogService.DAL;
using CatalogService.DI;
using CatalogService.Domain.Interfaces;
using CatalogService.WebApi.Extensions;
using CatalogService.WebApi.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersAndOData();

builder.Services.AddDbContext(builder.Configuration.GetConnectionString("CatalogService"));
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
