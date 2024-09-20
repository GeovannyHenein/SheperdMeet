using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using MinimalApiProject.Models;

var builder = WebApplication.CreateBuilder(args);

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
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

// New endpoints for handling priest availability
app.MapGet("/priestavailabilities", async (AppDbContext db) => await db.PriestAvailabilities.ToListAsync());

app.MapPost("/priestavailabilities", async (PriestAvailabilityInput availability, AppDbContext db) => {
    db.PriestAvailabilities.Add(availability);
    await db.SaveChangesAsync();
    return Results.Created($"/priestavailabilities/{availability.ID}", availability);
});

// Frontend integration: Example payload for priest availability
app.MapPost("/priestavailabilities/frontend", async (HttpContext context, AppDbContext db) =>
{
    // Reading JSON data from frontend
    var availabilityData = await context.Request.ReadFromJsonAsync<PriestAvailabilityInput>();

    if (availabilityData == null)
        return Results.BadRequest("Invalid availability data.");

    // Creating new PriestAvailability object
var newAvailability = new PriestAvailabilityInput
{
    StartDate = availabilityData.StartDate,
    EndDate = availabilityData.EndDate,
    Days = availabilityData.Days, // Correctly assign List<string>
    StartTime = availabilityData.StartTime,
    EndTime = availabilityData.EndTime
};

    db.PriestAvailabilities.Add(newAvailability);
    await db.SaveChangesAsync();
    return Results.Created($"/priestavailabilities/{newAvailability.ID}", newAvailability);
});

app.MapGet("/", () => "Hello World!");

// Remaining endpoints (user authentication, profiles, meetings, etc.)
app.MapGet("/login", async (HttpContext context) =>
{
    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/post-login"
    });
});

app.MapGet("/post-login", async (HttpContext context, AppDbContext db) =>
{
    if (context.User?.Identity?.IsAuthenticated == true)
    {
        var emailClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
        var nameClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        var FirstName = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
        var LastName = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;

        if (emailClaim != null)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == emailClaim);
            if (user != null)
            {
                // User exists, redirect to profile
                return Results.Redirect("/profile");
            }
            else
            {
                // User does not exist, create a new account
                var newUser = new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = emailClaim
                };
                db.Users.Add(newUser);
                await db.SaveChangesAsync();

                var htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Account Created</title>
                </head>
                <body>
                    <h1>Account Created</h1>
                    <p>An account for {nameClaim} has been created.</p>
                    <script>
                        setTimeout(function() {{
                            window.location.href = '/profile';
                        }}, 3000);
                    </script>
                </body>
                </html>";

                return Results.Content(htmlContent, "text/html");
            }
        }
    }
    return Results.Unauthorized();
});


app.MapGet("/profile", (HttpContext context) =>
{
    if (context.User?.Identity?.IsAuthenticated == true)
    {
        var nameClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/firstname")?.Value;
        var emailClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
        var result = new
        {
            Name = nameClaim,
            Email = emailClaim
        };

        return Results.Ok(result);
    }
    return Results.Unauthorized();
});

app.MapGet("/users", async (HttpContext context, AppDbContext db) =>
{
    var emailClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
    if (emailClaim != "heneinfilobatire@gmail.com")
    {
        return Results.Unauthorized();
    }

    var users = await db.Users.ToListAsync();
    return Results.Ok(users);
});

app.MapPut("/users/{id}", async (int id, HttpContext context, User updatedUser, AppDbContext db) =>
{
    var emailClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
    if (emailClaim != "heneinfilobatire@gmail.com")
    {
        return Results.Unauthorized();
    }

    var user = await db.Users.FindAsync(id);
    if (user == null)
    {
        return Results.NotFound();
    }

    user.FirstName = updatedUser.FirstName;
    user.Email = updatedUser.Email;
    // Update other fields as necessary

    await db.SaveChangesAsync();
    return Results.Ok(user);
});

app.MapPost("/users/cancel", (HttpContext context) =>
{
    var emailClaim = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
    if (emailClaim != "heneinfilobatire@gmail.com")
    {
        return Results.Unauthorized();
    }

    // Logic to handle cancel operation
    return Results.Ok("Operation canceled.");
});

//remove later
//work on backend for meeting
app.MapGet("/delete-all-users", async (AppDbContext db) =>
{
    async Task DeleteAllUsers()
    {
        var users = await db.Users.ToListAsync();
        db.Users.RemoveRange(users);
        await db.SaveChangesAsync();
    }

    await DeleteAllUsers();
    return Results.Ok("All users have been deleted.");
});


app.Run();