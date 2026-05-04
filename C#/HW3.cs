namespace CSharp;

using System;

public class HW3
{
    static void Main(string[] args)
    {
        // Task 3 Array Stats Processor
        Console.WriteLine("~~ Task 3 ~~");

        // Array A
        double[] A = new double[5];
        Console.WriteLine("Enter 5 fractional numbers for array A:");

        for (int i = 0; i < A.Length; i++)
        {
            Console.Write($"A[{i}] = ");
            A[i] = double.Parse(Console.ReadLine()!);
        }

        // 2D Araray B
        double[,] B = new double[3, 4];
        Random rnd = new Random();
        for (int i = 0; i < B.GetLength(0); i++)
        {
            for (int j = 0; j < B.GetLength(1); j++)
            {
                B[i, j] = rnd.NextDouble() * 100.0;
            }
        }

        // Display
        Console.WriteLine("\nArray A:");
        for (int i = 0; i < A.Length; i++)
        {
            Console.Write($"{A[i]:F2} ");
        }
        Console.WriteLine();

        // Display
        Console.WriteLine("\nArray B:");
        for (int i = 0; i < B.GetLength(0); i++)
        {
            for (int j = 0; j < B.GetLength(1); j++)
            {
                Console.Write($"{B[i, j],8:F2} ");
            }
            Console.WriteLine();
        }

        // Stats
        double overallMax = A[0];
        double overallMin = A[0];
        double overallSum = 0.0;
        double overallProd = 1.0;

        foreach (double val in A)
        {
            if (val > overallMax) overallMax = val;
            if (val < overallMin) overallMin = val;
            overallSum += val;
            overallProd *= val;
        }

        foreach (double val in B)
        {
            if (val > overallMax) overallMax = val;
            if (val < overallMin) overallMin = val;
            overallSum += val;
            overallProd *= val;
        }

        double sumEvenA = 0.0;
        foreach (double val in A)
        {
            if (val % 2 == 0)
                sumEvenA += val;
        }

        double sumOddColumnsB = 0.0;
        for (int i = 0; i < B.GetLength(0); i++)
        {
            for (int j = 0; j < B.GetLength(1); j++)
            {
                if (j % 2 == 1)
                    sumOddColumnsB += B[i, j];
            }
        }

        Console.WriteLine($"\nOverall maximum (A & B): {overallMax:F2}");
        Console.WriteLine($"Overall minimum (A & B): {overallMin:F2}");
        Console.WriteLine($"Total sum (A & B): {overallSum:F2}");
        Console.WriteLine($"Total product (A & B): {overallProd:F2}");
        Console.WriteLine($"Sum of even elements in A: {sumEvenA:F2}");
        Console.WriteLine($"Sum of elements in odd columns of B: {sumOddColumnsB:F2}");

        // Task 4 Between Min n Max Sum
        Console.WriteLine("\n~~ Task 4 ~~");

        // random 5x5 array
        int[,] matrix = new int[5, 5];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                matrix[i, j] = rnd.Next(-100, 101);
            }
        }

        // matrix dispay
        Console.WriteLine("5x5 Matrix:");
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Console.Write($"{matrix[i, j],5} ");
            }
            Console.WriteLine();
        }

        int minVal = matrix[0, 0];
        int maxVal = matrix[0, 0];
        int minIndex = 0;
        int maxIndex = 0;

        int idx = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int val = matrix[i, j];
                if (val < minVal)
                {
                    minVal = val;
                    minIndex = idx;
                }
                if (val > maxVal)
                {
                    maxVal = val;
                    maxIndex = idx;
                }
                idx++;
            }
        }

        // range
        int start = Math.Min(minIndex, maxIndex);
        int end = Math.Max(minIndex, maxIndex);

        int sumBetween = 0;
        idx = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (idx > start && idx < end)
                    sumBetween += matrix[i, j];
                idx++;
            }
        }

        Console.WriteLine($"\nMin value: {minVal} at flattened index {minIndex}");
        Console.WriteLine($"Max value: {maxVal} at flattened index {maxIndex}");
        Console.WriteLine($"Sum of elements between them (exclusive): {sumBetween}");

        // Task 5 Minus Five Counter
        Console.WriteLine("\n~~ Task 5 ~~");

        // Using same array from task 4
        int countDiff5 = 0;
        foreach (int val in matrix)
        {
            if (Math.Abs(val - minVal) == 5)
                countDiff5++;
        }

        Console.WriteLine($"Minimum element in the matrix: {minVal}");
        Console.WriteLine($"Number of elements differing from min by 5: {countDiff5}");

        Console.WriteLine("\nPress any key to terminate process...");
        Console.ReadKey();
    }
}
