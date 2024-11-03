using Microsoft.AspNetCore.Mvc;
using RentalMobil.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http; // For session management

namespace RentalMobil.Controllers
{
    // Remove [ApiController] to treat this controller as a regular MVC controller
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private static List<User> _users = new List<User>(); // Simulated user storage

        // GET: /Account/Login
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by Email and Password using LINQ
                var user = _users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    // Optionally store user information in session
                    HttpContext.Session.SetString("Email", user.Email);
                    return RedirectToAction("Index", "Home"); // Redirect to home page after successful login
                }
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }
            return View(model); // Return the view with validation errors
        }

        // GET: /Account/Register
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid && model.Password == model.RePassword) // Validate model and password match
            {
                // Check if the user already exists using LINQ
                if (_users.Any(u => u.Email == model.Email || u.PhoneNumber == model.Phone))
                {
                    ModelState.AddModelError(string.Empty, "Email or phone number already in use.");
                    return View(model); // Return view with error
                }

                // Create a new user if not already exists
                var newUser = new User
                {
                    UserId = _users.Count + 1, // Ensure this is unique; consider using Guid for production
                    Email = model.Email,
                    Username = model.Username,
                    PhoneNumber = model.Phone,
                    Address = model.Address,
                    LicenseNumber = model.LicenseNumber,
                    Status = "active", // Default status; adjust as necessary
                    Rentals = new List<Rental>(), // Initialize Rentals as a new list
                    Password = model.Password // Be cautious about storing passwords in plain text
                };
                _users.Add(newUser); // Add the new user to the simulated storage
                return RedirectToAction("Login"); // Redirect to login page after successful registration
            }
            return View(model); // Return the view with validation errors if any
        }

        // POST: /Account/Logout
        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Remove user information from session
            HttpContext.Session.Remove("Email");
            return RedirectToAction("Index", "Home"); // Redirect to home page after logout
        }
    }
}
