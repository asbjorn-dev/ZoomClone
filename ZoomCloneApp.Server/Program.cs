using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using ZoomCloneApp.Server.Data;
using ZoomCloneApp.Server.Helpers;
using ZoomCloneApp.Server.Interfaces;
using ZoomCloneApp.Server.Models;
using ZoomCloneApp.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// db context
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlite(builder.Configuration.GetConnectionString("DbConnection")));

// identity setup
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

// JWT auth setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// add MediatR services 
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// add custom services
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<ITwilioService, TwilioService>();


// add authorization services
builder.Services.AddAuthorization();

// CORS policy setup which allows the client app to access the API
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", x =>
    {
        x.WithOrigins(builder.Configuration["Client:BaseAddress"]!)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// adds Scalar + Cors policy
app.UseCors("CorsPolicy");

// adds authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
