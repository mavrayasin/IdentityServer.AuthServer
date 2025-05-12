using IdentityServer.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<HttpClient>();
builder.Services.AddScoped<IApiResourceHttpClient, ApiResourceHttpClient>();

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultScheme = "Cookies";
    opts.DefaultChallengeScheme = "oidc";
}).AddCookie("Cookies", opts => { opts.AccessDeniedPath = "/Home/AccessDenied"; }).AddOpenIdConnect("oidc", opts =>
{
    opts.SignInScheme = "Cookies";
    opts.Authority = "https://localhost:7009"; // IdentityServer URL
    opts.ClientId = "Client1-Mvc";
    opts.ClientSecret = "secret";
    opts.ResponseType = "code id_token";
    opts.GetClaimsFromUserInfoEndpoint = true;
    opts.SaveTokens = true;
    opts.Scope.Add("api1.read");
    opts.Scope.Add("offline_access"); //consent ile birlikte çalışıyor bunu yoruma alırsak refresh token almadan yapar
    opts.Scope.Add("CountryAndCity");
    opts.Scope.Add("Roles");
    opts.Scope.Add("email");
    opts.ClaimActions.MapUniqueJsonKey("country", "country");
    opts.ClaimActions.MapUniqueJsonKey("city", "city");
    opts.ClaimActions.MapUniqueJsonKey("role", "role");

    opts.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };
});
// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Configuration
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
       //.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName.ToLower()}.json", optional: true)
       .AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
