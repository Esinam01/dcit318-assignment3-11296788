using System;
using System.Collections.Generic;

// 1. Marker Interface
public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int Quantity { get; set; }
}

// 2a. Electronic Item
public class ElectronicItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public string Brand { get; }
    public int WarrantyMonths { get; }

    public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Brand = brand;
        WarrantyMonths = warrantyMonths;
    }

    public override string ToString() =>
        $"[Electronic] {Name} (ID: {Id}, Brand: {Brand}, Quantity: {Quantity}, Warranty: {WarrantyMonths} months)";
}

// 2b. Grocery Item
public class GroceryItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; }

    public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        ExpiryDate = expiryDate;
    }

    public override string ToString() =>
        $"[Grocery] {Name} (ID: {Id}, Quantity: {Quantity}, Expires: {ExpiryDate:dd-MM-yyyy})";
}

// 3. Custom Exceptions
public class DuplicateItemException : Exception
{
    public DuplicateItemException(string message) : base(message) { }
}

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message) { }
}

public class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string message) : base(message) { }
}

// 4. Generic Repository
public class InventoryRepository<T> where T : IInventoryItem
{
    private Dictionary<int, T> _items = new Dictionary<int, T>();

    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id))
            throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
        _items[item.Id] = item;
    }

    public T GetItemById(int id)
    {
        if (_items.TryGetValue(id, out T item))
            return item;
        throw new ItemNotFoundException($"Item with ID {id} not found.");
    }

    public void RemoveItem(int id)
    {
        if (!_items.Remove(id))
            throw new ItemNotFoundException($"Item with ID {id} not found for removal.");
    }

    public List<T> GetAllItems() => new List<T>(_items.Values);

    public void UpdateQuantity(int id, int newQuantity)
    {
        if (newQuantity < 0)
            throw new InvalidQuantityException("Quantity cannot be negative.");

        var item = GetItemById(id);
        item.Quantity = newQuantity;
    }
}

// 5. Warehouse Manager
public class WareHouseManager
{
    private InventoryRepository<ElectronicItem> _electronics = new InventoryRepository<ElectronicItem>();
    private InventoryRepository<GroceryItem> _groceries = new InventoryRepository<GroceryItem>();

    public void Run()
    {
        Console.WriteLine("=== WAREHOUSE SYSTEM ===\n");

        // Add electronics
        Console.Write("How many electronic items do you want to add? ");
        int eleCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < eleCount; i++)
        {
            try
            {
                Console.WriteLine($"\nEnter Electronic Item #{i + 1}");

                Console.Write("ID: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Quantity: ");
                int qty = int.Parse(Console.ReadLine());

                Console.Write("Brand: ");
                string brand = Console.ReadLine();

                Console.Write("Warranty (months): ");
                int warranty = int.Parse(Console.ReadLine());

                _electronics.AddItem(new ElectronicItem(id, name, qty, brand, warranty));
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ " + ex.Message);
            }
        }

        // Add groceries
        Console.Write("\nHow many grocery items do you want to add? ");
        int groCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < groCount; i++)
        {
            try
            {
                Console.WriteLine($"\nEnter Grocery Item #{i + 1}");

                Console.Write("ID: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Quantity: ");
                int qty = int.Parse(Console.ReadLine());

                Console.Write("Expiry Date (yyyy-mm-dd): ");
                DateTime expiry = DateTime.Parse(Console.ReadLine());

                _groceries.AddItem(new GroceryItem(id, name, qty, expiry));
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ " + ex.Message);
            }
        }

        // Print all items
        Console.WriteLine("\n--- Electronic Inventory ---");
        PrintAllItems(_electronics);

        Console.WriteLine("\n--- Grocery Inventory ---");
        PrintAllItems(_groceries);

        // Update, Remove, and Error Demos
        Console.WriteLine("\n--- Operations ---");
        try
        {
            Console.Write("Enter electronic ID to increase quantity: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter quantity to add: ");
            int qty = int.Parse(Console.ReadLine());

            var item = _electronics.GetItemById(id);
            _electronics.UpdateQuantity(id, item.Quantity + qty);

            Console.WriteLine($"Updated: {item.Name} now has {item.Quantity} units.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ " + ex.Message);
        }

        try
        {
            Console.Write("Enter grocery ID to remove: ");
            int id = int.Parse(Console.ReadLine());

            _groceries.RemoveItem(id);
            Console.WriteLine("Item removed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ " + ex.Message);
        }

        try
        {
            Console.Write("Enter electronic ID to set negative quantity (test error): ");
            int id = int.Parse(Console.ReadLine());

            _electronics.UpdateQuantity(id, -5);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ " + ex.Message);
        }

        Console.WriteLine("\n✅ Done. Press any key to exit...");
        Console.ReadKey();
    }

    public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
    {
        foreach (var item in repo.GetAllItems())
        {
            Console.WriteLine(item);
        }
    }
}

// 6. Main Entry Point
class Program
{
    static void Main()
    {
        var manager = new WareHouseManager();
        manager.Run();
    }
}
