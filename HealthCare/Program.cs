//using System;
using System.Collections.Generic;
using System.Linq;

// Generic Repository
public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item) => items.Add(item);

    public List<T> GetAll() => new List<T>(items);

    public T? GetById(Func<T, bool> predicate) => items.FirstOrDefault(predicate);

    public bool Remove(Func<T, bool> predicate)
    {
        var item = items.FirstOrDefault(predicate);
        if (item != null)
        {
            items.Remove(item);
            return true;
        }
        return false;
    }
}

// Patient Class
public class Patient
{
    public int Id { get; }
    public string Name { get; }
    public int Age { get; }
    public string Gender { get; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name;
        Age = age;
        Gender = gender;
    }

    public override string ToString() =>
        $"ID: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}";
}

// Prescription Class
public class Prescription
{
    public int Id { get; }
    public int PatientId { get; }
    public string MedicationName { get; }
    public DateTime DateIssued { get; }

    public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        DateIssued = dateIssued;
    }

    public override string ToString() =>
        $"ID: {Id}, Medication: {MedicationName}, Issued: {DateIssued:d}";
}

// Healthcare System
public class HealthSystemApp
{
    private Repository<Patient> _patientRepo = new Repository<Patient>();
    private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();
    private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

    public void Run()
    {
        Console.WriteLine("=== HEALTHCARE SYSTEM ===\n");

        // 1. Add Patients
        Console.Write("How many patients do you want to add? ");
        int patientCount = int.Parse(Console.ReadLine());
        for (int i = 0; i < patientCount; i++)
        {
            Console.WriteLine($"\nEnter Patient #{i + 1} details:");

            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());

            Console.Write("Gender: ");
            string gender = Console.ReadLine();

            _patientRepo.Add(new Patient(id, name, age, gender));
        }

        // 2. Add Prescriptions
        Console.Write("\nHow many prescriptions do you want to add? ");
        int prescriptionCount = int.Parse(Console.ReadLine());
        for (int i = 0; i < prescriptionCount; i++)
        {
            Console.WriteLine($"\nEnter Prescription #{i + 1} details:");

            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Patient ID: ");
            int patientId = int.Parse(Console.ReadLine());

            Console.Write("Medication Name: ");
            string name = Console.ReadLine();

            DateTime today = DateTime.Now;
            _prescriptionRepo.Add(new Prescription(id, patientId, name, today));
        }

        // 3. Build Prescription Map
        foreach (var p in _prescriptionRepo.GetAll())
        {
            if (!_prescriptionMap.ContainsKey(p.PatientId))
                _prescriptionMap[p.PatientId] = new List<Prescription>();

            _prescriptionMap[p.PatientId].Add(p);
        }

        // 4. Print all patients
        Console.WriteLine("\n=== PATIENT LIST ===");
        foreach (var patient in _patientRepo.GetAll())
        {
            Console.WriteLine(patient);
        }

        // 5. Ask for a PatientId and display prescriptions
        Console.Write("\nEnter Patient ID to view prescriptions: ");
        int selectedId = int.Parse(Console.ReadLine());

        if (_prescriptionMap.ContainsKey(selectedId))
        {
            Console.WriteLine($"\nPrescriptions for Patient ID {selectedId}:");
            foreach (var pres in _prescriptionMap[selectedId])
            {
                Console.WriteLine(pres);
            }
        }
        else
        {
            Console.WriteLine("No prescriptions found for this patient.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}

// Main Program
class Program
{
    static void Main()
    {
        HealthSystemApp app = new HealthSystemApp();
        app.Run();
    }
}
