using LiquorStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace LiquorStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly LSContext _context;

        public ProductController(LSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult ProductList()
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
        public async Task<IActionResult> Create(Product product)
        {
            Console.WriteLine("ImageFile: " + (product.ImageFile != null ? product.ImageFile.FileName : "null"));
            ModelState.Remove("Image");
            if (product.ImageFile != null && product.ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await product.ImageFile.CopyToAsync(memoryStream);
                    product.Image = memoryStream.ToArray();
                }
            }
            else
            {
                ModelState.AddModelError("Image", "Image is required.");
            }

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
        public async Task<IActionResult> Edit(int id, Product product, IFormFile ImageFile)

        {


            if (id != product.Id)
            {
                return NotFound();
            }

            var productToUpdate = await _context.Products.FindAsync(id);
            if (productToUpdate == null)
            {
                return NotFound();
            }

            ModelState.Remove("Image");

            productToUpdate.Name = product.Name;
            productToUpdate.Category = product.Category;
            productToUpdate.Brand = product.Brand;
            productToUpdate.Price = product.Price;
            productToUpdate.StockQuantity = product.StockQuantity;
            productToUpdate.Size = product.Size;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
                if (!allowedTypes.Contains(ImageFile.ContentType.ToLower()))
                {
                    ModelState.AddModelError("Image", "Only JPG and PNG files are allowed.");
                    return View(productToUpdate);
                }

                using (var ms = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(ms);
                    productToUpdate.Image = ms.ToArray();
                }

                _context.Entry(productToUpdate).Property(p => p.Image).IsModified = true;
            }
            Console.WriteLine("Image length: " + (productToUpdate.Image?.Length ?? 0));
            
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("Image length: " + (productToUpdate.Image?.Length ?? 0));
                    _context.Update(productToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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
            }

            return View(productToUpdate);
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


