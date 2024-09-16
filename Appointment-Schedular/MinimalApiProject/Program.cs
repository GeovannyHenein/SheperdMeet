using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalApiProject.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.MapGet("/users", async (AppDbContext db) => await db.Users.ToListAsync());
app.MapPost("/users", async (User user, AppDbContext db) => {
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.ID}", user);
});

app.Run();