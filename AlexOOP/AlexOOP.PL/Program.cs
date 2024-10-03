using System;
using AlexOOP.BLL;
 
namespace AlexOOP.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a customer
            Shopper shopper = new Shopper("Jane", "Doe");
 
            // Create products
            PhysicalItem desktop = new PhysicalItem("Desktop Computer", 1200.00m, 10);
            DigitalItem software = new DigitalItem("Antivirus Software", 50.00m, 5);
 
            // Subscribe to stock alert events
            desktop.StockAlert += message => Console.WriteLine($"[ALERT] {message}");
            software.StockAlert += message => Console.WriteLine($"[ALERT] {message}");
 
            // Create orders
            Purchase desktopOrder = new PhysicalPurchase();
            Purchase softwareOrder = new DigitalPurchase();
 
            // Process first order for desktop with a discount
            if (desktopOrder.InitiateOrder(shopper, desktop, 3))
            {
                desktopOrder.UseDiscount(new PercentageDiscount(15));
                desktopOrder.FinalizeOrder();
            }
 
            Console.WriteLine();
 
            // Process second order for software with a fixed discount
            if (softwareOrder.InitiateOrder(shopper, software, 4))
            {
                softwareOrder.UseDiscount(new AmountDiscount(10));
                softwareOrder.FinalizeOrder();
            }
 
            // Try to place another desktop order that exceeds stock
            if (!desktopOrder.InitiateOrder(shopper, desktop, 8))
            {
                Console.WriteLine("Failed to process another desktop order due to stock limitations.");
            }
        }
    }
}