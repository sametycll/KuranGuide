using KuranGuide.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

// Repository & Service Interfaces
using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Application.Interfaces.Services;

// Service Implementations
using KuranGuide.Application.Services;

// Repository Implementations
using KuranGuide.Infrastructure.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.OpenApi;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------
// DbContext
// -----------------------------------------------------
builder.Services.AddDbContext<KuranGuideDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsAssembly("KuranGuide.Infrastructure"));
});

// -----------------------------------------------------
// Controllers & Swagger
// -----------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KuranGuide API",
        Version = "v1"
    });

    // JWT Tanýmý
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT token formatý: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[]{}
        }
    });
});

// -----------------------------------------------------
// Repository DI
// -----------------------------------------------------
builder.Services.AddScoped<ITemaRepository, TemaRepository>();
builder.Services.AddScoped<IAyetRepository, AyetRepository>();
builder.Services.AddScoped<IHadisRepository, HadisRepository>();
builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();
builder.Services.AddScoped<IFavoriRepository, FavoriRepository>();
builder.Services.AddScoped<ISureRepository, SureRepository>();
// -----------------------------------------------------

// Service DI
// -----------------------------------------------------
builder.Services.AddScoped<ITemaService, TemaService>();
builder.Services.AddScoped<IAyetService, AyetService>();
builder.Services.AddScoped<IHadisService, HadisService>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();
builder.Services.AddScoped<IFavoriService, FavoriService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISureService, SureService>();
// -----------------------------------------------------
// JWT Authentication
// -----------------------------------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = ClaimTypes.Role
        };
    });

var app = builder.Build();

// --------------------------------------------------------
// OTOMATÝK MIGRATION VE SEED ÝÞLEMÝ
// --------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<KuranGuideDbContext>();

        // Veritabaný yoksa oluþtur, varsa migrationlarý uygula
        context.Database.Migrate();

        // Verileri Seed et
        await KuranGuideContextSeed.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabaný seed edilirken bir hata oluþtu.");
    }
}

// -----------------------------------------------------
// Middleware pipeline
// -----------------------------------------------------
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
