using Microsoft.AspNetCore.Mvc; // Importing the MVC framework for building web applications

namespace RentalMobil.Controllers // Declaring the namespace for the HomeController
{
    // Defining the HomeController class which inherits from Controller
    public class HomeController : Controller
    {
        // Action method to display the home page
        public IActionResult Index()
        {
            ViewData["Title"] = "Home"; // Setting the title for the view
            return View(); // Returning the default view for this action
        }

        // Action method to display the rental history page
        public IActionResult History()
        {
            ViewData["Title"] = "Riwayat Penyewaan"; // Setting the title for the rental history view
            return View(); // Returning the view for the rental history
        }

        // Action method to display the contact page
        public IActionResult Contact()
        {
            ViewData["Title"] = "Kontak Kami"; // Setting the title for the contact page view
            return View(); // Returning the view for the contact page
        }
    }
}
