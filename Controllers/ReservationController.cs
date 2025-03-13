using Hotel_Reservation.Data;
using Hotel_Reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservation.Controllers
{

    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReservationController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = _context.Reservation
                                       .Include(r => r.Room)
                                       .Include(r => r.Customer);
            ViewBag.Customers = new SelectList(_context.Customer.Select(c => new
            {
                CustomerID = c.CustomerID,
                FullName = c.FirstName + " " + c.LastName
            }), "CustomerID", "FullName");
            ViewBag.Rooms = new SelectList(_context.Room, "RoomID", "RoomNumber");
            ViewBag.RoomPrices = _context.Room.ToDictionary(r => r.RoomID, r => r.RoomPrice);
            return View(await reservations.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Room)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        public IActionResult Create()
        {
            ViewBag.Customers = new SelectList(_context.Customer.Select(c => new
            {
                CustomerID = c.CustomerID,
                FullName = c.FirstName + " " + c.LastName
            }), "CustomerID", "FullName");
            ViewBag.Rooms = new SelectList(_context.Room, "RoomID", "RoomNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID, RoomID, CheckInDate, CheckOutDate, NumberOfChildren, NumberOfAdults,TotalPrice,PaymentStatus")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Reservation.Add(reservation);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Reservation created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Customers = new SelectList(_context.Customer.Select(c => new
            {
                CustomerID = c.CustomerID,
                FullName = c.FirstName + " " + c.LastName
            }), "CustomerID", "FullName");
            ViewBag.Rooms = new SelectList(_context.Room, "RoomID", "RoomNumber", reservation.RoomID);
            return View(reservation);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            ViewBag.Customers = new SelectList(_context.Customer.Select(c => new
            {
                CustomerID = c.CustomerID,
                FullName = c.FirstName + " " + c.LastName
            }), "CustomerID", "FullName", reservation.CustomerID);
            ViewBag.RoomPrices = _context.Room.ToDictionary(r => r.RoomID, r => r.RoomPrice);
            ViewBag.Rooms = new SelectList(_context.Room, "RoomID", "RoomNumber", reservation.RoomID);
            return PartialView("Edit", reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID, CustomerID, RoomID, CheckInDate, CheckOutDate, NumberOfChildren, NumberOfAdults,TotalPrice,PaymentStatus")] Reservation reservation)
        {
            if (id != reservation.ReservationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "Reservation updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Customers = new SelectList(_context.Customer.Select(c => new
            {
                CustomerID = c.CustomerID,
                FullName = c.FirstName + " " + c.LastName
            }), "CustomerID", "FullName", reservation.CustomerID);
            ViewBag.Rooms = new SelectList(_context.Room, "RoomID", "RoomNumber", reservation.RoomID);
            return PartialView("Edit", reservation);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Room)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Reservation deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ReservationID == id);
        }
    }

}
