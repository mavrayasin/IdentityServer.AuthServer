using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:7009"; // IdentityServer URL
        options.Audience = "resource_api1"; // API resource name
        options.RequireHttpsMetadata = false; // For development only, set to true in production
    });
builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("ReadProduct", policy =>
    {
        policy.RequireClaim("scope", "api1.read");
    });
    opts.AddPolicy("UpdateOrCreate", policy =>
    {
        policy.RequireClaim("scope", new[] { "api1.update", "api1.create" });
    });
});
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
