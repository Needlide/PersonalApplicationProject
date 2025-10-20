using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PersonalApplicationProject.DAL;
using PersonalApplicationProject.DAL.Interfaces;
using PersonalApplicationProject.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

var dbHost = builder.Configuration["Database:Host"];
var dbName = builder.Configuration["Database:Name"];
var dbPort = builder.Configuration["Database:Port"];
var dbUser = await File.ReadAllTextAsync("/run/secrets/db_user");
var dbPassword = await File.ReadAllTextAsync("/run/secrets/db_password");

var connectionString = new NpgsqlConnectionStringBuilder
{
    Host = dbHost,
    Port = int.Parse(dbPort ?? "5432"),
    Database = dbName,
    Username = dbUser,
    Password = dbPassword
}.ConnectionString;

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IUnitOfWork>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(_ => {});
}

app.MapControllers();

await app.RunAsync();