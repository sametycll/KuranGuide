using KuranGuide.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

// Repository & Service Interfaces
using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Application.Interfaces.Services;

// Service Implementations
using KuranGuide.Application.Services;

// Repository Implementations
using KuranGuide.Infrastructure.Repositories;

using KuranGuide.Api.Middleware;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Security.Claims;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------
// DbContext
// ---------------------------------------------------------------
builder.Services.AddDbContext<KuranGuideDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsAssembly("KuranGuide.Infrastructure"));
});

// ---------------------------------------------------------------
// Controllers & Swagger
// ---------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KuranGuide API",
        Version = "v1",
        Description = "Kuran-i Kerim Tematik Arama API'si"
    });

    // JWT Tanimi
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT token formati: Bearer {token}",
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

// ---------------------------------------------------------------
// CORS (Cross-Origin Resource Sharing)
// ---------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7173",
                "https://localhost:5001"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ---------------------------------------------------------------
// Rate Limiting (.NET 8 Built-in)
// ---------------------------------------------------------------
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Genel limitleme: IP basina dakikada 100 istek
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 10;
    });

    // Auth endpoint limitleme: Brute-force korumasi
    options.AddFixedWindowLimiter("auth", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    // Global limitleme
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 200,
                Window = TimeSpan.FromMinutes(1)
            });
    });
});

// ---------------------------------------------------------------
// Repository DI
// ---------------------------------------------------------------
builder.Services.AddScoped<ITemaRepository, TemaRepository>();
builder.Services.AddScoped<IAyetRepository, AyetRepository>();
builder.Services.AddScoped<IHadisRepository, HadisRepository>();
builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();
builder.Services.AddScoped<IFavoriRepository, FavoriRepository>();
builder.Services.AddScoped<ISureRepository, SureRepository>();

// ---------------------------------------------------------------
// Service DI
// ---------------------------------------------------------------
builder.Services.AddScoped<ITemaService, TemaService>();
builder.Services.AddScoped<IAyetService, AyetService>();
builder.Services.AddScoped<IHadisService, HadisService>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();
builder.Services.AddScoped<IFavoriService, FavoriService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISureService, SureService>();
builder.Services.AddScoped<ISearchService, KuranGuide.Infrastructure.Repositories.SearchService>();

// ---------------------------------------------------------------
// JWT Authentication
// ---------------------------------------------------------------
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

// ---------------------------------------------------------------
// Response Caching
// ---------------------------------------------------------------
builder.Services.AddResponseCaching();

// ---------------------------------------------------------------
// Health Checks
// ---------------------------------------------------------------
builder.Services.AddHealthChecks()
    .AddDbContextCheck<KuranGuideDbContext>();

var app = builder.Build();

// ---------------------------------------------------------------
// OTOMATIK MIGRATION VE SEED ISLEMI
// ---------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<KuranGuideDbContext>();
        context.Database.Migrate();
        await KuranGuideContextSeed.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabani seed edilirken bir hata olustu.");
    }
}

// ---------------------------------------------------------------
// Middleware pipeline
// ---------------------------------------------------------------

// Global Exception Handler - en basa konmali
app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS Zorlamasi
app.UseHttpsRedirection();

// HSTS - Production ortaminda tarayicilara sadece HTTPS kullanmalarini soyler
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// CORS
app.UseCors("AllowWebApp");

// Rate Limiting
app.UseRateLimiter();

// Response Caching
app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

// Health Check endpoint
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
