using Hotel_Reservation.Data;
using Hotel_Reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservation.Controllers
{
    public class AppUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AppUserController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: AppUser
        public async Task<IActionResult> Index()
        {
            List<AppUser> appUsers = await _context.AppUser.ToListAsync();
            return View(appUsers);
        }

        // GET: AppUser/Create
        public IActionResult Create()
        {
            AppUser appUser = new AppUser();
            return PartialView("_CreateUser", appUser);
        }

        // POST: AppUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppUser appUser, IFormFile pictureFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (pictureFile != null && pictureFile.Length > 0)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(pictureFile.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(pictureFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await pictureFile.CopyToAsync(stream);
                        }

                        appUser.Pictures = "/Uploads/" + fileName;
                    }

                    _context.Add(appUser);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "User created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception (ex) here
                    ModelState.AddModelError("", "An error occurred while saving the user.");
                }
            }

            // If validation fails, return the partial view with validation errors
            TempData["ShowCreateModal"] = true; // Flag to show the modal again
            return PartialView("_CreateUser", appUser);
        }

        // GET: AppUser/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appUser = await _context.AppUser.FindAsync(id);
            if (appUser == null) return NotFound();

            return View(appUser);
        }

        // POST: AppUser/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppUser appUser, IFormFile? pictureFile)
        {
            if (id != appUser.AppUserID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (pictureFile != null)
                    {
                        string fileName = $"{Guid.NewGuid()}_{pictureFile.FileName}";
                        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

                        using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                        {
                            await pictureFile.CopyToAsync(fileStream);
                        }

                        appUser.Pictures = $"/uploads/{fileName}";
                    }

                    _context.Update(appUser);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "User updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AppUser.Any(e => e.AppUserID == id))
                        return NotFound();

                    throw;
                }
                catch (Exception ex)
                {
                    // Log the exception (ex) here
                    ModelState.AddModelError("", "An error occurred while updating the user.");
                }
            }

            return View(appUser);
        }

        // GET: AppUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appUser = await _context.AppUser
                .FirstOrDefaultAsync(m => m.AppUserID == id);

            if (appUser == null) return NotFound();

            return View(appUser);
        }

        // POST: AppUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var appUser = await _context.AppUser.FindAsync(id);
            if (appUser != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(appUser.Pictures))
                    {
                        string picturePath = Path.Combine(_webHostEnvironment.WebRootPath, appUser.Pictures.TrimStart('/'));
                        if (System.IO.File.Exists(picturePath))
                            System.IO.File.Delete(picturePath);
                    }

                    _context.AppUser.Remove(appUser);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "User deleted successfully!";
                }
                catch (Exception ex)
                {
                    // Log the exception (ex) here
                    TempData["ErrorMessage"] = "An error occurred while deleting the user.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(int id)
        {
            return _context.AppUser.Any(e => e.AppUserID == id);
        }
    }
}
