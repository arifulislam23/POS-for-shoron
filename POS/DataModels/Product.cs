namespace POS.DataModels
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public decimal BuyPrice { get; set; } //buying price
        public decimal ExpectedSellPrice { get; set; } //expected sell price
        public virtual ProductStock Stock { get; set; }
        public virtual ICollection<SellHistory> SellHistory { get; set; }
    }

    public class ProductStock
    {
        public int ProductStockId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
    }
    public class SellHistory
    {
        public int SellHistoryId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal SellPrice { get; set; } // Selling price
        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; } // Reference to the associated customer
        public DateTime CreateAt { get; set; }
    }


}
