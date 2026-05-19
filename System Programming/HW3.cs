using System.Text;

namespace SystemProcs;

class HW3
{
    static long totalWords, totalLines, totalPunctuation;

    static readonly char[] punctuation =
    [
        '.', ',', ';', ':',
        '!', '?', '"', '-',
        '(', ')', '{', '}',
        '[', ']', '<', '>', '/'
    ];

    static void Main()
    {
        Console.Write("Enter directory path: ");
        string path = Console.ReadLine();

        // prob wrong dir
        if (!Directory.Exists(path))
        {
            Console.WriteLine("Directory not found.");
            return;
        }

        string[] files = Directory.GetFiles(path, "*.txt");

        if (files.Length == 0)
        {
            Console.WriteLine("No text files found.");
            return;
        }

        // threads
        List<Thread> threads = new();

        foreach (string file in files)
        {
            Thread t = new(() => CheckFile(file));
            threads.Add(t);
            t.Start();
        }

        foreach (Thread t in threads)
            t.Join();

        Console.WriteLine($"\nTotal words: {totalWords}");
        Console.WriteLine($"Total lines: {totalLines}");
        Console.WriteLine($"Total punctuation: {totalPunctuation}");
    }

    static void CheckFile(string file)
    {
        try
        {
            string text = File.ReadAllText(file, Encoding.UTF8);

            long lines = text.Split('\n').Length;
            long punct = text.Count(c => punctuation.Contains(c));

            string cleaned = new string(text.Select(c => punctuation.Contains(c) ? ' ' : c).ToArray());
            long words = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            Interlocked.Add(ref totalWords, words);
            Interlocked.Add(ref totalLines, lines);
            Interlocked.Add(ref totalPunctuation, punct);

            Console.WriteLine($"[{Path.GetFileName(file)}] - Words: {words}, Lines: {lines}, Punctuation: {punct}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in {file}: {ex.Message}");
        }
    }
}
