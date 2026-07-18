using QuizApp.Core;

namespace QuizEditor
{
    internal class Program
    {
        private static readonly AdminService AdminService = new();
        private static readonly QuizManagementService ManagementService = new();

        private static void Main()
        {
            Console.WriteLine("\tQUIZ EDITOR (admin)");
            Admin? admin = null;

            while (admin == null)
            {
                Console.WriteLine("\n1. Log in\n2. Register admin account\n0. Exit");
                Console.Write("Choose an option: ");
                switch (Console.ReadLine())
                {
                    case "1": admin = LogIn(); break;
                    case "2": RegisterAdmin(); break;
                    case "0": return;
                    default: Console.WriteLine("Unknown option."); break;
                }
            }

            Console.WriteLine($"Welcome, {admin.Login}!");
            ShowEditorMenu();
        }

        private static Admin? LogIn()
        {
            Console.Write("Login: ");
            string login = Console.ReadLine() ?? "";
            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            var admin = AdminService.TryLogin(login, password);
            if (admin == null) Console.WriteLine("Wrong login or password.");
            return admin;
        }

        private static void RegisterAdmin()
        {
            Console.Write("Choose a login: ");
            string login = Console.ReadLine() ?? "";
            if (AdminService.LoginExists(login)) { Console.WriteLine("Login already exists."); return; }

            Console.Write("Choose a password: ");
            string password = Console.ReadLine() ?? "";
            if (password.Length < 4) { Console.WriteLine("Password too short."); return; }

            AdminService.Register(login, password);
            Console.WriteLine("Admin account created, you can log in now.");
        }

        private static void ShowEditorMenu()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n\tEditor menu");
                Console.WriteLine("1. List quiz sections\n2. Create new section\n3. Add question\n4. Edit question\n5. Delete question\n0. Exit");
                Console.Write("Choose an option: ");
                switch (Console.ReadLine())
                {
                    case "1": ListCategories(); break;
                    case "2": CreateCategory(); break;
                    case "3": AddQuestion(); break;
                    case "4": EditQuestion(); break;
                    case "5": DeleteQuestion(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Unknown option."); break;
                }
            }
        }

        private static void ListCategories()
        {
            var categories = ManagementService.GetAllCategories();
            if (categories.Count == 0) { Console.WriteLine("No quiz sections yet."); return; }
            foreach (var c in categories)
                Console.WriteLine($"- {c.Name} ({c.Questions.Count} question(s))");
        }

        private static void CreateCategory()
        {
            Console.Write("New section name: ");
            string name = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Name cannot be empty."); return; }
            ManagementService.AddCategory(name);
            Console.WriteLine("Section created.");
        }

        private static QuizCategory? ChooseCategory()
        {
            var categories = ManagementService.GetAllCategories();
            if (categories.Count == 0) { Console.WriteLine("No quiz sections yet."); return null; }

            for (int i = 0; i < categories.Count; i++)
                Console.WriteLine($"{i + 1}. {categories[i].Name}");

            Console.Write("Choose a section: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > categories.Count)
            {
                Console.WriteLine("Wrong choice.");
                return null;
            }
            return categories[index - 1];
        }

        private static void AddQuestion()
        {
            var category = ChooseCategory();
            if (category == null) return;

            Console.Write("Question text: ");
            string text = Console.ReadLine() ?? "";

            var options = ReadOptions();
            if (options.Count < 2) { Console.WriteLine("A question needs at least 2 options."); return; }

            var correct = ReadCorrectIndices(options.Count);
            if (correct.Count == 0) { Console.WriteLine("At least one correct option is required."); return; }

            ManagementService.AddQuestion(category.Name, new Question { Text = text, Options = options, CorrectOptionIndices = correct });
            Console.WriteLine("Question added.");
        }

        private static void EditQuestion()
        {
            var category = ChooseCategory();
            if (category == null || category.Questions.Count == 0) { Console.WriteLine("No questions here."); return; }

            foreach (var q in category.Questions) Console.WriteLine($"[{q.Id}] {q.Text}");
            Console.Write("Question id to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Wrong id."); return; }

            var existing = category.Questions.FirstOrDefault(q => q.Id == id);
            if (existing == null) { Console.WriteLine("Question not found."); return; }

            Console.Write($"New text (empty = keep current): ");
            string text = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(text)) text = existing.Text;

            var options = ReadOptions();
            if (options.Count < 2) options = existing.Options;

            var correct = ReadCorrectIndices(options.Count);
            if (correct.Count == 0) correct = existing.CorrectOptionIndices;

            ManagementService.UpdateQuestion(category.Name, new Question
            {
                Id = existing.Id, Text = text, Options = options, CorrectOptionIndices = correct
            });
            Console.WriteLine("Question updated.");
        }

        private static void DeleteQuestion()
        {
            var category = ChooseCategory();
            if (category == null || category.Questions.Count == 0) { Console.WriteLine("No questions here."); return; }

            foreach (var q in category.Questions) Console.WriteLine($"[{q.Id}] {q.Text}");
            Console.Write("Question id to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Wrong id."); return; }

            ManagementService.DeleteQuestion(category.Name, id);
            Console.WriteLine("Question deleted (if it existed).");
        }

        private static List<string> ReadOptions()
        {
            var options = new List<string>();
            Console.WriteLine("Enter options one by one, empty line to stop:");
            while (true)
            {
                Console.Write($"  Option {options.Count + 1}: ");
                string option = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(option)) break;
                options.Add(option);
            }
            return options;
        }

        private static List<int> ReadCorrectIndices(int optionCount)
        {
            Console.Write("Correct option numbers, comma-separated: ");
            string line = Console.ReadLine() ?? "";
            return line.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s, out int n) ? n - 1 : -1)
                .Where(n => n >= 0 && n < optionCount)
                .Distinct()
                .ToList();
        }
    }
}
