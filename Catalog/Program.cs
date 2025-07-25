using System.Reflection;
using Catalog.Data;
using Catalog.Endpoints;
using Catalog.Extensions;
using Catalog.Services;
using MassTransit;
using ServiceDefaults;
using ServiceDefaults.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Adds services to the container.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ProductDbContext>(connectionName: "catalogdb");
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductAIService>();
builder.Services.AddMassTransitWithAssemblies(Assembly.GetExecutingAssembly());

builder.AddOllamaApiClient("ollama-llama3-2")
    .AddChatClient();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseMigration();
    app.MapOpenApi();
}

app.MapProductEndpoints();

app.UseHttpsRedirection();

app.Run();