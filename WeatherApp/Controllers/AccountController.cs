using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;

// AccountController handles all user authentication
// This includes registering new users, logging in, and logging out
namespace WeatherApp.Controllers
{
    public class AccountController : Controller
    {
        // UserManager handles creating and managing users in the database
        private readonly UserManager<ApplicationUser> _userManager;

        // SignInManager handles logging users in and out
        private readonly SignInManager<ApplicationUser> _signInManager;

        // Constructor - ASP.NET automatically injects UserManager and SignInManager
        // This is called dependency injection - a core concept in .NET development
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Login
        // Returns the login page when the user navigates to it
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        // Handles the login form submission
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Attempt to sign in with the provided username and password
            // isPersistent: false means the login cookie expires when browser closes
            // lockoutOnFailure: true means account locks after too many failed attempts
            var result = await _signInManager.PasswordSignInAsync(
                username, password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // Login successful - redirect to the weather dashboard
                return RedirectToAction("Index", "Weather");
            }

            // Login failed - show error message on the login page
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // GET: /Account/Register
        // Returns the registration page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        // Handles the registration form submission
        [HttpPost]
        public async Task<IActionResult> Register(string username, string email,
            string password, string firstName, string lastName)
        {
            // Create a new user object with the provided information
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            // Attempt to create the user in the database
            // Identity automatically hashes the password before storing it
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Registration successful - automatically log the user in
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Weather");
            }

            // Registration failed - show the first error message
            ViewBag.Error = result.Errors.FirstOrDefault()?.Description;
            return View();
        }

        // POST: /Account/Logout
        // Logs the user out and redirects to the login page
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign the user out and clear their authentication cookie
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}