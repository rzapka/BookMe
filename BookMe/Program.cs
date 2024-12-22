using System.Globalization;
using TimeZoneConverter; 
using BookMe.Extensions;
using BookMe.Infrastructure.Extensions;
using BookMe.Application.Extensions;
using BookMe.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using BookMe.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var polandTimeZone = TZConvert.GetTimeZoneInfo("Europe/Warsaw");

var cultureInfo = new CultureInfo("pl-PL");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddSingleton(polandTimeZone);

var connectionString = builder.Configuration.GetConnectionString("BookMe") ?? throw new InvalidOperationException("Connection string 'BookMeDbContextConnection' not found.");
//builder.Services.AddDbContext<BookMeDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<BookMeDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddInfrastucture(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddSingleton<IdentityErrorDescriber, PolishIdentityErrorDescriber>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Uzytkownicy/Zaloguj");
    options.AccessDeniedPath = new PathString("/Uzytkownicy/BrakUprawnien");
});

var app = builder.Build();

var scope = app.Services.CreateScope();

var seeders = new List<object>
{
    scope.ServiceProvider.GetRequiredService<ServiceCategorySeeder>(),
    scope.ServiceProvider.GetRequiredService<ServiceSeeder>(),
    scope.ServiceProvider.GetRequiredService<UserSeeder>(),
    scope.ServiceProvider.GetRequiredService<EmployeeSeeder>(),
    scope.ServiceProvider.GetRequiredService<OfferSeeder>(),
    scope.ServiceProvider.GetRequiredService<BookingSeeder>(),
    scope.ServiceProvider.GetRequiredService<OpinionSeeder>(),
    scope.ServiceProvider.GetRequiredService<OpeningHoursSeeder>(),
    scope.ServiceProvider.GetRequiredService<ServiceImageSeeder>()
};

foreach (var seeder in seeders)
{
    if (seeder is ISeeder s)
    {
        await s.Seed();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Przechwycenie domyślnych tras Identity
    endpoints.MapGet("/Identity/Account/Register", context =>
    {
        context.Response.Redirect("/");
        return Task.CompletedTask;
    });

    endpoints.MapGet("/Identity/Account/Login", context =>
    {
        context.Response.Redirect("/");
        return Task.CompletedTask;
    });

    endpoints.MapRazorPages();
});

app.Run();

public partial class Program { }
