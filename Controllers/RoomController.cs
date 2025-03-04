using Hotel_Reservation.Data;
using Hotel_Reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservation.Controllers
{

    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RoomController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Room
        public async Task<IActionResult> Index()
        {
            List<Room> rooms = await _context.Room.ToListAsync();
            return View(rooms);
        }

        // GET: Room/Create
        public IActionResult Create()
        {
            Room room = new Room();
            return PartialView("_CreateRoom", room);
        }

        // POST: Room/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room, IFormFile pictureFile)
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

                        room.Pictures = "/Uploads/" + fileName;
                    }

                    _context.Add(room);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Room created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception (ex) here
                    ModelState.AddModelError("", "An error occurred while saving the room.");
                }
            }

            // If validation fails, return the partial view with validation errors
            TempData["ShowCreateModal"] = true; // Flag to show the modal again
            return PartialView("_CreateRoom", room);
        }

        // GET: Room/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Room.FindAsync(id);
            if (room == null) return NotFound();

            return View(room);
        }

        // POST: Room/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room room, IFormFile? pictureFile)
        {
            if (id != room.RoomID)
                return NotFound();

            // Get existing room from database
            var existingRoom = await _context.Room.FindAsync(id);
            if (existingRoom == null)
                return NotFound();

            // Store existing image path before any updates
            var existingImagePath = existingRoom.Pictures;

            if (ModelState.IsValid)
            {
                try
                {
                    // Update scalar properties (excluding image)
                    _context.Entry(existingRoom).CurrentValues.SetValues(room);

                    // Handle image upload
                    if (pictureFile != null)
                    {
                        // Delete old image if it exists
                        if (!string.IsNullOrEmpty(existingImagePath))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                                           existingImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save new image
                        var fileName = $"{Guid.NewGuid()}_{pictureFile.FileName}";
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await pictureFile.CopyToAsync(stream);
                        }

                        existingRoom.Pictures = $"/uploads/{fileName}";
                    }
                    else
                    {
                        // Retain existing image
                        existingRoom.Pictures = existingImagePath;
                    }

                    // Save changes
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Room updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Room.Any(e => e.RoomID == id))
                        return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    // Log error (ex)
                    ModelState.AddModelError("", "An error occurred while updating the room.");
                }
            }

            // Repopulate model with existing values if validation fails
            return View(existingRoom);
        }

        // GET: Room/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Room
                .FirstOrDefaultAsync(m => m.RoomID == id);

            if (room == null) return NotFound();

            return View(room);
        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Room.FindAsync(id);
            if (room != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(room.Pictures))
                    {
                        string picturePath = Path.Combine(_webHostEnvironment.WebRootPath, room.Pictures.TrimStart('/'));
                        if (System.IO.File.Exists(picturePath))
                            System.IO.File.Delete(picturePath);
                    }

                    _context.Room.Remove(room);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Room deleted successfully!";
                }
                catch (Exception ex)
                {
                    // Log the exception (ex) here
                    TempData["ErrorMessage"] = "An error occurred while deleting the room.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Room.Any(e => e.RoomID == id);
        }
    }


}
