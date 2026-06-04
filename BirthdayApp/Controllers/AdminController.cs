using BirthdayApp.Data;
using BirthdayApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.PendingUsers = _userManager.Users.Where(u => !u.IsApproved).ToList();
            ViewBag.ApprovedUsers = _userManager.Users.Where(u => u.IsApproved).ToList();
            ViewBag.Logs = _context.SystemLogs.OrderByDescending(l => l.Timestamp).Take(30).ToList();
            return View();
        }

        public async Task<IActionResult> ApproveUser(string id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsApproved = true;
                await _userManager.UpdateAsync(user);

                _context.SystemLogs.Add(new SystemLog
                {
                    UserEmail = "System Admin",
                    ActionDetail = $"Approved Account Profile Access for: {user.Email}"
                });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RevokeUser(string id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Login", "Account");

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsApproved = false;
                await _userManager.UpdateAsync(user);

                _context.SystemLogs.Add(new SystemLog
                {
                    UserEmail = "System Admin",
                    ActionDetail = $"Revoked/Suspended Account Profile Access for: {user.Email}"
                });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}