using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// AppDbContext is the bridge between our C# code and the SQL Server database
// It tells Entity Framework what tables to create and how to map our models to them
namespace WeatherApp.Models
{
    // AppDbContext inherits from IdentityDbContext
    // IdentityDbContext automatically creates all the tables needed for user login
    // such as Users, Roles, Claims, and Tokens in our SQL Server database
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor - receives database configuration options and passes them to the parent class
        // The options come from appsettings.json where we defined our connection string
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // OnModelCreating - called when the database tables are being set up
        // We call the parent version first to make sure Identity tables get created correctly
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}