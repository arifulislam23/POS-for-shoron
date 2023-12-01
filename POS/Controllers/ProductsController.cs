using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.DataModels;

namespace POS.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
              return _context.Products != null ? 
                          View(await _context.Products.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
         .Include(p => p.Stock)
         .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if(product.Name != null ||product.Details !=null)
            {
                if (_context.Products.Any(p => p.Name == product.Name))
                {
                    ModelState.AddModelError("Name", "Product name already exists.");
                    return View(product);
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
            
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products
                .Include(p => p.Stock)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (product.Name != null)
            {
                try
                {
                    var existingProduct = await _context.Products
                        .Include(p => p.Stock)
                        .FirstOrDefaultAsync(p => p.ProductId == id);

                    if (existingProduct == null)
                    {
                        return NotFound();
                    }
                    existingProduct.Name = product.Name;
                    existingProduct.Details = product.Details;
                    existingProduct.BuyPrice = product.BuyPrice;
                    existingProduct.ExpectedSellPrice = product.ExpectedSellPrice;
                    if (existingProduct.Stock == null)
                    {
                        existingProduct.Stock = new ProductStock
                        {
                            Quantity = product.Stock.Quantity
                        };
                    }
                    else
                    {
                        existingProduct.Stock.Quantity = product.Stock.Quantity;
                    }

                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
        .Include(p => p.Stock)
        .FirstOrDefaultAsync(m => m.ProductId == id);
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
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
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
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
