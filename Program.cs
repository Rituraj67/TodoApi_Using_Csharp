using MyTodo.Database;
using Microsoft.EntityFrameworkCore;
using MyTodo.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load();

// Configure app settings
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// ✅ Read DB_URI from environment variables
var dbUri = Environment.GetEnvironmentVariable("DB_URI");

// ✅ Set ConnectionStrings dynamically
if (!string.IsNullOrEmpty(dbUri))
{
    builder.Configuration["ConnectionStrings:DefaultConnection"] = dbUri;
}

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Database Connection: {connectionString}"); // Debugging


// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Configure PostgreSQL Database Connection
builder.Services.AddDbContext<TodoDbContext>(options =>
{
    var dbConnection = builder.Configuration["ConnectionStrings:DefaultConnection"];

    if (string.IsNullOrEmpty(dbConnection))
    {
        throw new InvalidOperationException("❌ Database connection string is missing inside UseNpgsql.");
    }

    Console.WriteLine($"🟢 Inside UseNpgsql: {dbConnection}"); // Debugging

    options.UseNpgsql(dbConnection);
});





//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//        .AddJwtBearer(options =>
//        {
//            options.TokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                ValidateAudience = true,
//                ValidAudience = builder.Configuration["Jwt:Audience"],
//                ValidateLifetime = true,
//                IssuerSigningKey = new SymmetricSecurityKey(
//                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Token"])),
//                ValidateIssuerSigningKey = true
//            };
//        });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.Cookie.Name = "authToken";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITodoService, TodoService>();

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
