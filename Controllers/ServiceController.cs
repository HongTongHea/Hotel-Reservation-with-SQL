using Hotel_Reservation.Data;
using Hotel_Reservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Reservation.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ServiceController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Services> services = _context.Services.ToList();
            return View(services);
        }

        public IActionResult Create()
        {
            Services service = new Services();
            return PartialView("Create", service);
        }

        [HttpPost]
        public IActionResult Create(Services service)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Services.Add(service);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Service created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the Service.");
                }
            }
            TempData["ShowCreateModal"] = true;
            return PartialView("Create", service);
        }

        public IActionResult Edit(int id)
        {
            Services service = GetServiceById(id);
            if (service == null)
            {
                return NotFound();
            }
            return PartialView("Edit", service);
        }


        [HttpPost]
        public IActionResult Edit(Services service)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Services.Update(service);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Service updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the Service.");
                }
            }
            TempData["ShowEditModal"] = true;
            return PartialView("Edit", service);
        }

        public IActionResult Delete(int id)
        {
            Services service = GetServiceById(id);
            if (service == null)
            {
                return NotFound();
            }
            return PartialView("Delete", service);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int ServiceID)
        {
            Services service = GetServiceById(ServiceID);
            if (service == null)
            {
                TempData["ErrorMessage"] = "Service not found!";
            }
            try
            {
                _context.Services.Remove(service);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Service deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = "An error occurred while deleting the Service.";
                return RedirectToAction("Index");
            }
        }

        private Services GetServiceById(int id)
        {
            return _context.Services.FirstOrDefault(e => e.ServiceID == id);
        }

    }
}
