using Hotel_Reservation.Data;
using Hotel_Reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hotel_Reservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Combined constructor
        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch the count of rooms from the database
            int reservationCount = await _context.Reservation.CountAsync();
            int roomCount = await _context.Room.CountAsync();
            int customerCount = await _context.Customer.CountAsync();
            int employeeCount = await _context.Employee.CountAsync();
            int billingsCount = await _context.Billing.CountAsync();
            int userCount = await _context.AppUser.CountAsync();

            // Pass the count to the view using ViewBag
            ViewBag.ReservationCount = reservationCount;
            ViewBag.RoomCount = roomCount;
            ViewBag.CustomerCount = customerCount;
            ViewBag.EmployeeCount = employeeCount;
            ViewBag.BillingsCount = billingsCount;
            ViewBag.UserCount = userCount;

            ViewBag.CheckInCount = 455; // Total check-ins
            ViewBag.CheckOutCount = 321; // Total check-outs

            // Daily check-in data for the last 10 days
            ViewBag.CheckInDay1 = 120;
            ViewBag.CheckInDay2 = 190;
            ViewBag.CheckInDay3 = 300;
            ViewBag.CheckInDay4 = 500;
            ViewBag.CheckInDay5 = 200;
            ViewBag.CheckInDay6 = 300;
            ViewBag.CheckInDay7 = 234;
            ViewBag.CheckInDay8 = 320;
            ViewBag.CheckInDay9 = 305;
            ViewBag.CheckInDay10 = 450;

            ViewBag.CheckOutDay1 = 150;
            ViewBag.CheckOutDay2 = 230;
            ViewBag.CheckOutDay3 = 270;
            ViewBag.CheckOutDay4 = 400;
            ViewBag.CheckOutDay5 = 150;
            ViewBag.CheckOutDay6 = 350;
            ViewBag.CheckOutDay7 = 400;
            ViewBag.CheckOutDay8 = 450;
            ViewBag.CheckOutDay9 = 250;
            ViewBag.CheckOutDay10 = 400;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}