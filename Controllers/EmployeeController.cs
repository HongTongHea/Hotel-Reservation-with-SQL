using Hotel_Reservation.Data;
using Hotel_Reservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Reservation.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public ActionResult Index()
        {
            List<Employee> employees = _context.Employee.ToList();
            return View(employees);
        }

        public IActionResult Create()
        {
            Employee employee = new Employee();
            return PartialView("_CreateEmployee", employee);
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Employee.Add(employee);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Employee created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the Employee.");
                }
            }
            TempData["ShowCreateModal"] = true;
            return PartialView("_CreateEmployee", employee);
        }

        public IActionResult Edit(int id)
        {
            Employee employee = GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return PartialView("_EditEmployee", employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Employee.Update(employee);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Employee updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the Employee.");
                }
            }
            TempData["ShowEditModal"] = true;
            return PartialView("_EditEmployee", employee);
        }

        public IActionResult Delete(int id)
        {
            Employee employee = GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return PartialView("_DeleteEmployee", employee);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int EmployeeID)
        {
            Employee employee = GetEmployeeById(EmployeeID);
            if (employee == null)
            {
                TempData["ErrorMessage"] = "Employee not found.";
                return RedirectToAction("Index");
            }
            try
            {
                _context.Employee.Remove(employee);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Employee deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the employee.";
                return RedirectToAction("Index");
            }
        }
        private Employee GetEmployeeById(int id)
        {
            return _context.Employee.FirstOrDefault(e => e.EmployeeID == id);
        }
    }
}
