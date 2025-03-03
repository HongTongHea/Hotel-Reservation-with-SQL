﻿using Hotel_Reservation.Data;
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
            if (id != room.RoomID) return NotFound();

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

                        room.Pictures = $"/uploads/{fileName}";
                    }

                    _context.Update(room);
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
                    // Log the exception (ex) here
                    ModelState.AddModelError("", "An error occurred while updating the room.");
                }
            }

            return View(room);
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
