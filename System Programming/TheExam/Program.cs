using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;

namespace TheExam;

internal sealed class Program
{
    private static Mutex? _singleInstanceMutex;

    [STAThread]
    public static void Main(string[] args)
    {
        // Linux-friendly global mutex name
        string mutexName = "Global\\ProductionSuite_Unique_Application_Mutex_ID";
        _singleInstanceMutex = new Mutex(true, mutexName, out bool isNewInstance);

        if (!isNewInstance)
        {
            Console.WriteLine("Error: Another instance of this application is already running.");
            return;
        }

        if (args.Length > 0 && args[0].Equals("--headless", StringComparison.OrdinalIgnoreCase))
        {
            RunHeadlessMode(args).GetAwaiter().GetResult();
            return;
        }

        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        finally
        {
            _singleInstanceMutex.ReleaseMutex();
            _singleInstanceMutex.Dispose();
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();

    private static async Task RunHeadlessMode(string[] args)
    {
        Console.WriteLine("[CLI Mode] Initializing automated background scan...");
        string[] forbiddenWords = ["restricted", "confidential", "malware"];
        string outputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "CensoredOutput");
        
        Directory.CreateDirectory(outputDir);
        var scanner = new WordScanner(forbiddenWords, outputDir, _ => {}, _ => {});
        
        await scanner.ExecuteScanAsync(CancellationToken.None);
        Console.WriteLine("[CLI Mode] Processing sequence completed successfully.");
    }
}