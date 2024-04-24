using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PostHubAPI.Data;
using PostHubAPI.Models;
using PostHubAPI.Services;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<HubService>();
builder.Services.AddScoped<PictureService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<PostService>();

// DbContext
builder.Services.AddDbContext<PostHubAPIContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PostHubAPIContext") ?? throw new InvalidOperationException("Connection string not found"));
    options.UseLazyLoadingProxies();
});

// DbContext avec Identity
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<PostHubAPIContext>();

// Authentification (déchiffrement du token)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; // Lors du développement
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = "http://localhost:4200", // Client -> HTTP
        ValidIssuer = "https://localhost:7007", // Serveur -> HTTPS
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes("LooOOongue Phrase SiNoN Ça ne Marchera PaAaAAAaAas !"))
    };
});

// Mot de passe simplifié
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 5;
});



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cross origin resource sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow all", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Allow all");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
