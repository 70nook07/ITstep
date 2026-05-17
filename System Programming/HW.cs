using System;
using System.Threading;

class HW2
{
    static int[] numbers = new int[10000];
    static int max = int.MinValue;
    static int min = int.MaxValue;
    static long sum = 0;
    static readonly object lockObj;

    static void Main()
    {
        while (true)
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("1. print 0 to 50");
            Console.WriteLine("2. print user-defined range");
            Console.WriteLine("3. multiple threads, user-defined range");
            Console.WriteLine("4. max, min, average (10k numbers)");
            Console.WriteLine("5. task 4 + print numbers thread");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1": Task1(); break;
                case "2": Task2(); break;
                case "3": Task3(); break;
                case "4": Task4(); break;
                case "5": Task5(); break;
                case "0": return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    // Task 1
    static void Task1()
    {
        Console.WriteLine("Task 1: printing 0 to 50");
        Thread thread = new Thread(() => PrintRange(0, 50));
        thread.Start();
        thread.Join();
    }

    // Task 2
    static void Task2()
    {
        Console.Write("Enter start of range: ");
        int start = int.Parse(Console.ReadLine());
        Console.Write("Enter end of range: ");
        int end = int.Parse(Console.ReadLine());

        Console.WriteLine($"Task 2: printing {start} to {end}");
        Thread thread = new Thread(() => PrintRange(start, end));
        thread.Start();
        thread.Join();
    }

    // Task 3:
    static void Task3()
    {
        Console.Write("Enter start of range: ");
        int start = int.Parse(Console.ReadLine());
        Console.Write("Enter end of range: ");
        int end = int.Parse(Console.ReadLine());
        Console.Write("Enter how many threads to use: ");
        int threadCount = int.Parse(Console.ReadLine());

        Console.WriteLine($"Task 3: {threadCount} threads printing {start} to {end}");
        Thread[] threads = new Thread[threadCount];

        for (int i = 0; i < threadCount; i++)
        {
            int id = i + 1;
            threads[i] = new Thread(() =>
            {
                Console.WriteLine($"[{id} output]");
                PrintRange(start, end);
            });
            threads[i].Start();
        }
        
        foreach (Thread t in threads) t.Join();
    }
    
    static void PrintRange(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            Console.Write(i + " ");
        }
        Console.WriteLine();
    }

    // Task 4
    static void Task4()
    {
        GenerateNumbers();
        Console.WriteLine("Task 4: get max, min, average with threads...");

        // reset
        max = int.MinValue;
        min = int.MaxValue;
        sum = 0;
        
        Thread maxThread = new Thread(FindMax);
        Thread minThread = new Thread(FindMin);
        Thread avgThread = new Thread(FindSum);

        maxThread.Start();
        minThread.Start();
        avgThread.Start();

        maxThread.Join();
        minThread.Join();
        avgThread.Join();

        double average = (double)sum / numbers.Length;
        Console.WriteLine($"Max: {max}");
        Console.WriteLine($"Min: {min}");
        Console.WriteLine($"Average: {average:F3}");
    }

    // Task 5
    static void Task5()
    {
        GenerateNumbers();
        Console.WriteLine("Task 5: Task 4 + printing thread...");

        // reset
        max = int.MinValue;
        min = int.MaxValue;
        sum = 0;

        Thread maxThread = new Thread(FindMax);
        Thread minThread = new Thread(FindMin);
        Thread sumThread = new Thread(FindSum);
        Thread printThread = new Thread(PrintNumbers);

        maxThread.Start();
        minThread.Start();
        sumThread.Start();
        printThread.Start();

        maxThread.Join();
        minThread.Join();
        sumThread.Join();
        printThread.Join();

        double average = (double)sum / numbers.Length;
        Console.WriteLine($"\nMax: {max}");
        Console.WriteLine($"Min: {min}");
        Console.WriteLine($"Average: {average:F3}");
    }

    static void GenerateNumbers()
    {
        Random rand = new Random();
        for (int i = 0; i < numbers.Length; i++)
            numbers[i] = rand.Next(0, 1001);
        Console.WriteLine("Generated 10,000 random numbers.");
    }

    static void FindMax()
    {
        int localMax = int.MinValue;
        foreach (int n in numbers)
            if (n > localMax) localMax = n;
        max = localMax;
    }

    static void FindMin()
    {
        int localMin = int.MaxValue;
        foreach (int n in numbers)
            if (n < localMin) localMin = n;
        min = localMin;
    }

    static void FindSum()
    {
        long localSum = 0;
        foreach (int n in numbers)
            localSum += n;
        Interlocked.Exchange(ref sum, localSum);
    }

    static void PrintNumbers()
    {
        Console.WriteLine("Printing all numbers (thread):");
        foreach (int n in numbers)
            Console.Write(n + " ");
        Console.WriteLine();
    }
}
