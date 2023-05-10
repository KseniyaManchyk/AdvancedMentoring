using CatalogService.DI;
using CatalogService.WebApi.Extensions;
using MessageQueue;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Add services to the container.

builder.Services.AddControllersAndOData();

builder.Services.AddDbContext(builder.Configuration.GetConnectionString("CatalogService"));
builder.Services.AddScoped<IHelpUrlBuilder, HelpUrlBuilder>();
builder.Services.AddMQConnectionProvider(builder.Configuration.GetConnectionString("MessageQueue"));
builder.Services.AddRabbitMQ(builder.Configuration.GetValue<string>("MessageQueue:Name"));
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
