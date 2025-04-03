using DataAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.Linq;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserDbContext _context;

        public AccountController(UserDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);

                if (user != null && user.Password == model.Password)  
                {
                    HttpContext.Session.SetString("Username", user.Username);

                    return RedirectToAction("Index", "Polls");  
                }

                ModelState.AddModelError("", "Invalid username or password");
            }

            return View(model); 
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("", "Username already taken.");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password  
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToAction("Index", "Polls");  
            }

            return View(model); 
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login");
        }
    }
}
