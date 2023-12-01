using System.ComponentModel.DataAnnotations;

namespace POS.DataModels
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public virtual ICollection<SellHistory> SellHistory { get; set; }
    }

}
