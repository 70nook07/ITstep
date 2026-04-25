namespace PW1;

class Program
{
    static int CheckInt(string message, int minValue)
    {
        int value;

        while (true)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();

            if (int.TryParse(input, out value))
            {
                if (value >= minValue)
                    return value;

                Console.WriteLine($"Number must be >= {minValue}");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }

    static void Main(string[] args)
    {

        // TASK 1
        Console.WriteLine("TASK 1");

        int size1 = CheckInt("Enter array size:", 1);
        int[] numbers1 = new int[size1];

        Console.WriteLine("Enter numbers:");

        for (int i = 0; i < size1; i++)
        {
            numbers1[i] = CheckInt($"Element [{i}]:", int.MinValue);
        }

        Console.WriteLine();

        int countLessThan7 = 0;

        foreach (int num in numbers1)
        {
            if (num < 7)
                countLessThan7++;
        }

        Console.WriteLine($"Numbers less than 7: {countLessThan7}");

        // TASK 2
        Random rand = new Random();

        Console.WriteLine("\nTASK 2");

        int size2 = CheckInt("Enter array size:", 1);
        int[] numbers2 = new int[size2];

        Console.WriteLine("Generated array:");

        for (int i = 0; i < size2; i++)
        {
            numbers2[i] = rand.Next(-10, 11);
            Console.Write(numbers2[i] + " ");
        }

        Console.WriteLine();

        int evenCount = 0;
        int oddCount = 0;
        int uniqueCount = 0;

        foreach (int num in numbers2)
        {
            if (num % 2 == 0)
                evenCount++;
            else
                oddCount++;
        }

        for (int i = 0; i < numbers2.Length; i++)
        {
            bool isUnique = true;

            for (int j = 0; j < numbers2.Length; j++)
            {
                if (i != j && numbers2[i] == numbers2[j])
                {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique)
                uniqueCount++;
        }

        Console.WriteLine($"Even numbers: {evenCount}");
        Console.WriteLine($"Odd numbers: {oddCount}");
        Console.WriteLine($"Unique numbers: {uniqueCount}");
    }
}
