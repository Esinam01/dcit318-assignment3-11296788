using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// 1. Marker Interface
public interface IInventoryEntity
{
    int Id { get; }
}

// 2. Record for InventoryItem
public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

// 3. Generic Inventory Logger
public class InventoryLogger<T> where T : IInventoryEntity
{
    private List<T> _log = new List<T>();
    private string _filePath;

    public InventoryLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Add(T item)
    {
        _log.Add(item);
    }

    public List<T> GetAll() => new List<T>(_log);

    public void SaveToFile()
    {
        try
        {
            string json = JsonSerializer.Serialize(_log, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
            Console.WriteLine("✅ Data saved to file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error saving to file: " + ex.Message);
        }
    }

    public void LoadFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("❌ File does not exist.");
                return;
            }

            string json = File.ReadAllText(_filePath);
            _log = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            Console.WriteLine("✅ Data loaded from file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error loading file: " + ex.Message);
        }
    }
}

// 4. Main App
public class InventoryApp
{
    private InventoryLogger<InventoryItem> _logger;

    public InventoryApp(string filePath)
    {
        _logger = new InventoryLogger<InventoryItem>(filePath);
    }

    public void SeedSampleData()
    {
        Console.Write("How many inventory items do you want to add? ");
        int count = int.Parse(Console.ReadLine());

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\n--- Item {i + 1} ---");

            Console.Write("Enter ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Quantity: ");
            int qty = int.Parse(Console.ReadLine());

            var item = new InventoryItem(id, name, qty, DateTime.Now);
            _logger.Add(item);
        }
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        var items = _logger.GetAll();

        Console.WriteLine("\n=== INVENTORY RECORDS ===");
        foreach (var item in items)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Qty: {item.Quantity}, Added: {item.DateAdded}");
        }
    }
}

// 5. Entry Point
class Program
{
    static void Main()
    {
        string filePath = "inventory.json";

        InventoryApp app = new InventoryApp(filePath);

        Console.WriteLine("=== INVENTORY LOGGER ===");

        app.SeedSampleData();
        app.SaveData();

        Console.WriteLine("\nMemory cleared... Now simulating data load from file...\n");

        // simulate new session by creating a new instance
        app = new InventoryApp(filePath);
        app.LoadData();
        app.PrintAllItems();

        Console.WriteLine("\n✅ Done. Press any key to exit...");
        Console.ReadKey();
    }
}
