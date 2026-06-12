using Microsoft.AspNetCore.Identity;

// This file defines our User model
// We extend IdentityUser which gives us built-in login functionality
// like password hashing, login tracking, and security features
namespace WeatherApp.Models
{
    // ApplicationUser inherits from IdentityUser
    // IdentityUser already includes: Username, Email, Password (hashed), and more
    // We can add custom fields here if we need extra user info later
    public class ApplicationUser : IdentityUser
    {
        // FirstName - stores the user's first name
        // ? means this field is optional (nullable)
        public string? FirstName { get; set; }

        // LastName - stores the user's last name
        // ? means this field is optional (nullable)
        public string? LastName { get; set; }
    }
}