using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalApiProject.Models;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax; // Ensure cookies are correctly configured
})
.AddGoogle(options =>
{
    options.ClientId = "849172139625-qdi6glhi56skbbpvtntbtguq5369ti3p.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.CallbackPath = "/signin-google"; // Ensure this matches the redirect URI in Google Developer Console
});
builder.Services.AddAuthorization();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => "Hello World!");
app.MapGet("/meetings", async (AppDbContext db) => await db.Meetings.ToListAsync());
app.MapPost("/meetings", async (Meeting meeting, AppDbContext db) => {
    db.Meetings.Add(meeting);
    await db.SaveChangesAsync();
    return Results.Created($"/meetings/{meeting.ID}", meeting);
});
app.MapGet("/priestavailabilities", async (AppDbContext db) => await db.PriestAvailabilities.ToListAsync());
app.MapPost("/priestavailabilities", async (PriestAvailability availability, AppDbContext db) => {
    db.PriestAvailabilities.Add(availability);
    await db.SaveChangesAsync();
    return Results.Created($"/priestavailabilities/{availability.ID}", availability);
});
app.MapGet("/locations", async (AppDbContext db) => await db.Locations.ToListAsync());
app.MapPost("/locations", async (Location location, AppDbContext db) => {
    db.Locations.Add(location);
    await db.SaveChangesAsync();
    return Results.Created($"/locations/{location.ID}", location);
});
app.MapGet("/login", async (HttpContext context) =>
{
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/"
    });
});
app.MapGet("/profile", (HttpContext context) =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        var claims = context.User.Claims.Select(c => new { c.Type, c.Value });
        return Results.Ok(claims);
    }
    return Results.Unauthorized();
});
app.MapGet("/signin-google", async (HttpContext context) =>
{
    var result = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    if (result?.Principal is not null && result.Principal.Identity.IsAuthenticated)
    {
        return Results.Redirect("/profile"); // Redirect to a profile or other page after sign-in
    }
    return Results.Unauthorized();
});
app.Run();