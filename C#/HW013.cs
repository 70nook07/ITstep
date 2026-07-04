// Employee
public class Employee
{
    public string FullName { get; set; }
    public string Position { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public decimal Salary { get; set; }
}

// Firm
public class Firm
{
    public string Name { get; set; }
    public DateTime FoundedDate { get; set; }
    public string Profile { get; set; }
    public string DirectorName { get; set; }
    public int EmployeeCount => Employees.Count;
    public string Address { get; set; }
    public List<Employee> Employees { get; set; } = new List<Employee>();
}

// Phone
public class Phone
{
    public string Name { get; set; }
    public string Manufacturer { get; set; }
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
}

class Program
{
    static void Main()
    {
        // Data populated by AI
        var firms = new List<Firm>
        {
            new Firm { Name = "London IT Food Solutions", FoundedDate = DateTime.Today.AddYears(-3), Profile = "IT", DirectorName = "John White", Address = "London, UK", Employees = new List<Employee> {
                new Employee { FullName = "Lionel Messi", Position = "Manager", Phone = "2345678", Email = "di_leo@firm.com", Salary = 5000 },
                new Employee { FullName = "Alice Smith", Position = "Developer", Phone = "9876543", Email = "alice@firm.com", Salary = 3000 }
            }},
            new Firm { Name = "White Marketing Group", FoundedDate = DateTime.Today.AddDays(-123), Profile = "Marketing", DirectorName = "Sarah Black", Address = "Manchester, UK", Employees = new List<Employee> {
                new Employee { FullName = "Bob Jones", Position = "Manager", Phone = "5551234", Email = "bob@firm.com", Salary = 4500 }
            }},
            new Firm { Name = "Global Food Corp", FoundedDate = DateTime.Today.AddYears(-1), Profile = "Food Production", DirectorName = "Robert Brown", Address = "London, UK", Employees = Enumerable.Range(1, 150).Select(i => new Employee { FullName = $"Emp {i}", Position = "Staff", Phone = "111", Email = "staff@firm.com", Salary = 1500 }).ToList() }
        };

        // Data populated by AI
        var phones = new List<Phone>
        {
            new Phone { Name = "iPhone 15", Manufacturer = "Apple", Price = 999, ReleaseDate = new DateTime(2023, 9, 22) },
            new Phone { Name = "Galaxy S23", Manufacturer = "Samsung", Price = 799, ReleaseDate = new DateTime(2023, 2, 17) },
            new Phone { Name = "Pixel 7a", Manufacturer = "Google", Price = 499, ReleaseDate = new DateTime(2023, 5, 10) },
            new Phone { Name = "Redmi Note 12", Manufacturer = "Xiaomi", Price = 150, ReleaseDate = new DateTime(2022, 10, 27) }
        };

        // TASK 1: Firm
        
        Console.WriteLine("\tTASK 1: FIRM QUERIES\n");

        PrintFirms("All firms:", firms);
        PrintFirms("Firms with 'Food' in name:", firms.Where(f => f.Name.Contains("Food", StringComparison.OrdinalIgnoreCase)));
        PrintFirms("Firms in Marketing:", firms.Where(f => f.Profile.Equals("Marketing", StringComparison.OrdinalIgnoreCase)));
        PrintFirms("Firms in Marketing or IT:", firms.Where(f => f.Profile.Equals("Marketing", StringComparison.OrdinalIgnoreCase) || f.Profile.Equals("IT", StringComparison.OrdinalIgnoreCase)));
        PrintFirms("Firms with employees > 100:", firms.Where(f => f.EmployeeCount > 100));
        PrintFirms("Firms with employees 100 to 300:", firms.Where(f => f.EmployeeCount >= 100 && f.EmployeeCount <= 300));
        PrintFirms("Firms in London:", firms.Where(f => f.Address.Contains("London", StringComparison.OrdinalIgnoreCase)));
        PrintFirms("Firms with Director White:", firms.Where(f => f.DirectorName.EndsWith("White", StringComparison.OrdinalIgnoreCase)));
        PrintFirms("Firms founded > 2 years ago:", firms.Where(f => f.FoundedDate < DateTime.Today.AddYears(-2)));
        PrintFirms("Firms founded exactly 123 days ago:", firms.Where(f => (DateTime.Today - f.FoundedDate.Date).Days == 123));
        PrintFirms("Director Black & Firm Name contains White:", firms.Where(f => f.DirectorName.EndsWith("Black", StringComparison.OrdinalIgnoreCase) && f.Name.Contains("White", StringComparison.OrdinalIgnoreCase)));

        // TASK 2: Employee
        
        Console.WriteLine("\n\tTASK 2: EMPLOYEE QUERIES\n");

        var targetFirm = firms[0];
        PrintEmployees($"Employees of {targetFirm.Name}:", targetFirm.Employees);
        PrintEmployees($"Employees of {targetFirm.Name} with salary > 4000:", targetFirm.Employees.Where(e => e.Salary > 4000));
        PrintEmployees("Managers across all firms:", firms.SelectMany(f => f.Employees).Where(e => e.Position.Equals("Manager", StringComparison.OrdinalIgnoreCase)));
        PrintEmployees("Employees with phone starting with 23:", firms.SelectMany(f => f.Employees).Where(e => e.Phone.StartsWith("23")));
        PrintEmployees("Employees with email starting with di:", firms.SelectMany(f => f.Employees).Where(e => e.Email.StartsWith("di", StringComparison.OrdinalIgnoreCase)));
        PrintEmployees("Employees named Lionel:", firms.SelectMany(f => f.Employees).Where(e => e.FullName.StartsWith("Lionel", StringComparison.OrdinalIgnoreCase)));

        // TASK 3: Phone
        
        Console.WriteLine("\n\tTASK 3: PHONE AGGREGATE QUERIES\n");

        Console.WriteLine($"Total phones: {phones.Count()}");
        Console.WriteLine($"Phones with price > 100: {phones.Count(p => p.Price > 100)}");
        Console.WriteLine($"Phones with price 400 to 700: {phones.Count(p => p.Price >= 400 && p.Price <= 700)}");
        
        string targetBrand = "Apple";
        Console.WriteLine($"Phones manufactured by {targetBrand}: {phones.Count(p => p.Manufacturer.Equals(targetBrand, StringComparison.OrdinalIgnoreCase))}");

        var minPricePhone = phones.MinBy(p => p.Price);
        Console.WriteLine($"Phone with min price: {minPricePhone?.Name} (${minPricePhone?.Price})");

        var maxPricePhone = phones.MaxBy(p => p.Price);
        Console.WriteLine($"Phone with max price: {maxPricePhone?.Name} (${maxPricePhone?.Price})");

        var oldestPhone = phones.MinBy(p => p.ReleaseDate);
        Console.WriteLine($"Oldest phone: {oldestPhone?.Name} (Released: {oldestPhone?.ReleaseDate.ToShortDateString()})");

        var newestPhone = phones.MaxBy(p => p.ReleaseDate);
        Console.WriteLine($"Newest phone: {newestPhone?.Name} (Released: {newestPhone?.ReleaseDate.ToShortDateString()})");

        Console.WriteLine($"Average phone price: ${phones.Average(p => p.Price):F2}");
    }

    // Additional Methos
    static void PrintFirms(string header, IEnumerable<Firm> result)
    {
        Console.WriteLine($"-- {header}");
        foreach (var f in result) Console.WriteLine($" - {f.Name} (Dir: {f.DirectorName}, Profile: {f.Profile}, Emps: {f.EmployeeCount}, Loc: {f.Address})");
        if (!result.Any()) Console.WriteLine(" [No matches]");
        Console.WriteLine();
    }

    static void PrintEmployees(string header, IEnumerable<Employee> result)
    {
        Console.WriteLine($"-- {header}");
        foreach (var e in result) Console.WriteLine($" - {e.FullName} ({e.Position}, Phone: {e.Phone}, Email: {e.Email}, Sal: ${e.Salary})");
        if (!result.Any()) Console.WriteLine(" [No matches]");
        Console.WriteLine();
    }
}
