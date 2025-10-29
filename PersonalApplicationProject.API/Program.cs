using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using PersonalApplicationProject.BLL.Interfaces;
using PersonalApplicationProject.BLL.Options;
using PersonalApplicationProject.BLL.Services;
using PersonalApplicationProject.BLL.Validators.User;
using PersonalApplicationProject.DAL;
using PersonalApplicationProject.DAL.Interfaces;
using PersonalApplicationProject.Middleware;
using PersonalApplicationProject.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure JSON
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, ApiJsonSerializerContext.Default);
});

// Configure database connection (Docker)
string connectionString;
if (builder.Environment.IsDevelopment() && !Directory.Exists("/run/secrets"))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException(
                           "Connection string 'DefaultConnection' not found in appsettings.Development.json. Please ensure it is configured for local development.");
}
else
{
    var dbHost = builder.Configuration["Database:Host"];
    var dbName = builder.Configuration["Database:Name"];
    var dbPort = builder.Configuration["Database:Port"];
    var dbUser = await File.ReadAllTextAsync("/run/secrets/db_user");
    var dbPassword = await File.ReadAllTextAsync("/run/secrets/db_password");

    connectionString = new NpgsqlConnectionStringBuilder
    {
        Host = dbHost,
        Port = int.Parse(dbPort ?? "5432"),
        Database = dbName,
        Username = dbUser,
        Password = dbPassword
    }.ConnectionString;
}

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Register controllers
builder.Services.AddControllers().AddNewtonsoftJson();

// Register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Register global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Bind JWT options from appsettings.json
var jwtOptions = new JwtOptions();
builder.Configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);
builder.Services.AddSingleton(jwtOptions);

// Register services from BLL
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService>(serviceProvider =>
{
    var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
    return new AuthService(unitOfWork, jwtOptions);
});
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITagService, TagService>();

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestDtoValidator>();

// Configure Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Key))
        };
    });

// Configure Authorization
var fallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddAuthorizationBuilder().SetFallbackPolicy(fallbackPolicy);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://needlide.github.io")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseStatusCodePages();

app.UseRouting();
app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonalApplicationProject.API v1"));
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

await app.RunAsync();