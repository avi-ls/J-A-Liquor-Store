using LiquorStore.Models;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly LSContext _context;

    public AccountController(LSContext context)
    {
        _context = context;
    }

    // GET: /Account/Register
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(Account user)
    {
        if (ModelState.IsValid)
        {
            // Check if user already exists
            var existingUser = _context.Account
                .FirstOrDefault(u => u.UserName == user.UserName);
            if (existingUser == null)
            {
                // Add new user to the database
                _context.Account.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("", "Username already exists");
            }
        }
        return View(user);
    }

    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(Account user)
    {
        if (ModelState.IsValid)
        {
            // Check if user exists with matching credentials
            var existingUser = _context.Account
                .FirstOrDefault(u => u.UserName == user.UserName &&
                                    u.Password == user.Password);

            if (existingUser != null)
            {
                // Store user info in session
                HttpContext.Session.SetString("userId", existingUser.Id.ToString());
                HttpContext.Session.SetString("username", existingUser.UserName);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
            }
        }
        return View(user);
    }

    public IActionResult LoggedIn()
    {
        // Check if user is actually logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
        {
            return RedirectToAction("Index");
        }
        return View();
    }

}