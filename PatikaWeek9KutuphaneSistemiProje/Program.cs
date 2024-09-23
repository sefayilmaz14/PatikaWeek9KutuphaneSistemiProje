using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.LogoutPath = new PathString("/");
    options.AccessDeniedPath = new PathString("/");

    // Giriþ - Çýkýþ - Eriþim reddi durumlarýnda.

});

var app = builder.Build();

app.UseAuthentication();


app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}"
    );

app.Run();
