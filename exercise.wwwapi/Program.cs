using System.Diagnostics;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.Data;
using exercise.wwwapi.DTOs.Register;
using exercise.wwwapi.DTOs.UpdateUser;
using exercise.wwwapi.Endpoints;
using exercise.wwwapi.EndPoints;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Validators.UserValidators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text;
using exercise.wwwapi.Models.UserInfo;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var config = new ConfigurationSettings();

// Add services to the container.
builder.Services.AddScoped<IConfigurationSettings, ConfigurationSettings>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Credential>, Repository<Credential>>();
builder.Services.AddScoped<IRepository<Profile>, Repository<Profile>>();
builder.Services.AddScoped<ILogger, Logger<string>>();
builder.Services.AddScoped<IValidator<RegisterRequestDTO>, UserRegisterValidator>();
builder.Services.AddScoped<IValidator<UpdateUserRequestDTO>, UserUpdateValidator>();

builder.Services.AddDbContext<DataContext>(options =>
{
    if (builder.Configuration.GetValue<string>("testing") != null)
    {
        options.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }
    else
    {
        var host = builder.Configuration["Neon:Host"];
        var database = builder.Configuration["Neon:Database"];
        var username = builder.Configuration["Neon:Username"];
        var password = builder.Configuration["Neon:Password"];

        const string defaultConnectionName = "DefaultConnection";
        var connectionString = builder.Configuration.GetConnectionString(defaultConnectionName);
        if (connectionString == null)
        {
            throw new Exception("Could not find connection string with name: " + defaultConnectionName);
        }

        connectionString = connectionString.Replace("${Neon:Host}", host);
        connectionString = connectionString.Replace("${Neon:Database}", database);
        connectionString = connectionString.Replace("${Neon:Username}", username);
        connectionString = connectionString.Replace("${Neon:Password}", password);

        options.UseNpgsql(connectionString);
        options.LogTo(message => Debug.WriteLine(message));
    }
});

string? token;

if (builder.Environment.IsStaging())
{
    token = config.GetValue("AppSettings:TestToken");
}
else
{
    throw new Exception("this shouldn't happen");
    token = builder.Configuration["Token"];
}

if (token == null)
{
    throw new Exception("Could not find suitable token, try adding a token using usersecrets");
}

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "C# Team Development Server",
        Description = "A .Net Minimal API with JWT Auth",
    });
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "Add an Authorization header with a JWT token using the Bearer scheme see the app.http file for an example.)",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
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
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
    app.UseSwaggerUI();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v3.json", "Demo API"));
    app.MapScalarApiReference();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.ConfigureAuthApi();
app.ConfigureSecureApi();
app.ConfigureLogEndpoints();
app.ConfigureCohortEndpoints();
app.ConfigurePostEndpoints();
app.Run();

public partial class Program
{
} // needed for testing - please ignore