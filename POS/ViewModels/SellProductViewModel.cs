using POS.DataModels;
using System.ComponentModel.DataAnnotations;

namespace POS.ViewModels
{
    public class SellProductViewModel
    {
        // Product Information
        public int ProductId { get; set; }
        public decimal SellingPrice { get; set; }

        public int QuantityToSell { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }

    }
    public class CustomerBuyHistoryViewModel
    {
        public Customer Customer { get; set; }
        public List<SellHistory> SellHistory { get; set; }
    }


}
