using AdminPanel.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/LocalAuth/Login";
    options.LogoutPath = "/LocalAuth/Logout";
})
.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"), 
    openIdConnectScheme: "EntraId",
    cookieScheme: "EntraIdCookie");

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin", "GlobalAdmin"));

    options.AddPolicy("EditorOrAbove", policy =>
        policy.RequireRole("Admin", "GlobalAdmin", "Editor"));
});

builder.Services.Configure<CmsLinksOptions>(
    builder.Configuration.GetSection("CmsLinks"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
