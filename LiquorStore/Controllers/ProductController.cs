using LiquorStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LiquorStore.Controllers
{
    public class ProductController : Controller
    {         private readonly LSContext _context;

        public ProductController(LSContext context)
        {
            _context = context;
        } 

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Categories()
        {
            var categories = _context.Products
                .Select(p => p.Category)
                .Distinct()
                .ToList();

            return View(categories);
        }

        public IActionResult CategoryProducts(string category)
        {
            var products = _context.Products
                .Where(p => p.Category == category)
                .ToList();

            if (!products.Any())
            {
                return RedirectToAction(nameof(Categories));
            }

            ViewBag.Category = category;
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,Brand,Price,StockQuantity,Size")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet]

        public IActionResult Search(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Content("");
            }

            // Convert search term to lowercase
            string termLower = term.ToLower();

            var results = _context.Products
                .Where(p =>
                    p.Name.ToLower().Contains(termLower) ||
                    p.Category.ToLower().Contains(termLower) ||
                    p.Brand.ToLower().Contains(termLower)
                )
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    category = p.Category,
                    brand = p.Brand,
                    price = p.Price
                })
                .ToList();

            return Json(results);
        }

        public IActionResult FilterProducts(string term)
        {
            // Convert search term to lowercase
            string termLower = term.ToLower();

            var filteredProducts = _context.Products
                .Where(p =>
                    p.Name.ToLower().Contains(termLower) ||
                    p.Category.ToLower().Contains(termLower) ||
                    p.Brand.ToLower().Contains(termLower)
                )
                .ToList();

            return PartialView("_DisplayProducts", filteredProducts);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Brand,Price,StockQuantity,Size")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}


