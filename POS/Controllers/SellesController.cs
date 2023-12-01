using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.DataModels;
using POS.ViewModels;

namespace POS.Controllers
{
    [Authorize]
    public class SellesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SellesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Sell()
        {
            var products = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select Product", Disabled = true, Selected = true }
            };

            products.AddRange(_context.Products
                .Select(p => new SelectListItem
                {
                    Value = p.ProductId.ToString(),
                    Text = $"{p.ProductId} - {p.Name} ({(p.Stock != null ? p.Stock.Quantity : 0)} in stock)"
                })
                .ToList());


            ViewBag.Products = new SelectList(products, "Value", "Text");
            return View();
        }

        [HttpPost]
        public IActionResult SellItem([FromBody] SellProductViewModel model)
        {
           
            if  (model ==null || model.ProductId == 0 ||
                string.IsNullOrEmpty(model.CustomerName) ||
                string.IsNullOrEmpty(model.CustomerPhoneNumber) ||
                model.QuantityToSell <= 0 ||
                model.SellingPrice <= 0)
            {
                return Json(new { status = false, message = "Please give the required information." });
            }
            try
            {
                var productStock = _context.ProductStocks.FirstOrDefault(ps => ps.ProductId == model.ProductId);

                if (productStock != null && productStock.Quantity >= model.QuantityToSell)
                {
                    var existingCustomer = _context.Customers.FirstOrDefault(c => c.PhoneNumber == model.CustomerPhoneNumber);

                    if (existingCustomer == null)
                    {
                        var newCustomer = new Customer
                        {
                            CustomerName = model.CustomerName,
                            PhoneNumber = model.CustomerPhoneNumber,
                            Email = model.CustomerEmail,
                            Address = model.Address
                        };

                        _context.Customers.Add(newCustomer);
                        _context.SaveChanges();
                        existingCustomer = newCustomer;
                    }
                    var sellHistory = new SellHistory
                    {
                        ProductId = model.ProductId,
                        CustomerId = existingCustomer.CustomerId,
                        Quantity = model.QuantityToSell,
                        SellPrice = model.SellingPrice,
                        CreateAt = DateTime.Now

                    };

                    _context.SellHistories.Add(sellHistory);
                    productStock.Quantity -= model.QuantityToSell;

                    _context.SaveChanges();

                    return Json(new { success = true, message = "Sale recorded successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Insufficient stock for the selected product." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error recording sale. " + ex.Message });
            }

        }
        [HttpGet]
        public IActionResult GetSellingPrice(int productId)
        {
            var sellingPrice = _context.Products.FirstOrDefault(p => p.ProductId == productId)?.ExpectedSellPrice;

            return Json(sellingPrice);
        }
        [HttpGet]
        public IActionResult GetCustomerSuggestions(string term)
        {
            if (term != null)
            {
                var suggestions = _context.Customers
                    .Where(c => c.CustomerName.Contains(term) || c.PhoneNumber.Contains(term))
                    .Select(c => new
                    {
                        label = $"{c.CustomerName} ({c.PhoneNumber})",
                        value = c.CustomerName,
                        phoneNumber = c.PhoneNumber,
                        email = c.Email,
                        address = c.Address
                    })
                    .ToList();

                return Json(suggestions);
            }
            return Ok();
        }

        public IActionResult SalesHistory()
        {
            var salesHistory = _context.SellHistories
                .Include(sh => sh.Product)
                .Include(sh => sh.Customer)
                .OrderByDescending(x=>x.CreateAt)
                .ToList();
            foreach (var sale in salesHistory)
            {
                if (sale.Product == null)
                {
                    sale.Product = new Product { Name = "Product Not Available" };
                }

                if (sale.Customer == null)
                {
                    sale.Customer = new Customer { CustomerName = "Customer Not Available" };
                }
            }
            return View(salesHistory);
        }



    }
}
