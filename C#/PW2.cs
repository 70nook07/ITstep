using System.Text.RegularExpressions;

public class Worker
{
    private string? _fullName;
    private int _age;
    private decimal _salary;
    private DateTime _hireDate;
    
    public string? FullName
    {
        get => _fullName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Full name cannot be empty.");
            
            string pattern = @"^[A-Z][a-zA-Z]*(-[A-Z][a-zA-Z]*)? [A-Z]\.[A-Z]\.$";  // example: Vasyl-Ivan O.B.
            if (!Regex.IsMatch(value, pattern))
                throw new ArgumentException(
                    "Full name must be in the format 'Surname I.I.");

            _fullName = value;
        }
    }

    public int Age
    {
        get => _age;
        set
        {
            if (value < 16 || value > 100)
                throw new ArgumentOutOfRangeException(
                    nameof(value), "Age must be between 16 and 100.");
            _age = value;
        }
    }

    public decimal Salary
    {
        get => _salary;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(value), "Salary must be greater than zero.");
            _salary = value;
        }
    }

    public DateTime HireDate
    {
        get => _hireDate;
        set
        {
            if (value.Date > DateTime.Today)
                throw new ArgumentOutOfRangeException(
                    nameof(value), "Hire date cannot be in the future.");
            _hireDate = value;
        }
    }

    public Worker() { }

    public Worker(string fullName, int age, decimal salary, DateTime hireDate)
    {
        FullName = fullName;
        Age = age;
        Salary = salary;
        HireDate = hireDate;
    }

    public override string ToString() =>
        $"{FullName}, Age: {Age}, Salary: {Salary:C}, Hired: {HireDate:yyyy-MM-dd}";
}

class Program
{
    static void Main()
    {
        const int workerCount = 5;
        Worker[] workers = new Worker[workerCount];

        Console.WriteLine($"Enter data for {workerCount} workers.");
        Console.WriteLine("Full name format: Surname I.I. (e.g. Smith J.K.)");
        Console.WriteLine("Hire date format: yyyy-mm-dd");
        Console.WriteLine();

        for (int i = 0; i < workerCount; i++)
        {
            Console.WriteLine($"--- Worker #{i + 1} ---");
            Worker w = new Worker();

            // Name
            while (true)
            {
                try
                {
                    Console.Write("Full name (Surname I.I.): ");
                    w.FullName = Console.ReadLine();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message} Please try again.");
                }
            }
            
            // Age
            while (true)
            {
                try
                {
                    Console.Write("Age: ");
                    if (!int.TryParse(Console.ReadLine(), out int age))
                        throw new FormatException("Age must be an integer.");
                    w.Age = age;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message} Please try again.");
                }
            }

            // Salary
            while (true)
            {
                try
                {
                    Console.Write("Salary: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
                        throw new FormatException("Salary must be a decimal number.");
                    w.Salary = salary;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message} Please try again.");
                }
            }

            // HireDate
            while (true)
            {
                try
                {
                    Console.Write("Hire date (yyyy-mm-dd): ");
                    string? input = Console.ReadLine();
                    if (!DateTime.TryParseExact(input, "yyyy-MM-dd", null,
                        System.Globalization.DateTimeStyles.None, out DateTime date))
                        throw new FormatException("Date must be in yyyy-MM-dd format.");
                    w.HireDate = date;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message} Please try again.");
                }
            }

            workers[i] = w;
            Console.WriteLine();
        }
        
        // Sort ABC
        Array.Sort(workers, (a, b) => string.Compare(a.FullName, b.FullName, StringComparison.OrdinalIgnoreCase));

        Console.WriteLine("\nWorkers sorted alphabetically:");
        foreach (var worker in workers)
            Console.WriteLine(worker);
        
        // Exp
        Console.Write("\nEnter minimum work experience in years: ");
        int minYears;
        while (!int.TryParse(Console.ReadLine(), out minYears) || minYears < 0)
        {
            Console.Write("Invalid input. Enter a non-negative integer: ");
        }

        DateTime today = DateTime.Today;
        Console.WriteLine($"\nWorkers with more than {minYears} year(s) of experience:");
        bool found = false;
        foreach (var worker in workers)
        {
            // Years of exp (days ignored)
            int experience = today.Year - worker.HireDate.Year;
            if (worker.HireDate.Date > today.AddYears(-experience))
                experience--;

            if (experience > minYears)
            {
                Console.WriteLine(worker.FullName);
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("None.");

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
