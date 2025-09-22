using exercise.wwwapi;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.Data;
using exercise.wwwapi.DTOs.Comments;
using exercise.wwwapi.DTOs.Comments.UpdateComment;
using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.DTOs.Posts;
using exercise.wwwapi.DTOs.Posts.UpdatePost;
using exercise.wwwapi.DTOs.Register;
using exercise.wwwapi.DTOs.UpdateUser;
using exercise.wwwapi.DTOs.Users;
using exercise.wwwapi.Endpoints;
using exercise.wwwapi.EndPoints;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Validators.NoteValidators;
using exercise.wwwapi.Validators.PostValidators;
using exercise.wwwapi.Validators.UserValidators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var config = new ConfigurationSettings();

// Register model repositories
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Post>, Repository<Post>>();
builder.Services.AddScoped<IRepository<Comment>, Repository<Comment>>();
builder.Services.AddScoped<IRepository<Like>, Repository<Like>>();
builder.Services.AddScoped<IRepository<Course>, Repository<Course>>();
builder.Services.AddScoped<IRepository<Cohort>, Repository<Cohort>>();
builder.Services.AddScoped<IRepository<Module>, Repository<Module>>();
builder.Services.AddScoped<IRepository<Unit>, Repository<Unit>>();
builder.Services.AddScoped<IRepository<Exercise>, Repository<Exercise>>();
builder.Services.AddScoped<IRepository<Note>, Repository<Note>>();
builder.Services.AddScoped<IRepository<CohortCourse>, Repository<CohortCourse>>();
builder.Services.AddScoped<IRepository<Exercise>, Repository<Exercise>>();

// Register general services
builder.Services.AddScoped<IConfigurationSettings, ConfigurationSettings>();
builder.Services.AddScoped<ILogger, Logger<string>>();

// Register validators
builder.Services.AddScoped<IValidator<PostUserDTO>, UserRegisterValidator>();
builder.Services.AddScoped<IValidator<PatchUserDTO>, UserUpdateValidator>();
builder.Services.AddScoped<IValidator<CreateNoteRequestDTO>, CreateNoteValidator>();
builder.Services.AddScoped<IValidator<UpdateNoteRequestDTO>, UpdateNoteValidator>();

// Post validators
builder.Services.AddScoped<IValidator<CreatePostRequestDTO>, CreatePostValidator>();
builder.Services.AddScoped<IValidator<UpdatePostRequestDTO>, UpdatePostValidator>();

// Comment validators
builder.Services.AddScoped<IValidator<CreateCommentRequestDTO>, CreateCommentsValidator>();
builder.Services.AddScoped<IValidator<UpdateCommentRequestDTO>, UpdateCommentsValidator>();

// Database context
builder.Services.AddDbContext<DataContext>(options =>
{
    if (builder.Configuration.GetValue<string>(Globals.TestingEnvVariable) != null)
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
    token = config.GetValue(Globals.TestTokenKey);
}
else
{
    token = builder.Configuration[Globals.TokenKey];
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
            []
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

    // Generate a JWT token using your existing signing key
    var devJwtToken = GenerateDevJwtToken(token);

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API");
        c.SwaggerEndpoint("/openapi/v3.json", "Demo API");

        c.HeadContent = $@"
            <script>
                window.addEventListener('load', function() {{
                    setTimeout(function() {{
                        if (window.ui && window.ui.preauthorizeApiKey) {{
                            window.ui.preauthorizeApiKey('Bearer', 'Bearer {devJwtToken}');
                            console.log('Swagger UI auto-authenticated with dev token');
                        }} else {{
                            console.log('Swagger UI not ready for auto-authentication');
                        }}
                    }}, 2000);
                }});
            </script>";
    });
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
app.ConfigureNoteApi();
app.ConfigureSecureApi();
app.ConfigureLogEndpoints();
app.ConfigureCohortEndpoints();
app.ConfigurePostEndpoints();
app.ConfigureCommentEndpoints();
app.ConfigureExerciseEndpoints();
app.ConfigureCourseEndpoints();
app.ConfigureLikeEndpoints();
app.Run();

static string GenerateDevJwtToken(string signingKey)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(signingKey);

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, "Development User"),
        new Claim(ClaimTypes.Email, "dev@localhost.com"),
        new Claim(ClaimTypes.Role, "Teacher")
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddDays(30), 
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
    };

    var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(jwtToken);
}

public partial class Program
{
} // needed for testing - please ignore



