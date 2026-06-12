using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Models;

// Program.cs is the entry point of the application
// This is where we configure all the services and middleware the app needs to run

var builder = WebApplication.CreateBuilder(args);

// Add MVC services - this enables Controllers and Views (the MVC pattern)
builder.Services.AddControllersWithViews();

// Add HttpClient factory - allows controllers to make external API calls
builder.Services.AddHttpClient();

// Add Entity Framework - connects our app to SQL Server using the connection string
// from appsettings.json under "DefaultConnection"
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity - sets up user authentication (login, logout, register)
// Uses our ApplicationUser model and AppDbContext for storing user data
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password requirements - keeping it simple for development
    options.Password.RequireDigit = true;           // Must have a number
    options.Password.RequiredLength = 8;            // Minimum 8 characters
    options.Password.RequireUppercase = true;       // Must have a capital letter
    options.Password.RequireNonAlphanumeric = false; // No special character required
})
.AddEntityFrameworkStores<AppDbContext>() // Store user data in our SQL database
.AddDefaultTokenProviders();             // Enables password reset tokens

// Configure login and logout redirect paths
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";   // Redirect here if not logged in
    options.LogoutPath = "/Account/Logout"; // Redirect here after logout
});

var app = builder.Build();

// Configure error handling for production environment
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enforces HTTPS in production
}

// Redirect HTTP requests to HTTPS for security
app.UseHttpsRedirection();

// Serve static files like CSS, JavaScript, and images from wwwroot folder
app.UseStaticFiles();

// Enable routing so the app knows how to handle incoming URLs
app.UseRouting();

// Enable authentication - checks if the user is logged in
app.UseAuthentication();

// Enable authorization - checks if the user has permission to access a page
app.UseAuthorization();

// Define the default URL routing pattern
// {controller=Home} means use HomeController by default
// {action=Index} means use the Index method by default
// {id?} means id is optional
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Start the application
app.Run();