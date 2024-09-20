using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using FluentValidation;
using MinimalApiProject.Models;
using MinimalApiProject.Services;
using MinimalApiProject.Validators;

var builder = WebApplication.CreateBuilder(args);

// Configure services
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
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax; 
})
.AddGoogle(options =>
{
    options.Scope.Add("email");
    options.Scope.Add("profile");
});

builder.Services.AddAuthorization();
builder.Services.AddHttpClient(); // For external APIs like Calendly
builder.Services.AddValidatorsFromAssemblyContaining<PriestAvailabilityValidator>(); // Register FluentValidation

// Register custom services
builder.Services.AddScoped<CalendlyService>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

// Priest availability endpoints
app.MapGet("/priestavailabilities", async (AppDbContext db) => await db.PriestAvailabilities.ToListAsync());

app.MapPost("/priestavailabilities", async (PriestAvailabilityInput availability, AppDbContext db) => {
    db.PriestAvailabilities.Add(availability);
    await db.SaveChangesAsync();
    return Results.Created($"/priestavailabilities/{availability.ID}", availability);
});

app.MapPost("/priestavailabilities/frontend", async (HttpContext context, AppDbContext db, IValidator<PriestAvailabilityInput> validator, CalendlyService calendlyService) =>
{
    var availabilityData = await context.Request.ReadFromJsonAsync<PriestAvailabilityInput>();

    var validationResult = await validator.ValidateAsync(availabilityData);
    if (!validationResult.IsValid)
        return Results.BadRequest(validationResult.Errors);

    db.PriestAvailabilities.Add(availabilityData);
    await db.SaveChangesAsync();

    // Optionally call Calendly API to create event
    await calendlyService.CreateEventAsync(availabilityData);
    return Results.Created($"/priestavailabilities/{availabilityData.ID}", availabilityData);
});

// Google authentication
app.MapGet("/login", async (HttpContext context) =>
{
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/post-login"
    });
});

// Post-login endpoint
app.MapGet("/post-login", async (HttpContext context, AppDbContext db) =>
{
    if (context.User?.Identity?.IsAuthenticated == true)
    {
        var emailClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
        var firstName = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
        var lastName = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;

        if (emailClaim != null)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == emailClaim);
            if (user == null)
            {
                var newUser = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = emailClaim
                };
                db.Users.Add(newUser);
                await db.SaveChangesAsync();
            }

            return Results.Redirect("/profile");
        }
    }

    return Results.Unauthorized();
});

app.MapGet("/profile", (HttpContext context) =>
{
    if (context.User?.Identity?.IsAuthenticated == true)
    {
        var name = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        var email = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

        return Results.Ok(new { Name = name, Email = email });
    }
    return Results.Unauthorized();
});

// Delete all users (only for development purposes)
app.MapGet("/delete-all-users", async (AppDbContext db) =>
{
    var users = await db.Users.ToListAsync();
    db.Users.RemoveRange(users);
    await db.SaveChangesAsync();
    return Results.Ok("All users deleted.");
});

app.Run();
