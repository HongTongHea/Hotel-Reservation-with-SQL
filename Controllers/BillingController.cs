using Hotel_Reservation.Data;
using Hotel_Reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservation.Controllers
{
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BillingController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Billing> billing = _context.Billing
            .Include(b => b.Customer)
            .Include(b => b.Room)
            .ToList();

            ViewBag.Customers = new SelectList(_context.Customer.Select(c => new
            {
                CustomerID = c.CustomerID,
                FullName = c.FirstName + " " + c.LastName
            }), "CustomerID", "FullName");
            ViewBag.Rooms = new SelectList(_context.Room, "RoomID", "RoomNumber");
            ViewBag.Services = new SelectList(_context.Services, "ServiceID", "ServiceName");
            ViewBag.Reservations = new SelectList(_context.Reservation.Select(r => new
            {
                ReservationID = r.ReservationID,
                DisplayText = "Reservation " + r.ReservationID
            }), "ReservationID", "DisplayText");

            return View(billing);
        }

        [HttpGet]
        public async Task<JsonResult> GetReservationTotalPrice(int id)
        {
            var reservation = await _context.Reservation
                                            .Where(r => r.ReservationID == id)
                                            .Select(r => r.TotalPrice ?? 0)
                                            .FirstOrDefaultAsync();
            return Json(reservation);
        }

        [HttpGet]
        public async Task<JsonResult> GetServiceRate(int id)
        {
            var service = await _context.Services
                                        .Where(s => s.ServiceID == id)
                                        .Select(s => s.ServiceRate)
                                        .FirstOrDefaultAsync();
            return Json(service);
        }

        public IActionResult Create()
        {
            Billing billing = new Billing();
            ViewBag.Customers = new SelectList(_context.Customer.Select(c => new
            {
                CustomerID = c.CustomerID,
                FullName = c.FirstName + " " + c.LastName
            }), "CustomerID", "FullName");
            ViewBag.Rooms = new SelectList(_context.Room, "RoomID", "RoomNumber");
            ViewBag.Services = new SelectList(_context.Services, "ServiceID", "ServiceName");
            ViewBag.Reservations = new SelectList(_context.Reservation.Select(r => new
            {
                ReservationID = r.ReservationID,
                DisplayText = "Reservation " + r.ReservationID
            }), "ReservationID", "DisplayText");

            return PartialView("_CreateBill", billing);
        }


        [HttpPost]
        public IActionResult Create(Billing billing)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Billing.Add(billing);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Billing created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the Billing.");
                }
            }
            TempData["ShowCreateModal"] = true;
            return PartialView("_CreateBilling", billing);
        }

        public IActionResult Edit(int id)
        {
            Billing billing = GetBillingId(id);
            ViewBag.Customers = new SelectList(_context.Customer.Select(c => new
            {
                CustomerID = c.CustomerID,
                FullName = c.FirstName + " " + c.LastName
            }), "CustomerID", "FullName");
            ViewBag.Rooms = new SelectList(_context.Room, "RoomID", "RoomNumber");
            ViewBag.Reservations = new SelectList(_context.Reservation.Select(r => new
            {
                ReservationID = r.ReservationID,
                DisplayText = "Reservation " + r.ReservationID // Adjust display text as needed
            }), "ReservationID", "DisplayText");
            return View(billing);
        }

        [HttpPost]
        public IActionResult Edit(Billing billing)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Billing.Update(billing);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Billing updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the Billing.");
                }
            }
            TempData["ShowEditModal"] = true;
            return View(billing);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the billing record by BillingID
            var billing = _context.Billing
                .Include(b => b.Customer)  // Include related Customer data
                .Include(b => b.Room)      // Include related Room data
                .FirstOrDefault(b => b.BillingID == id);

            if (billing == null)
            {
                return NotFound();
            }

            // Return the partial view with the billing record
            return View(billing);
        }

        public IActionResult Delete(int id)
        {
            Billing billing = GetBillingId(id);
            if (billing == null)
            {
                return NotFound();
            }
            return View(billing);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int BillingID)
        {
            Billing billing = GetBillingId(BillingID);
            if (billing == null)
            {
                TempData["ErrorMessage"] = "Billing not found.";
                return RedirectToAction("Index");
            }
            try
            {
                _context.Billing.Remove(billing);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Billing deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the billing.";
                return RedirectToAction("Index");
            }
        }

        private Billing? GetBillingId(int id)
        {
            return _context.Billing.FirstOrDefault(e => e.BillingID == id);
        }

    }
}
