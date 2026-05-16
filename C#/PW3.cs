namespace CSharp;

public class PW3
{
    static void Main()
    {
        Console.WriteLine("Choose a task:");
        Console.WriteLine("1 - String convertor to int");
        Console.WriteLine("2 - Mult expression");
        Console.Write("Choose: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Task1();
                break;
            case "2":
                Task2();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

    static void Task1()
    {
        Console.Write("Enter a string of digits (0-9): ");
        string input = Console.ReadLine();

        try
        {
            int number = int.Parse(input);
            Console.WriteLine($"Converted number: {number}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: input contains non-digit characters or is empty.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Error: int out of range.");
        }
    }

    static void Task2()
    {
        Console.Write("Enter a multiplication expression (e.g., 2*1*3*4): ");
        string expr = Console.ReadLine();

        try
        {
            string[] whole = expr.Split('*');
            int result = 1;

            foreach (string piece in whole)
            {
                int num = int.Parse(piece);
                result *= num;
            }

            Console.WriteLine($"Result: {result}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: input contains non-digit values or is empty.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Error: int out of range.");
        }
    }
}
