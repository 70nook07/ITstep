namespace CSharp
{
    class HW012
    {
        // TASK 1
        public static void Swap<T>(ref T a, ref T b)
        {
            (a, b) = (b, a);
        }

        // TASK 2
        public class MyStack<T>
        {
            private List<T> _items = new();
            public int Count => _items.Count;

            public void Push(T item)
            {
                _items.Add(item);
            }

            public T Pop()
            {
                if (_items.Count == 0) throw new InvalidOperationException("The stack is empty.");
                int lastIndex = _items.Count - 1;
                T item = _items[lastIndex];
                _items.RemoveAt(lastIndex);
                return item;
            }

            public T Peek()
            {
                if (_items.Count == 0) throw new InvalidOperationException("The stack is empty.");
                return _items[_items.Count - 1];
            }
        }

        // TASK 3
        public abstract class MarineCreature(string name, string species)
        {
            public string Name { get; set; } = name;
            public string Species { get; set; } = species;

            public abstract void DisplayInfo();
        }

        // Creatures
        public class Shark(string name, string species) : MarineCreature(name, species)
        {
            public override void DisplayInfo() => Console.WriteLine($"[Shark] Name: {Name}, Species: {Species}");
        }

        public class Dolphin(string name, string species) : MarineCreature(name, species)
        {
            public override void DisplayInfo() => Console.WriteLine($"[Dolphin] Name: {Name}, Species: {Species}");
        }

        public class SeaTurtle(string name, string species) : MarineCreature(name, species)
        {
            public override void DisplayInfo() => Console.WriteLine($"[Sea Turtle] Name: {Name}, Species: {Species}");
        }

        // Oceanarium foreach
        public class Oceanarium : System.Collections.IEnumerable
        {
            private List<MarineCreature> _inhabitants = new List<MarineCreature>();

            public void AddCreature(MarineCreature creature)
            {
                _inhabitants.Add(creature);
            }

            public System.Collections.IEnumerator GetEnumerator()
            {
                return _inhabitants.GetEnumerator();
            }
        }

        // TASK 4
        public class PasswordManager
        {
            private Dictionary<string, string> _userDatabase = new();

            // Create employee
            public void AddEmployee(string login, string password)
            {
                if (_userDatabase.ContainsKey(login))
                {
                    Console.WriteLine($"Error: Employee with login '{login}' already exists.");
                    return;
                }

                _userDatabase.Add(login, password);
                Console.WriteLine($"Employee '{login}' successfully added.");
            }

            // Delete employee
            public void DeleteEmployee(string login)
            {
                if (_userDatabase.Remove(login))
                {
                    Console.WriteLine($"Employee '{login}' successfully removed.");
                }
                else
                {
                    Console.WriteLine($"Error: Employee '{login}' not found.");
                }
            }

            // Update employee
            public void UpdateEmployee(string login, string newPassword)
            {
                if (_userDatabase.ContainsKey(login))
                {
                    _userDatabase[login] = newPassword;
                    Console.WriteLine($"Credentials updated for employee '{login}'.");
                }
                else
                {
                    Console.WriteLine($"Error: Employee '{login}' not found.");
                }
            }

            public string GetPassword(string login)
            {
                return _userDatabase.GetValueOrDefault(login, "[Access Denied: User Not Found]");
            }
        }

        static void Main(string[] args)
        {
            // TASK 1: Generic Swap Method
            Console.WriteLine("\tTask 1: Generic Swap");
            int num1 = 10, num2 = 20;
            Console.WriteLine($"Before Swap: num1 = {num1}, num2 = {num2}");
            Swap(ref num1, ref num2);
            Console.WriteLine($"After Swap:  num1 = {num1}, num2 = {num2}");
            string str1 = "Hello", str2 = "World";
            Console.WriteLine($"Before Swap: str1 = {str1}, str2 = {str2}");
            Swap(ref str1, ref str2);
            Console.WriteLine($"After Swap:  str1 = {str1}, str2 = {str2}\n");

            // TASK 2: Generic Stack
            Console.WriteLine("\tTask 2: Generic Stack");
            MyStack<string> wordStack = new MyStack<string>();
            wordStack.Push("First");
            wordStack.Push("Second");
            wordStack.Push("Third");
            Console.WriteLine($"Count: {wordStack.Count}");
            Console.WriteLine($"Top item: {wordStack.Peek()}");
            Console.WriteLine($"Popped: {wordStack.Pop()}");
            Console.WriteLine($"Count after pop: {wordStack.Count}\n");

            // TASK 3: Oceanarium Iteration
            Console.WriteLine("\tTask 3: Oceanarium Iterator");
            Oceanarium myOceanarium = new Oceanarium();
            myOceanarium.AddCreature(new Shark("Brute", "Great White"));
            myOceanarium.AddCreature(new Dolphin("Flipper", "Bottlenose"));
            myOceanarium.AddCreature(new SeaTurtle("Scute", "Green Sea Turtle"));
            foreach (MarineCreature creature in myOceanarium)
            {
                creature.DisplayInfo();
            }

            Console.WriteLine();

            // TASK 4: Employee Management
            Console.WriteLine("\tTask 4: Employee Management");
            PasswordManager manager = new PasswordManager();

            // 1. Adding users
            manager.AddEmployee("john_doe", "password123");
            manager.AddEmployee("alice_smith", "secure456");

            // 2. Getting password
            Console.WriteLine($"john_doe's password: {manager.GetPassword("john_doe")}");

            // 3. Changing password
            manager.UpdateEmployee("john_doe", "newSecret789");
            Console.WriteLine($"john_doe's updated password: {manager.GetPassword("john_doe")}");

            // 4. Deleting a user
            manager.DeleteEmployee("alice_smith");
            Console.WriteLine($"Trying to to delete alice_smith: {manager.GetPassword("alice_smith")}");
        }
    }
}
