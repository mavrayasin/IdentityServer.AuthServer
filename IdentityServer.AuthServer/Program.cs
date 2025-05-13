using IdentityServer.AuthServer;
using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Repository;
using IdentityServer.AuthServer.Seeds;
using IdentityServer.AuthServer.Services;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomUserRepository, CustomUserRepository>();
builder.Services.AddDbContext<CustomDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));

var assemblyName = typeof(Program).Assembly.GetName().Name;

// Add services to the container.
builder.Services.AddIdentityServer()
    .AddConfigurationStore(opts =>
    {
        opts.ConfigureDbContext = b => b.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"),
     sql => sql.MigrationsAssembly(assemblyName));
    })
    .AddOperationalStore(opts =>
    {
        opts.ConfigureDbContext = b => b.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"),
     sql => sql.MigrationsAssembly(assemblyName));
        opts.EnableTokenCleanup = true;
        opts.TokenCleanupInterval = 30; // every 30 seconds (default is 3600 seconds)
    })
    //.AddInMemoryApiResources(Config.GetApiResources())
    //.AddInMemoryApiScopes(Config.GetApiScopes())
    //.AddInMemoryClients(Config.GetClients())
    //.AddInMemoryIdentityResources(Config.GetIdentityResources())
    //.AddTestUsers(Config.GetUsers().ToList())
    .AddDeveloperSigningCredential()
    // This is for development purposes only. In production, use a proper signing certificate.
    .AddProfileService<CustomProfileService>()
    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
    ;

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Seed işlemi
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ConfigurationDbContext>();
    IdentityServerSeedData.SeedData(context);
}

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
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
