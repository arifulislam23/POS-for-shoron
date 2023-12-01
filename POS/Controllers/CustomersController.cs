using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.DataModels;
using POS.ViewModels;

namespace POS.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult CustomerList()
        {
            var customers = _context.Customers.ToList();
            return View(customers);
        }
        public IActionResult Update(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        [HttpPost]
        public IActionResult Update(Customer customer)
        {
            if (customer.CustomerId <= 0)
            {
                return NotFound();
            }
            var existingCustomer = _context.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
            if (existingCustomer == null)
            {
                return NotFound();
            }
            var isPhoneNumberDuplicate = _context.Customers.Any(c => c.CustomerId != customer.CustomerId && c.PhoneNumber == customer.PhoneNumber);
            if (isPhoneNumberDuplicate)
            {
                ModelState.AddModelError(nameof(customer.PhoneNumber), "Phone number is already in use by another customer.");
                return View(customer);
            }
            if (string.IsNullOrWhiteSpace(customer.CustomerName) || string.IsNullOrWhiteSpace(customer.PhoneNumber))
            {
                ModelState.AddModelError("", "Customer name and phone number are required.");
                return View(customer);
            }
            existingCustomer.CustomerName = customer.CustomerName;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.Email= customer.Email;
            existingCustomer.Address = customer.Address;
            _context.SaveChanges();
            return RedirectToAction("CustomerList");
        }
        public IActionResult BuyHistory(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var customerWithHistory = _context.Customers
                .Include(c => c.SellHistory)
                .ThenInclude(sh => sh.Product)
                .FirstOrDefault(c => c.CustomerId == id);

            if (customerWithHistory == null)
            {
                return NotFound();
            }
            var viewModel = new CustomerBuyHistoryViewModel
            {
                Customer = customerWithHistory,
                SellHistory = customerWithHistory.SellHistory.ToList()
            };

            return View(viewModel);
        }



    }
}
