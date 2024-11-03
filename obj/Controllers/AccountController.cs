using Microsoft.AspNetCore.Mvc;
using RentalMobil.Models;

namespace RentalMobil.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
