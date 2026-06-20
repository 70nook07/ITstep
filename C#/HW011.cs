namespace CSharp
{
    
    // Task 1: A delegate for numbers
    public delegate bool NumberPredicate(int number);

    // Task 4: A delegate for string
    public delegate int StringProcessor(string text);

    class Hw011
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TASK 1: ARRAY FILTERING WITH DELEGATES");
            Task1();
            Console.WriteLine("\n--------------------------------------------------\n");

            Console.WriteLine("TASK 2: LIB DELEGATES Action, Predicate, Func");
            Task2();
            Console.WriteLine("\n--------------------------------------------------\n");

            Console.WriteLine("ASK 3: CREDIT CARD");
            Task3();
            Console.WriteLine("\n--------------------------------------------------\n");

            Console.WriteLine("TASK 4: STRING PROCESSING DELEGATES");
            Task4();
            
            Console.ReadLine();
        }
        
        // TASK 1
        static int[] FilterArray(int[] array, NumberPredicate predicate)
        {
            int count = 0;
            foreach (int num in array)
            {
                if (predicate(num))
                {
                    count++;
                }
            }
            
            int[] result = new int[count];
            int index = 0;
            foreach (int num in array)
            {
                if (predicate(num))
                {
                    result[index] = num;
                    index++;
                }
            }
            return result;
        }

        static bool IsEven(int n) => n % 2 == 0;
        static bool IsOdd(int n) => n % 2 != 0;
        
        static bool IsPrime(int n)
        {
            if (n <= 1) return false;
            for (int i = 2; i * i <= n; i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        static bool IsFibonacci(int n)
        {
            if (n < 0) return false;
            return IsSquare(5L * n * n + 4) || IsSquare(5L * n * n - 4);
        }

        static bool IsSquare(long x)
        {
            long s = (long)Math.Sqrt(x);
            return (s * s == x);
        }

        static string ArrayToString(int[] array)
        {
            if (array.Length == 0) return "None";
            string result = "";
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i] + (i < array.Length - 1 ? ", " : "");
            }
            return result;
        }
        static void Task1()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 8, 13, 14, 17, 21, 24, 34, 37 };
            Console.WriteLine("Original Array: " + ArrayToString(numbers));

            NumberPredicate evenPredicate = IsEven;
            NumberPredicate oddPredicate = IsOdd;
            NumberPredicate primePredicate = IsPrime;
            NumberPredicate fibonacciPredicate = IsFibonacci;

            int[] evens = FilterArray(numbers, evenPredicate);
            int[] odds = FilterArray(numbers, oddPredicate);
            int[] primes = FilterArray(numbers, primePredicate);
            int[] fibs = FilterArray(numbers, fibonacciPredicate);

            Console.WriteLine("Even numbers: " + ArrayToString(evens));
            Console.WriteLine("Odd numbers:  " + ArrayToString(odds));
            Console.WriteLine("Prime numbers: " + ArrayToString(primes));
            Console.WriteLine("Fibonacci numbers: " + ArrayToString(fibs));
        }

        
        
        // TASK 2
        static void Task2()
        {
            Action displayTime = () =>
                Console.WriteLine($"Current Time: {DateTime.Now.ToString("HH:mm:ss")}");
            Action displayDate = () =>
                Console.WriteLine($"Current Date: {DateTime.Now.ToString("yyyy-MM-dd")}");
            Action displayDayOfWeek = () =>
                Console.WriteLine($"Current Day: {DateTime.Now.DayOfWeek}");
            
            Predicate<double> isValidDimension = val =>
                val > 0;

            Func<double, double, double> triangleArea = (baseLen, height) =>
                0.5 * baseLen * height;
            Func<double, double, double> rectangleArea = (width, height) =>
                width * height;
            
            displayTime();
            displayDate();
            displayDayOfWeek();

            double tBase = 5.0;
            double tHeight = 8.0;
            double rWidth = 4.0;
            double rHeight = 10.0;

            if (isValidDimension(tBase) && isValidDimension(tHeight))
            {
                Console.WriteLine($"Triangle Area (base:{tBase}, height:{tHeight}): {triangleArea(tBase, tHeight)}");
            }

            if (isValidDimension(rWidth) && isValidDimension(rHeight))
            {
                Console.WriteLine($"Rectangle Area (width:{rWidth}, height:{rHeight}): {rectangleArea(rWidth, rHeight)}");
            }
        }
        
        // TASK 3
        public class CreditCard
        {
            public string CardNumber { get; set; }
            public string OwnerName { get; set; }
            public string ExpiryDate { get; set; }
            private string Pin { get; set; }
            public double CreditLimit { get; set; }
            public double Money { get; set; }

            public event Action<double> OnDeposit;
            public event Action<double> OnExpense;
            public event Action OnStartUsingCredit;
            public event Action<double> OnTargetAmountReached;
            public event Action<string> OnPinChanged;

            public CreditCard(string cardNumber, string ownerName, string expiryDate, string pin, double creditLimit, double initialMoney)
            {
                CardNumber = cardNumber;
                OwnerName = ownerName;
                ExpiryDate = expiryDate;
                Pin = pin;
                CreditLimit = creditLimit;
                Money = initialMoney;
            }

            public void Deposit(double amount)
            {
                if (amount > 0)
                {
                    Money += amount;
                    OnDeposit?.Invoke(amount);
                }
            }

            public void Expense(double amount)
            {
                if (amount <= 0) return;

                if (Money + CreditLimit >= amount)
                {
                    bool wasInCreditRangeBefore = Money < 0;
                    Money -= amount;
                    
                    OnExpense.Invoke(amount);

                    if (!wasInCreditRangeBefore && Money < 0)
                    {
                        OnStartUsingCredit?.Invoke();
                    }
                }
                else
                {
                    Console.WriteLine("[ERROR] Transaction declined: Insufficient funds / Credit limit exceeded.");
                }
            }

            public void CheckTargetAmount(double target)
            {
                if (Money >= target)
                {
                    OnTargetAmountReached?.Invoke(target);
                }
            }

            public void ChangePin(string oldPin, string newPin)
            {
                if (Pin == oldPin)
                {
                    Pin = newPin;
                    OnPinChanged?.Invoke(newPin);
                }
                else
                {
                    Console.WriteLine("[ERROR] Incorrect old PIN. Authorization failed.");
                }
            }
        }
        static void Task3()
        {
            CreditCard card = new CreditCard("1234-5678-9876-5432", "John Doe", "12/29", "1111", 5000.0, 1000.0);

            card.OnDeposit += (amount) =>
                Console.WriteLine($"[EVENT] Deposited: {amount:C}. New Balance: {card.Money:C}");
            card.OnExpense += (amount) =>
                Console.WriteLine($"[EVENT] Spent: {amount:C}. Remaining Balance: {card.Money:C}");
            card.OnStartUsingCredit += () =>
                Console.WriteLine("[WARNING] You have run out of your own funds and started using credit money!");
            card.OnTargetAmountReached += (target) =>
                Console.WriteLine($"[YAY] Target achieved! Your money has reached or exceeded {target:C}");
            card.OnPinChanged += (newPin) =>
                Console.WriteLine($"[ALERT] PIN successfully altered. New setup complete.");

            card.Deposit(500.0);
            
            card.CheckTargetAmount(1400.0);

            card.Expense(1200.0);
            card.Expense(400.0);
            
            card.ChangePin("1111", "5555");
        }

        // TASK 4
        static int CountVowels(string text)
        {
            int count = 0;
            string vowels = "aeiouAEIOU";
            
            for (int i = 0; i < text.Length; i++)
            {
                if (vowels.IndexOf(text[i]) != -1)
                {
                    count++;
                }
            }
            return count;
        }

        static int CountConsonants(string text)
        {
            int count = 0;
            string vowels = "aeiouAEIOU";

            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')) && vowels.IndexOf(ch) == -1)
                {
                    count++;
                }
            }
            return count;
        }

        static int GetStringLength(string text)
        {
            return text.Length;
        }
        static void Task4()
        {
            string sampleText = "LOWER TEXT";
            Console.WriteLine($"Target String: \"{sampleText}\"\n");

            StringProcessor vowelCounter = CountVowels;
            StringProcessor consonantCounter = CountConsonants;
            StringProcessor lengthCounter = GetStringLength;

            int vowelsCount = vowelCounter(sampleText);
            int consonantsCount = consonantCounter(sampleText);
            int lengthCount = lengthCounter(sampleText);

            Console.WriteLine($"Vowels count:     {vowelsCount}");
            Console.WriteLine($"Consonants count: {consonantsCount}");
            Console.WriteLine($"Total character length:   {lengthCount}");
        }
    }
}
