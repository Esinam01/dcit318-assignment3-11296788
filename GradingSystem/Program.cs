using System;
using System.Collections.Generic;

// Custom Exceptions
public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message) : base(message) { }
}

public class MissingFieldException : Exception
{
    public MissingFieldException(string message) : base(message) { }
}

// Student Class
public class Student
{
    public int Id { get; }
    public string FullName { get; }
    public int Score { get; }

    public Student(int id, string fullName, int score)
    {
        Id = id;
        FullName = fullName;
        Score = score;
    }

    public string GetGrade()
    {
        if (Score >= 80) return "A";
        if (Score >= 70) return "B";
        if (Score >= 60) return "C";
        if (Score >= 50) return "D";
        return "F";
    }

    public override string ToString()
    {
        return $"{FullName} (ID: {Id}) — Score: {Score}, Grade: {GetGrade()}";
    }
}

// Processor
public class StudentResultProcessor
{
    public List<Student> Students = new List<Student>();

    public void AddStudent(int id, string name, string scoreInput)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(scoreInput))
            throw new MissingFieldException("Some fields are missing.");

        if (!int.TryParse(scoreInput, out int score))
            throw new InvalidScoreFormatException("Score must be a valid number.");

        Students.Add(new Student(id, name, score));
    }

    public void PrintAll()
    {
        Console.WriteLine("\n=== STUDENT GRADES ===");
        foreach (var student in Students)
        {
            Console.WriteLine(student);
        }
    }
}

// Main App
class Program
{
    static void Main()
    {
        Console.WriteLine("=== STUDENT GRADING SYSTEM ===");

        StudentResultProcessor processor = new StudentResultProcessor();

        Console.Write("How many students do you want to grade? ");
        int count = int.Parse(Console.ReadLine());

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\n--- Student {i + 1} ---");

            try
            {
                Console.Write("Enter ID: ");
                int id = int.Parse(Console.ReadLine());

                Console.Write("Enter Full Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter Score: ");
                string scoreInput = Console.ReadLine();

                processor.AddStudent(id, name, scoreInput);

                var last = processor.Students[^1]; // Get the just-added student
                Console.WriteLine($"✅ {last.FullName} scored {last.Score} → Grade: {last.GetGrade()}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine("❌ Error: " + ex.Message);
                i--; // retry this student
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine("❌ Error: " + ex.Message);
                i--; // retry this student
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Unexpected error: " + ex.Message);
                i--; // retry this student
            }
        }

        processor.PrintAll();

        Console.WriteLine("\n✅ Done. Press any key to exit...");
        Console.ReadKey();
    }
}
