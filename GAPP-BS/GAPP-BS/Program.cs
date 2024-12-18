using GAPP_BS.Data;
using GAPP_BS.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//}).AddCookie();
//builder.Services.AddOidcAuthentication(options =>
//{
//    options.ProviderOptions.Authority = "https://accounts.google.com";
//    options.ProviderOptions.ClientId = "25082372106-gaaa8ulgs4s5hi5qpsaagek5c50emjtl.apps.googleusercontent.com";
//    options.ProviderOptions.ResponseType = "code";
//    //options.ProviderOptions.DefaultScopes.Add("openid");
//    //options.ProviderOptions.DefaultScopes.Add("profile");
//    options.ProviderOptions.RedirectUri = "/authentication/login-callback";
//});
builder.Services.AddMvc(options =>
{
    options.Filters.Add(new IgnoreAntiforgeryTokenAttribute());
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
       .AddCookie()
       .AddGoogle(options =>
       {
           options.ClientId = "25082372106-gaaa8ulgs4s5hi5qpsaagek5c50emjtl.apps.googleusercontent.com";
           options.ClientSecret = "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";
           options.CallbackPath = new PathString("/authentication/login-callback");
       });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GoogleUser", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes(GoogleDefaults.AuthenticationScheme);
    });
});
builder.Services.AddHttpContextAccessor(); // Register IHttpContextAccessor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Add services to the container.
builder.Services.AddScoped< CustomAuthenticationStateProvider>();

builder.Services.AddSingleton<WeatherForecast>();

builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
