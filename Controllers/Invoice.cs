using Microsoft.AspNetCore.Mvc;

namespace Hotel_Reservation.Controllers
{
    public class Invoice : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
