using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using task_manager.Infraestructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

AuthorizationPolicy authenticatedUserPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

//Configurar la localización en el proyecto
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources"; //Ruta donde se encuentran los archivos de recursos
});

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter(authenticatedUserPolicy));
}).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix); //Esta línea permite que las vistas tengan soporte para la localización

//Configurar la clase ApplicationDbContext como un servicio
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

//Configurar Identity Framework Core
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configurar la autenticación con cookies
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/Users/Login"; //Ruta de inicio de sesión
    options.LogoutPath = "/Users/Logout"; //Ruta de cierre de sesión
    options.AccessDeniedPath = "/Users/AccessDenied"; //Ruta de acceso denegado
});

WebApplication app = builder.Build();

string[] supportedUICultures = ["es", "en"];

app.UseRequestLocalization(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es");
    options.SupportedUICultures = supportedUICultures.Select(culture => new CultureInfo(culture)).ToList();
    //options.SupportedCultures = supportedUICultures.Select(culture => new CultureInfo(culture)).ToList();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();

app.Run();

/*
 * Internacionalización: Proceso de tener una app que funcione para distintas culturas, le llamamos internacionalización.
 * La globalización se refiere al proceso de diseñar una aplicación para que funcione con distintas culturas.
 * La localización se refiere al proceso de tomar una aplicación globalizada, y aplicarle una cultura especifica.
 * La Cultura se refiere al resultado de funciones que dependen de la cultura, como fechas, tiempo, número y formato de dinero.
 * La CulturaUI se refiere al idioma en el que se muestra la aplicación.
 */

/*
 * Como sabe ASP.NET Core en que idioma mostrar la aplicación?
 * por defecto tenemos 4 posobilidades:
 * 1) Query String: ?ui-culture=es-ES
 * 2) Cookie: ASP.NET Core guarda la cultura en una cookie
 * 3) Header: Accept-Language
 * 4) Cultura por defecto: si no se especifica ninguna cultura, se usa la cultura por defecto configurada en el RequestLocalizationOptions
 */