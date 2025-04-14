using System.Security.Claims;
using LiquorStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

public class AccountController : Controller
{
    private readonly LSContext _context;

    public AccountController(LSContext context)
    {
        _context = context;
    }

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
            var existingUser = _context.Account
                .FirstOrDefault(u => u.UserName == user.UserName);
            if (existingUser == null)
            {
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
    [Authorize(Roles = "Admin")]
    public IActionResult AdminRegister()
    {
        return View();
    }



    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel user)
    {
        if (ModelState.IsValid)
        {
            var existingUser = _context.Account
                .FirstOrDefault(u => u.UserName == user.UserName &&
                                    u.Password == user.Password);

            if (existingUser != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                new Claim(ClaimTypes.Name, existingUser.UserName),
                new Claim(ClaimTypes.Role, existingUser.Role)
            };

                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync("Cookies", principal);

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
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
        {
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");

        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public JsonResult IsUserNameAvailable(string userName)
    {
        bool isAvailable = !_context.Account
            .Any(u => u.UserName.ToLower() == userName.Trim().ToLower());

        return Json(new { isAvailable });
    }
}