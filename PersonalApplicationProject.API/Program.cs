using Microsoft.EntityFrameworkCore;
using Npgsql;
using PersonalApplicationProject.DAL;

var builder = WebApplication.CreateSlimBuilder(args);

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

var app = builder.Build();



await app.RunAsync();
