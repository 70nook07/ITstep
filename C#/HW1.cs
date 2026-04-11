namespace HW1;

public class Program
{
    static void Task1()
    {
        int[] A = new int[5];
        double[,] B = new double[3, 4];
        Random rand = new Random();
        double sum = 0;
        double product = 1;
        double max = A[0];
        double min = A[0];
        int sumEvenA = 0;
        double sumOddColsB = 0;
        
        Console.WriteLine("Enter 5 integers:");
        for (int i = 0; i < A.Length; i++)
        {
            while (!int.TryParse(Console.ReadLine(), out A[i]))
            {
                Console.Write("Incorrect integer, try again: ");
            }
        }

        for (int i = 0; i < B.GetLength(0); i++)
        {
            for (int j = 0; j < B.GetLength(1); j++)
            {
                B[i, j] = rand.Next(-10, 10) + rand.NextDouble();
            }
        }

        Console.WriteLine("\nArray A:");
        foreach (var x in A)
            Console.Write($"{x} ");

        Console.WriteLine("\n\nArray B:");
        for (int i = 0; i < B.GetLength(0); i++)
        {
            for (int j = 0; j < B.GetLength(1); j++)
            {
                Console.Write($"{B[i, j]}\t");
            }
            Console.WriteLine();
        }
        
        foreach (var x in A)
        {
            sum += x;
            product *= x;

            if (x > max) max = x;
            if (x < min) min = x;

            if (x % 2 == 0) sumEvenA += x;
        }

        for (int i = 0; i < B.GetLength(0); i++)
        {
            for (int j = 0; j < B.GetLength(1); j++)
            {
                double val = B[i, j];

                sum += val;
                product *= val;

                if (val > max) max = val;
                if (val < min) min = val;

                if (j % 2 != 0)
                    sumOddColsB += val;
            }
        }

        Console.WriteLine($"\n\nMax: {max}");
        Console.WriteLine($"Min: {min}");
        Console.WriteLine($"Sum: {sum}");
        Console.WriteLine($"Product: {product}");
        Console.WriteLine($"Sum of even in A: {sumEvenA}");
        Console.WriteLine($"Sum of odd cols in B: {sumOddColsB:F2}");
    }
    
    
    
    static void Task2()
    {
        int[,] arr = new int[5, 5];
        Random rand = new Random();

        int min = int.MaxValue, max = int.MinValue;
        int minIndex = 0, maxIndex = 0;
        
        int index = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                arr[i, j] = rand.Next(-100, 101);
                Console.Write($"{arr[i, j]}\t");

                if (arr[i, j] < min)
                {
                    min = arr[i, j];
                    minIndex = index;
                }

                if (arr[i, j] > max)
                {
                    max = arr[i, j];
                    maxIndex = index;
                }

                index++;
            }
            Console.WriteLine();
        }

        int start = Math.Min(minIndex, maxIndex);
        int end = Math.Max(minIndex, maxIndex);

        int sum = 0;
        index = 0;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (index > start && index < end)
                    sum += arr[i, j];

                index++;
            }
        }

        Console.WriteLine($"\nSum between min and max values: {sum}");
    }
    static string CaesarEncrypt(string text, int shift)
    {
        char[] result = new char[text.Length];

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            if (char.IsLetter(c))
            {
                char offset = char.IsUpper(c) ? 'A' : 'a';  // Using A as it is first in ABC
                result[i] = (char)((c - offset + shift) % 26 + offset);  // 26 letters in ABC
            }
            else
            {
                result[i] = c;
            }
        }

        return new string(result);
    }

    static string CaesarDecrypt(string text, int shift)
    {
        return CaesarEncrypt(text, 26 - shift);
    }
    
    
    static void Main(string[] args)
    {
        Console.WriteLine("\nTask 1");
        Task1();

        Console.WriteLine("\nTask 2");
        Task2();

        Console.WriteLine("\nTask 3");
        Console.Write("Enter string (in English): ");
        string input = Console.ReadLine();

        Console.Write("Shifted: ");
        int shift = int.Parse(Console.ReadLine());

        string encrypted = CaesarEncrypt(input, shift);
        Console.WriteLine($"Encrypted: {encrypted}");

        string decrypted = CaesarDecrypt(encrypted, shift);
        Console.WriteLine($"Decrypted: {decrypted}");
    }
}
