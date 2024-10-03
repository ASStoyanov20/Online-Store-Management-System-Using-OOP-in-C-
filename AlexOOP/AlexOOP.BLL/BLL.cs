using System;
 
namespace AlexOOP.BLL
{
    // Base class for all product types
    public abstract class StoreItem
    {
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int AvailableStock { get; private set; }
        public event Action<string> StockAlert;
 
        protected StoreItem(string productName, decimal price, int stock)
        {
            ProductName = productName;
            UnitPrice = price;
            AvailableStock = stock;
        }
 
        // Abstract method for displaying product details
        public abstract void ShowProductInfo();
 
        // Stock management method
        public void DecreaseStock(int quantity)
        {
            if (AvailableStock >= quantity)
            {
                AvailableStock -= quantity;
                if (AvailableStock == 0)
                {
                    StockAlert?.Invoke($"{ProductName} is out of stock!");
                }
            }
            else
            {
                throw new InvalidOperationException($"Not enough stock for {ProductName}. Current stock: {AvailableStock}");
            }
        }
    }
 
    // Class for physical products
    public class PhysicalItem : StoreItem
    {
        public PhysicalItem(string productName, decimal price, int stock) : base(productName, price, stock) { }
 
        public override void ShowProductInfo()
        {
            Console.WriteLine($"Physical Product: {ProductName}, Price: {UnitPrice}, In Stock: {AvailableStock}");
        }
    }
 
    // Class for digital products
    public class DigitalItem : StoreItem
    {
        public DigitalItem(string productName, decimal price, int stock) : base(productName, price, stock) { }
 
        public override void ShowProductInfo()
        {
            Console.WriteLine($"Digital Product: {ProductName}, Price: {UnitPrice}, In Stock: {AvailableStock}");
        }
    }
 
    // Class representing a customer
    public class Shopper
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
 
        public Shopper(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
 
        public void DisplayShopperInfo()
        {
            Console.WriteLine($"Customer: {FirstName} {LastName}");
        }
    }
 
    // Base class for handling orders
    public abstract class Purchase
    {
        protected Shopper Buyer { get; private set; }
        protected StoreItem Item { get; private set; }
        protected int Quantity { get; private set; }
        protected decimal TotalPrice { get; private set; }
 
        // Method for initiating an order
        public bool InitiateOrder(Shopper buyer, StoreItem item, int quantity)
        {
            if (item.AvailableStock >= quantity)
            {
                Buyer = buyer;
                Item = item;
                Quantity = quantity;
                TotalPrice = item.UnitPrice * quantity;
                Console.WriteLine($"Order placed for {Quantity} unit(s) of {Item.ProductName}.");
                return true;
            }
            else
            {
                Console.WriteLine($"Insufficient stock for {Item.ProductName}. Stock available: {Item.AvailableStock}");
                return false;
            }
        }
 
        // Apply discount
        public void UseDiscount(IDiscount discount)
        {
            TotalPrice = discount.CalculateDiscount(TotalPrice);
            Console.WriteLine($"Discount applied. Total price: ${TotalPrice}");
        }
 
        // Finalize the order
        public void FinalizeOrder()
        {
            Item.DecreaseStock(Quantity);
            Console.WriteLine($"{Quantity} unit(s) of {Item.ProductName} deducted from stock.");
            Console.WriteLine($"Order for {Buyer.FirstName} {Buyer.LastName} completed.");
            ProcessOrder();
        }
 
        // Abstract method for processing the order based on product type
        protected abstract void ProcessOrder();
    }
 
    // Concrete class for physical product orders
    public class PhysicalPurchase : Purchase
    {
        protected override void ProcessOrder()
        {
            Console.WriteLine("Item shipped to customer.");
        }
    }
 
    // Concrete class for digital product orders
    public class DigitalPurchase : Purchase
    {
        protected override void ProcessOrder()
        {
            Console.WriteLine("Download link sent to customer.");
        }
    }
 
    // Discount interface
    public interface IDiscount
    {
        decimal CalculateDiscount(decimal originalPrice);
    }
 
    // Fixed amount discount
    public class AmountDiscount : IDiscount
    {
        private decimal DiscountValue;
 
        public AmountDiscount(decimal discountValue)
        {
            DiscountValue = discountValue;
        }
 
        public decimal CalculateDiscount(decimal originalPrice)
        {
            return originalPrice - DiscountValue;
        }
    }
 
    // Percentage-based discount
    public class PercentageDiscount : IDiscount
    {
        private decimal DiscountRate;
 
        public PercentageDiscount(decimal discountRate)
        {
            DiscountRate = discountRate;
        }
 
        public decimal CalculateDiscount(decimal originalPrice)
        {
            return originalPrice - (originalPrice * DiscountRate / 100);
        }
    }
}