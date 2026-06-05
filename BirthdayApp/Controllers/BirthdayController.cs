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
    [Authorize]
    public class BirthdayController : Controller
    {
        private readonly AppDbContext _context;

        public BirthdayController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            var query = _context.Birthdays.Where(b => !b.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Name.Contains(search));
                ViewBag.Search = search;
            }

            var allBirthdays = query.OrderBy(b => b.Name).ToList();

            ViewBag.TotalBirthdays = query.Count();
            DateTime today = DateTime.Today;
            ViewBag.TodayBirthdays = query
                .Where(b => b.DateOfBirth.Day == today.Day && b.DateOfBirth.Month == today.Month)
                .ToList();

            return View(allBirthdays);
        }

        [HttpPost]
        public IActionResult Create(Birthday birthday)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            birthday.CreatedBy = User.Identity?.Name ?? "Unknown";
            _context.Birthdays.Add(birthday);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var birthday = _context.Birthdays.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
            if (birthday == null) return NotFound();

            if (birthday.CreatedBy != User.Identity?.Name) return Forbid();

            return View(birthday);
        }

        [HttpPost]
        public IActionResult Edit(Birthday birthday)
        {
            var existing = _context.Birthdays.AsNoTracking().FirstOrDefault(b => b.Id == birthday.Id);
            if (existing == null) return NotFound();

            if (existing.CreatedBy != User.Identity?.Name) return Forbid();

            if (ModelState.IsValid)
            {
                birthday.CreatedBy = existing.CreatedBy;
                _context.Birthdays.Update(birthday);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(birthday);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var birthday = _context.Birthdays.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
            if (birthday == null) return NotFound();

            if (!HasAccess(birthday)) return Forbid();

            return View(birthday);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Updated to safely find only active records
            var birthday = _context.Birthdays.FirstOrDefault(b => b.Id == id && !b.IsDeleted);

            if (birthday == null) return NotFound();

            if (HasAccess(birthday))
            {
                birthday.IsDeleted = true;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult SendWish(int birthdayId, string message)
        {
            var wish = new BirthdayWish
            {
                BirthdayId = birthdayId,
                SenderName = User.Identity?.Name ?? "Anonymous",
                Message = message,
                SentAt = DateTime.Now
            };

            _context.BirthdayWishes.Add(wish);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult GetTemplates()
        {
            var templates = _context.WishTemplates.ToList();
            return Json(templates);
        }

        private bool HasAccess(Birthday birthday)
        {
            var isAdmin = HttpContext.Session.GetString("UserRole") == "Admin";
            var isCreator = birthday.CreatedBy == User.Identity?.Name;
            return isCreator || isAdmin;
        }
    }
}