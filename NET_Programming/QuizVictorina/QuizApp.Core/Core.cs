using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace QuizApp.Core
{
    public class User
    {
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = "";
        public bool IsConfirmed { get; set; }
    }

    public class Admin
    {
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public List<string> Options { get; set; } = new();
        public List<int> CorrectOptionIndices { get; set; } = new();
    }

    public class QuizCategory
    {
        public string Name { get; set; } = "";
        public List<Question> Questions { get; set; } = new();
    }

    public class QuizResult
    {
        public string Login { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; } = 20;
        public DateTime FinishedAt { get; set; } = DateTime.Now;
    }

    // Confirmation code.
    // Set your own SMTP details below. If sending fails, the code is printed to the console instead.
    public class EmailService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _sender;
        private readonly string _password;
        private readonly bool _ssl;

        public EmailService(string host = "smtp.gmail.com", int port = 587, string sender = "your.email@gmail.com",
            string password = "your-password", bool ssl = true)
        {
            _host = host;
            _port = port;
            _sender = sender;
            _password = password;
            _ssl = ssl;
        }

        public void SendConfirmationCode(string toEmail, string login, string code)
        {
            try
            {
                using var client = new SmtpClient(_host, _port)
                {
                    Credentials = new NetworkCredential(_sender, _password), EnableSsl = _ssl
                };
                var message = new MailMessage(_sender, toEmail)
                {
                    Subject = "Quiz App - registration confirmation",
                    Body = $"Hello, {login}!\n\nYour confirmation code is: {code}"
                };
                client.Send(message);
                Console.WriteLine($"Confirmation code was sent to {toEmail}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not send e-mail: " + ex.Message);
                Console.WriteLine($"Demo confirmation code: {code}");
            }
        }
    }

    // Reads/writes the JSON files.
    public static class DataStore
    {
        private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

        public static string GetDataFolder()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Data"))) dir = dir.Parent;
            string folder = dir != null
                ? Path.Combine(dir.FullName, "Data")
                : Path.Combine(AppContext.BaseDirectory, "Data");
            Directory.CreateDirectory(folder);
            return folder;
        }

        public static List<T> Load<T>(string fileName)
        {
            string path = Path.Combine(GetDataFolder(), fileName);
            if (!File.Exists(path)) return new List<T>();
            string json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return new List<T>();
            return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
        }

        public static void Save<T>(string fileName, List<T> items)
        {
            string path = Path.Combine(GetDataFolder(), fileName);
            File.WriteAllText(path, JsonSerializer.Serialize(items, Options));
        }
    }

    public class UserService
    {
        private const string File = "users.json";
        private readonly EmailService _email;
        public UserService(EmailService email) => _email = email;

        public bool LoginExists(string login) =>
            DataStore.Load<User>(File).Any(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));

        public void Register(User user)
        {
            var users = DataStore.Load<User>(File);
            users.Add(user);
            DataStore.Save(File, users);
        }

        public string GenerateConfirmationCode() => new Random().Next(100000, 999999).ToString();

        public void SendConfirmationCode(string email, string login, string code) =>
            _email.SendConfirmationCode(email, login, code);

        public User? FindByLogin(string login) =>
            DataStore.Load<User>(File).FirstOrDefault(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));

        public User? TryLogin(string login, string password)
        {
            var user = FindByLogin(login);
            if (user == null || !user.IsConfirmed) return null;
            return user.Password == password ? user : null;
        }

        public void ChangePassword(string login, string newPassword)
        {
            var users = DataStore.Load<User>(File);
            var user = users.FirstOrDefault(u => u.Login == login);
            if (user == null) return;
            user.Password = newPassword;
            DataStore.Save(File, users);
        }

        public void ChangeBirthDate(string login, DateTime newBirthDate)
        {
            var users = DataStore.Load<User>(File);
            var user = users.FirstOrDefault(u => u.Login == login);
            if (user == null) return;
            user.BirthDate = newBirthDate;
            DataStore.Save(File, users);
        }
    }

    public class QuizService
    {
        private const string QuizzesFile = "quizzes.json";
        private const string ResultsFile = "results.json";
        private const int QuestionsPerQuiz = 20;
        public const string MixedCategoryName = "Mixed";
        public List<QuizCategory> GetAllCategories() => DataStore.Load<QuizCategory>(QuizzesFile);

        public List<Question> BuildQuestionSet(string categoryName)
        {
            var categories = GetAllCategories();
            var pool = categoryName == MixedCategoryName
                ? categories.SelectMany(c => c.Questions).ToList()
                : categories.FirstOrDefault(c => c.Name == categoryName)?.Questions ?? new List<Question>();
            var rnd = new Random();
            return pool.OrderBy(_ => rnd.Next()).Take(QuestionsPerQuiz).ToList();
        }

        public bool IsAnswerCorrect(Question question, HashSet<int> selected) =>
            new HashSet<int>(question.CorrectOptionIndices).SetEquals(selected);

        public void SaveResult(QuizResult result)
        {
            var results = DataStore.Load<QuizResult>(ResultsFile);
            results.Add(result);
            DataStore.Save(ResultsFile, results);
        }

        public List<QuizResult> GetResultsForUser(string login) =>
            DataStore.Load<QuizResult>(ResultsFile)
                .Where(r => r.Login.Equals(login, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(r => r.FinishedAt).ToList();

        public List<QuizResult> GetTop20(string categoryName) =>
            DataStore.Load<QuizResult>(ResultsFile).Where(r => r.CategoryName == categoryName)
                .OrderByDescending(r => r.CorrectAnswers).ThenBy(r => r.FinishedAt).Take(20).ToList();

        public int GetPlace(QuizResult result)
        {
            var ordered = DataStore.Load<QuizResult>(ResultsFile).Where(r => r.CategoryName == result.CategoryName)
                .OrderByDescending(r => r.CorrectAnswers).ThenBy(r => r.FinishedAt).ToList();
            return ordered.FindIndex(r =>
                r.Login == result.Login && r.FinishedAt == result.FinishedAt &&
                r.CorrectAnswers == result.CorrectAnswers) + 1;
        }
    }

    public class AdminService
    {
        private const string File = "admins.json";

        public Admin? TryLogin(string login, string password)
        {
            var admin = DataStore.Load<Admin>(File).FirstOrDefault(a => a.Login == login);
            return admin != null && admin.Password == password ? admin : null;
        }

        public bool LoginExists(string login) => DataStore.Load<Admin>(File).Any(a => a.Login == login);

        public void Register(string login, string password)
        {
            var admins = DataStore.Load<Admin>(File);
            admins.Add(new Admin { Login = login, Password = password });
            DataStore.Save(File, admins);
        }
    }

    public class QuizManagementService
    {
        private const string File = "quizzes.json";
        public List<QuizCategory> GetAllCategories() => DataStore.Load<QuizCategory>(File);
        public void SaveAll(List<QuizCategory> categories) => DataStore.Save(File, categories);

        public void AddCategory(string name)
        {
            var categories = GetAllCategories();
            if (categories.Any(c => c.Name == name)) return;
            categories.Add(new QuizCategory { Name = name });
            SaveAll(categories);
        }

        public void AddQuestion(string categoryName, Question question)
        {
            var categories = GetAllCategories();
            var category = categories.FirstOrDefault(c => c.Name == categoryName);
            if (category == null) return;
            question.Id = category.Questions.Count == 0 ? 1 : category.Questions.Max(q => q.Id) + 1;
            category.Questions.Add(question);
            SaveAll(categories);
        }

        public void UpdateQuestion(string categoryName, Question updated)
        {
            var categories = GetAllCategories();
            var existing = categories.FirstOrDefault(c => c.Name == categoryName)?.Questions
                .FirstOrDefault(q => q.Id == updated.Id);
            if (existing == null) return;
            existing.Text = updated.Text;
            existing.Options = updated.Options;
            existing.CorrectOptionIndices = updated.CorrectOptionIndices;
            SaveAll(categories);
        }

        public void DeleteQuestion(string categoryName, int questionId)
        {
            var categories = GetAllCategories();
            categories.FirstOrDefault(c => c.Name == categoryName)?.Questions.RemoveAll(q => q.Id == questionId);
            SaveAll(categories);
        }
    }
}