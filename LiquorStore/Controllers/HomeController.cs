using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LiquorStore.Models;
using Microsoft.EntityFrameworkCore;

namespace LiquorStore.Controllers;




public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly LSContext _context;


    public HomeController(ILogger<HomeController> logger, LSContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products.Where(p => p.Category == "Tequila");
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult AccessDenied()
{
    return View();
}
}
