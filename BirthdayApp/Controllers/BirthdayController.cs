using BirthdayApp.Data;
using BirthdayApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BirthdayApp.Controllers
{
    [Authorize] // Ensures only logged-in users can access these features
    public class BirthdayController : Controller
    {
        private readonly AppDbContext _context;

        public BirthdayController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: Dashboard
        [HttpGet]
        public IActionResult Index(string search)
        {
            var birthdaysQuery = _context.Birthdays.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                birthdaysQuery = birthdaysQuery.Where(b => b.Name.Contains(search));
                ViewBag.Search = search;
            }

            var allBirthdays = birthdaysQuery.OrderBy(b => b.Name).ToList();
            ViewBag.TotalBirthdays = _context.Birthdays.Count();

            DateTime today = DateTime.Today;
            ViewBag.TodayBirthdays = _context.Birthdays
                .Where(b => b.DateOfBirth.Day == today.Day && b.DateOfBirth.Month == today.Month)
                .ToList();

            return View(allBirthdays);
        }

        // 2. POST: Add Birthday (Stamps user creator)
        [HttpPost]
        public IActionResult Create(Birthday birthday)
        {
            // Automatically assign the currently logged-in user's name as the owner
            birthday.CreatedBy = User.Identity?.Name ?? "Unknown";

            if (ModelState.IsValid)
            {
                _context.Birthdays.Add(birthday);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // 3. GET: Edit Route (Ownership Validation)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var birthday = _context.Birthdays.FirstOrDefault(b => b.Id == id);
            if (birthday == null) return NotFound();

            // Strict Check: Only the creator can edit their own records
            if (birthday.CreatedBy != User.Identity?.Name)
            {
                return Forbid(); // Blocks unauthorized access
            }

            return View(birthday);
        }

        // 4. POST: Save Edit Changes
        [HttpPost]
        public IActionResult Edit(Birthday birthday)
        {
            var existingRecord = _context.Birthdays.AsNoTracking().FirstOrDefault(b => b.Id == birthday.Id);
            if (existingRecord == null) return NotFound();

            if (existingRecord.CreatedBy != User.Identity?.Name)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                birthday.CreatedBy = existingRecord.CreatedBy; // Retain original creator
                _context.Birthdays.Update(birthday);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(birthday);
        }

        // 5. GET: Delete Confirmation Route
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var birthday = _context.Birthdays.FirstOrDefault(b => b.Id == id);
            if (birthday == null) return NotFound();

            // FIX: Changed 'Context' to 'HttpContext'
            var userRole = HttpContext.Session.GetString("UserRole");

            // Allow access if the user created it OR if the user is an Admin
            if (birthday.CreatedBy != User.Identity?.Name && userRole != "Admin")
            {
                return Forbid();
            }

            return View(birthday);
        }

        // 6. POST: Commit Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var birthday = _context.Birthdays.FirstOrDefault(b => b.Id == id);
            if (birthday == null) return NotFound();

            // FIX: Changed 'Context' to 'HttpContext'
            var userRole = HttpContext.Session.GetString("UserRole");

            // Double check authority rules before destroying row data
            if (birthday.CreatedBy == User.Identity?.Name || userRole == "Admin")
            {
                _context.Birthdays.Remove(birthday);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}