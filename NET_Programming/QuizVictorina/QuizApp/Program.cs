using System.Globalization;
using QuizApp.Core;

namespace QuizApp
{
    internal class Program
    {
        private static readonly EmailService EmailService = new();
        private static readonly UserService UserService = new(EmailService);
        private static readonly QuizService QuizService = new();

        private static void Main()
        {
            Console.WriteLine("\tQUIZ APP");
            User? currentUser = null;

            while (currentUser == null)
            {
                Console.WriteLine("\n1. Log in\n2. Register\n0. Exit");
                Console.Write("Choose an option: ");
                switch (Console.ReadLine())
                {
                    case "1": currentUser = LogIn(); break;
                    case "2": Register(); break;
                    case "0": return;
                    default: Console.WriteLine("Unknown option."); break;
                }
            }

            ShowMainMenu(currentUser);
        }

        private static User? LogIn()
        {
            Console.Write("Login: ");
            string login = Console.ReadLine() ?? "";
            Console.Write("Password: ");
            string password = ReadPassword();

            var user = UserService.TryLogin(login, password);
            if (user == null)
            {
                Console.WriteLine("Wrong login/password, or account not confirmed.");
                return null;
            }

            Console.WriteLine($"Welcome, {user.Login}!");
            return user;
        }

        private static void Register()
        {
            Console.WriteLine("\n\tRegistration");

            string login;
            while (true)
            {
                Console.Write("Choose a login: ");
                login = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(login)) { Console.WriteLine("Login cannot be empty."); continue; }
                if (UserService.LoginExists(login)) { Console.WriteLine("Login already taken."); continue; }
                break;
            }

            string password;
            while (true)
            {
                Console.Write("Choose a password: ");
                password = ReadPassword();
                Console.Write("Repeat password: ");
                if (ReadPassword() != password) { Console.WriteLine("Passwords do not match."); continue; }
                if (password.Length < 4) { Console.WriteLine("Password too short (min 4 chars)."); continue; }
                break;
            }

            DateTime birthDate;
            while (true)
            {
                Console.Write("Birth date (dd.MM.yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out birthDate)) break;
                Console.WriteLine("Wrong format, use dd.MM.yyyy.");
            }

            string email;
            while (true)
            {
                Console.Write("E-mail: ");
                email = Console.ReadLine() ?? "";
                if (email.Contains('@')) break;
                Console.WriteLine("Invalid e-mail.");
            }

            string code = UserService.GenerateConfirmationCode();
            UserService.SendConfirmationCode(email, login, code);

            Console.Write("Enter the confirmation code: ");
            if (Console.ReadLine() != code)
            {
                Console.WriteLine("Wrong code. Registration cancelled.");
                return;
            }

            UserService.Register(new User
            {
                Login = login,
                Password = password,
                BirthDate = birthDate,
                Email = email,
                IsConfirmed = true
            });

            Console.WriteLine("Registration complete, you can log in now.");
        }

        // Shows password as "*" instead of the typed characters
        private static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        private static void ShowMainMenu(User user)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine($"\n\tMain menu ({user.Login})");
                Console.WriteLine("1. Start a new quiz\n2. My results\n3. Top-20 for a quiz\n4. Settings\n0. Log out");
                Console.Write("Choose an option: ");
                switch (Console.ReadLine())
                {
                    case "1": StartQuiz(user); break;
                    case "2": ShowMyResults(user); break;
                    case "3": ShowTop20(); break;
                    case "4": ShowSettings(user); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Unknown option."); break;
                }
            }
        }

        private static void ShowMyResults(User user)
        {
            var results = QuizService.GetResultsForUser(user.Login);
            if (results.Count == 0) { Console.WriteLine("No quizzes completed yet."); return; }

            foreach (var r in results)
                Console.WriteLine($"{r.FinishedAt:dd.MM.yyyy HH:mm} | {r.CategoryName,-15} | {r.CorrectAnswers}/{r.TotalQuestions}");
        }

        private static void ShowTop20()
        {
            var categoryName = ChooseCategory(true);
            if (categoryName == null) return;

            var top = QuizService.GetTop20(categoryName);
            if (top.Count == 0) { Console.WriteLine("No results for this quiz yet."); return; }

            Console.WriteLine($"\n\tTop-20: {categoryName}");
            int place = 1;
            foreach (var r in top)
                Console.WriteLine($"{place++,2}. {r.Login,-15} {r.CorrectAnswers}/{r.TotalQuestions}  ({r.FinishedAt:dd.MM.yyyy})");
        }

        private static void ShowSettings(User user)
        {
            Console.WriteLine("\n1. Change password\n2. Change birth date\n0. Back");
            Console.Write("Choose an option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("New password: ");
                    string newPassword = Console.ReadLine() ?? "";
                    if (newPassword.Length < 4) { Console.WriteLine("Too short, nothing changed."); return; }
                    UserService.ChangePassword(user.Login, newPassword);
                    Console.WriteLine("Password changed.");
                    break;

                case "2":
                    Console.Write("New birth date (dd.MM.yyyy): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime newDate))
                    {
                        UserService.ChangeBirthDate(user.Login, newDate);
                        user.BirthDate = newDate;
                        Console.WriteLine("Birth date changed.");
                    }
                    else Console.WriteLine("Wrong format, nothing changed.");
                    break;
            }
        }

        private static void StartQuiz(User user)
        {
            var categoryName = ChooseCategory(true);
            if (categoryName == null) return;

            var questions = QuizService.BuildQuestionSet(categoryName);
            if (questions.Count == 0) { Console.WriteLine("This quiz has no questions yet."); return; }

            int correctCount = 0;
            for (int q = 0; q < questions.Count; q++)
            {
                var question = questions[q];
                Console.WriteLine($"\nQuestion {q + 1}/{questions.Count}: {question.Text}");
                for (int o = 0; o < question.Options.Count; o++)
                    Console.WriteLine($"  {o + 1}. {question.Options[o]}");

                Console.Write(question.CorrectOptionIndices.Count > 1
                    ? "Enter ALL correct option numbers, comma-separated: "
                    : "Enter the correct option number: ");

                var selected = ParseAnswer(Console.ReadLine() ?? "");
                if (QuizService.IsAnswerCorrect(question, selected))
                {
                    correctCount++;
                    Console.WriteLine("Correct!");
                }
                else Console.WriteLine("Incorrect.");
            }

            var result = new QuizResult
            {
                Login = user.Login,
                CategoryName = categoryName,
                CorrectAnswers = correctCount,
                TotalQuestions = questions.Count
            };
            QuizService.SaveResult(result);
            int place = QuizService.GetPlace(result);

            Console.WriteLine($"\n\tQuiz finished: {correctCount}/{questions.Count} correct");
            Console.WriteLine($"Your place in the '{categoryName}' leaderboard: {place}");
        }

        private static HashSet<int> ParseAnswer(string line)
        {
            var result = new HashSet<int>();
            foreach (var part in line.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                if (int.TryParse(part, out int n) && n > 0) result.Add(n - 1);
            return result;
        }

        // Shows the list of categories
        private static string? ChooseCategory(bool includeMixed)
        {
            var categories = QuizService.GetAllCategories();
            if (categories.Count == 0) { Console.WriteLine("No quizzes available yet."); return null; }

            Console.WriteLine("\nAvailable quizzes:");
            for (int i = 0; i < categories.Count; i++)
                Console.WriteLine($"{i + 1}. {categories[i].Name}");
            if (includeMixed)
                Console.WriteLine($"{categories.Count + 1}. {QuizService.MixedCategoryName}");

            Console.Write("Your choice: ");
            int max = categories.Count + (includeMixed ? 1 : 0);
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > max)
            {
                Console.WriteLine("Wrong choice.");
                return null;
            }

            return index == categories.Count + 1 ? QuizService.MixedCategoryName : categories[index - 1].Name;
        }
    }
}
