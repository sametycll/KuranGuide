using KuranGuide.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ApiClient>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        // COOKIE'DEN TOKEN ALMAK İÇİN ZORUNLU
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                    context.Token = token;

                return Task.CompletedTask;
            },
            // GİRİŞ YOK ? LOGIN
            OnChallenge = context =>
            {
                context.HandleResponse(); // default 401’i durdur

                // Admin gizleme
                if (context.Request.Path.Value != null && context.Request.Path.Value.StartsWith("/admin", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = 404;
                }
                else
                {
                    context.Response.Redirect("/Auth/Login");
                }
                return Task.CompletedTask;
            },

            // ROL YOK ? YETKİSİZ
            OnForbidden = context =>
            {
                // Admin gizleme
                if (context.Request.Path.Value != null && context.Request.Path.Value.StartsWith("/admin", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = 404;
                }
                else
                {
                    context.Response.Redirect("/Auth/YetkisizErisim");
                }
                return Task.CompletedTask;
            }

        };
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
