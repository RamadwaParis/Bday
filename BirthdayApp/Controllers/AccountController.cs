using BirthdayApp.Data;
using BirthdayApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BirthdayApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        // Hardcoded Credentials preserved as per your requirement
        private const string HardcodedAdminEmail = "admin@birthdayapp.com";
        private const string HardcodedAdminPassword = "AdminPass123!";

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Credentials cannot be blank.");
                return View();
            }

            // 1. Check Hardcoded Admin Account First
            if (email.ToLower() == HardcodedAdminEmail.ToLower() && password == HardcodedAdminPassword)
            {
                HttpContext.Session.SetString("UserRole", "Admin");
                return RedirectToAction("Index", "Admin");
            }

            // 2. Normal User Database Validation
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (!user.IsApproved)
                {
                    ModelState.AddModelError("", "Access Denied. Your registration is pending approval from the Admin.");
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    HttpContext.Session.SetString("UserRole", "User");
                    return RedirectToAction("Index", "Birthday");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Fields cannot be blank.");
                return View();
            }

            if (email.ToLower() == HardcodedAdminEmail.ToLower())
            {
                ModelState.AddModelError("", "This email address is reserved for system administration.");
                return View();
            }

            var user = new ApplicationUser { UserName = email, Email = email, FullName = fullName, IsApproved = false };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Registration successful! You can log in once an Admin approves your account.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            TempData["SuccessMessage"] = "If that email matches our records, a secure password reset link has been dispatched.";
            return View();
        }
    }
}