using System.Text;

namespace CSharp;

public class HW4
{
    static void Main(string[] args)
    {
        // Task 1
        string og = "Hello world!";
        string toInsert = "beautiful ";
        int pos = 6;
        string result1 = InsertString(og, toInsert, pos);
        Console.WriteLine($"Task 1: \"{og}\" + \"{toInsert}\" at position {pos} = \"{result1}\"");

        // Task 2
        string[] testPalindromes = { "racecar", "Do nine men Interpret? Nine men I nod", "hello", "hair", "madam" };
        foreach (var s in testPalindromes)
        {
            bool isPal = IsPalindrome(s);
            Console.WriteLine($"Task 2: \"{s}\" is palindrome? {isPal}");
        }

        // Task 3
        string text = "Hello World! 123456 LOL";
        LetterPercentage(text, out double lowerPcent, out double upperPcent);
        Console.WriteLine($"Task 3: \"{text}\" -> lowercase: {lowerPcent:F2}%, uppercase: {upperPcent:F2}%");

        // Task 4
        string[] words = { "apple", "tree", "sun", "moonlight", "hi" };
        int targetLen = 5;
        string[] modifiedWords = ReplaceLastThreeWithBux(words, targetLen);
        Console.WriteLine($"Task 4: words with length {targetLen} modified: [{string.Join(", ", modifiedWords)}]");

        // Task 5
        string text5 = "A quick red car";
        int wordNumber = 3; // 1-based
        char? firstChar = GetFirstCharOfNthWord(text5, wordNumber);
        Console.WriteLine($"Task 5: in \"{text5}\" word #{wordNumber} first char is '{firstChar}'");

        // Task 6
        string messy = "   Hello    world   from  C#   ";
        string cleaned = NormalizeAndSeparateByStar(messy);
        Console.WriteLine($"Task 6: \"{messy}\" -> \"{cleaned}\"");

        // Task 7:
        Console.WriteLine("\nTask 7: Enter words one by one. Finish with a word ending with a dot");
        string joinedWords = BuildStringFromInput();
        Console.WriteLine($"Result: {joinedWords}");
    }

    // Task 1 func

    static string InsertString(string original, string toInsert, int position)
    {
        if (original == null) return null;
        if (toInsert == null) return original;
        if (position < 0 || position > original.Length) return original;
        return original.Insert(position, toInsert);
    }

    // Task 2 func
    static bool IsPalindrome(string input)
    {
        if (string.IsNullOrEmpty(input)) return true;
        int left = 0;
        int right = input.Length - 1;
        while (left < right)
        {
            while (left < right && !char.IsLetterOrDigit(input[left])) left++;
            while (left < right && !char.IsLetterOrDigit(input[right])) right--;
            if (left < right)
            {
                if (char.ToLowerInvariant(input[left]) != char.ToLowerInvariant(input[right])) return false;
                left++;
                right--;
            }
        }

        return true;
    }

    // Task 3 func
    static void LetterPercentage(string text, out double lowerPercent, out double upperPercent)
    {
        lowerPercent = 0;
        upperPercent = 0;
        if (string.IsNullOrEmpty(text)) return;
        int total = text.Length;
        int lowerCount = 0;
        int upperCount = 0;
        foreach (char c in text)
        {
            if (char.IsLower(c))
                lowerCount++;
            else if (char.IsUpper(c)) upperCount++;
        }

        lowerPercent = (double)lowerCount / total * 100.0;
        upperPercent = (double)upperCount / total * 100.0;
    }

    // Task 4 func
    static string[] ReplaceLastThreeWithBux(string[] words, int targetLength)
    {
        if (words == null) return Array.Empty<string>();
        string[] result = new string[words.Length];
        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];
            if (word != null && word.Length == targetLength && word.Length >= 3)
            {
                result[i] = word.Substring(0, word.Length - 3) + "$$$";
            }
            else
            {
                result[i] = word;
            }
        }

        return result;
    }

    // Task 5:  func
    static char? GetFirstCharOfNthWord(string text, int n)
    {
        if (string.IsNullOrWhiteSpace(text) || n <= 0) return null;
        string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (n <= words.Length)
        {
            string word = words[n - 1];
            if (word.Length > 0) return word[0];
        }

        return null;
    }
    
    // Task 6 func
    static string NormalizeAndSeparateByStar(string input)
    {
        if (input == null) return string.Empty;
        string[] parts = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join("*", parts);
    }

    // Task 7 func
    static string BuildStringFromInput()
    {
        List<string> words = new List<string>();
        string line;
        while (true)
        {
            line = Console.ReadLine();
            if (line == null)   //EOF
                break;
            string word = line.Trim();
            if (word.Length == 0) continue;
            words.Add(word);
            if (word.EndsWith(".")) break;
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < words.Count; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append(words[i]);
        }

        return sb.ToString();
    }
}
