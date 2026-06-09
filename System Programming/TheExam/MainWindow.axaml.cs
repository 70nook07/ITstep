using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace TheExam;

public partial class MainWindow : Window
{
    private CancellationTokenSource? _scannerCts;
    private WordScanner? _scannerEngine;
    private ActivityMonitor? _monitorEngine;

    public MainWindow()
    {
        InitializeComponent();
        TxtOutputDir.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Desktop",
            "ScannerOutput");
        TxtMonitorLogPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Desktop", "MonitorLogs");
    }

    // TASK 1: EVENT HANDLERS
    private async void OnBrowseOutputClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = GetTopLevel(this);
        if (topLevel == null) return;
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Target Output Directory", AllowMultiple = false
        });
        if (folders.Count > 0)
        {
            // TryGetLocalPath returns Linux /home/user/Desktop
            string? selectedPath = folders[0].TryGetLocalPath();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                TxtOutputDir.Text = selectedPath;
            }
        }
    }

    private async void OnStartScanClick(object? sender, RoutedEventArgs e)
    {
        string[] keywords =
            TxtForbiddenWords.Text?.Split(',',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];
        string targetDir = TxtOutputDir.Text ?? string.Empty;
        if (keywords.Length == 0 || string.IsNullOrEmpty(targetDir)) return;
        Directory.CreateDirectory(targetDir);
        _scannerCts = new CancellationTokenSource();
        _scannerEngine = new WordScanner(keywords, targetDir,
            log => Dispatcher.UIThread.Post(() => LstScanLogs.Items.Add(log)), pct => Dispatcher.UIThread.Post(() =>
            {
                PrgScanProgress.Value = pct;
                TxtProgressState.Text = $"Status: Scanning System Data... ({pct}%)";
            }));
        BtnStart.IsEnabled = false;
        BtnPause.IsEnabled = true;
        BtnStop.IsEnabled = true;
        LstScanLogs.Items.Clear();
        try
        {
            await _scannerEngine.ExecuteScanAsync(_scannerCts.Token);
            TxtProgressState.Text = "Status: Completed successfully.";
        }
        catch (OperationCanceledException)
        {
            TxtProgressState.Text = "Status: Process terminated by user.";
        }
        finally
        {
            BtnStart.IsEnabled = true;
            BtnPause.IsEnabled = false;
            BtnResume.IsEnabled = false;
            BtnStop.IsEnabled = false;
        }
    }

    private void OnPauseScanClick(object? sender, RoutedEventArgs e)
    {
        _scannerEngine?.Pause();
        BtnPause.IsEnabled = false;
        BtnResume.IsEnabled = true;
        TxtProgressState.Text = "Status: Scan paused.";
    }

    private void OnResumeScanClick(object? sender, RoutedEventArgs e)
    {
        _scannerEngine?.Resume();
        BtnPause.IsEnabled = true;
        BtnResume.IsEnabled = false;
        TxtProgressState.Text = "Status: Scanning data...";
    }

    private void OnStopScanClick(object? sender, RoutedEventArgs e)
    {
        _scannerCts?.Cancel();
    }

    // TASK 2: EVENT HANDLERS

    private void OnToggleMonitorClick(object? sender, RoutedEventArgs e)
    {
        if (_monitorEngine is { IsRunning: true })
        {
            _monitorEngine.Stop();
            TxtMonitorStatus.Text = "Status: Inactive";
            TxtMonitorStatus.Foreground = Avalonia.Media.Brushes.Red;
            return;
        }

        // Banned items
        string[] bannedApps =
            TxtBannedApps.Text?.Split(',',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];
        string[] bannedKeys =
            TxtBannedKeys.Text?.Split(',',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];
        string logRoot = TxtMonitorLogPath.Text ?? string.Empty;
        _monitorEngine = new ActivityMonitor(ChkStatsActive.IsChecked ?? false, ChkModerationActive.IsChecked ?? false,
            bannedApps, bannedKeys, logRoot,
            activity => Dispatcher.UIThread.Post(() => LstActivityLogs.Items.Add(activity)),
            violation => Dispatcher.UIThread.Post(() => LstViolationLogs.Items.Add(violation)));
        _monitorEngine.Start();
        TxtMonitorStatus.Text = "Status: Active & Tracking";
        TxtMonitorStatus.Foreground = Avalonia.Media.Brushes.Green;
    }
}

// Logic

public sealed class WordScanner(
    string[] targetWords,
    string destinationPath,
    Action<string> logUi,
    Action<int> progressUi)
{
    private readonly ManualResetEventSlim _pauseSync = new(true);
    private readonly ConcurrentDictionary<string, int> _wordHitCounters = new();
    private readonly List<string> _discoveredViolations = [];
    public void Pause() => _pauseSync.Reset();
    public void Resume() => _pauseSync.Set();

    public async Task ExecuteScanAsync(CancellationToken token)
    {
        foreach (var word in targetWords) _wordHitCounters[word] = 0;

        // Linux mounted file-system discovery (for different distros)
        List<DirectoryInfo> targetRoots = [];
        if (Directory.Exists("/media")) targetRoots.AddRange(new DirectoryInfo("/media").GetDirectories());
        if (Directory.Exists("/mnt")) targetRoots.AddRange(new DirectoryInfo("/mnt").GetDirectories());
        string userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (Directory.Exists(userHome)) targetRoots.Add(new DirectoryInfo(userHome));
        List<FileInfo> discoverableFiles = [];
        int rootIndex = 0;
        foreach (var root in targetRoots)
        {
            token.ThrowIfCancellationRequested();
            logUi($"[DISCOVERY] Indexing Target Directory tree: {root.FullName}");
            DiscoverFilesRecursively(root, discoverableFiles, token);
            rootIndex++;
            progressUi((int)((double)rootIndex / targetRoots.Count * 15)); // Allocate up to 15% for fast indexing
        }

        int processedCount = 0;
        int totalFiles = discoverableFiles.Count;
        await Task.Run(() =>
        {
            Parallel.ForEach(discoverableFiles,
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount, CancellationToken = token },
                (file) =>
                {
                    _pauseSync.Wait(token);
                    token.ThrowIfCancellationRequested();
                    ProcessTargetFile(file);
                    int state = Interlocked.Increment(ref processedCount);
                    if (state % 10 == 0 || state == totalFiles)
                    {
                        int pct = 15 + (int)((double)state / totalFiles * 85);
                        progressUi(pct);
                    }
                });
        }, token);
        await CompileFinalReportsAsync();
    }

    private void DiscoverFilesRecursively(DirectoryInfo root, List<FileInfo> accumulator, CancellationToken token)
    {
        try
        {
            accumulator.AddRange(root.GetFiles("*.txt")); // Getting txts
            foreach (var dir in root.GetDirectories())
            {
                _pauseSync.Wait(token);
                if (token.IsCancellationRequested) return;

                // Avoid recursive virtual loop directories
                if (dir.Attributes.HasFlag(FileAttributes.ReparsePoint) || dir.Name.StartsWith('.')) continue;
                DiscoverFilesRecursively(dir, accumulator, token);
            }
        }
        catch (UnauthorizedAccessException)
        {
            /* skip root access required files */
        }
    }

    private void ProcessTargetFile(FileInfo file)
    {
        try
        {
            string documentFile = File.ReadAllText(file.FullName);
            bool containsViolation = false;
            int totalReplacementsPerFile = 0;
            StringBuilder outputBuilder = new(documentFile);
            foreach (var word in targetWords)
            {
                int occurrences = 0;
                int searchIndex = documentFile.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                while (searchIndex != -1)
                {
                    occurrences++;
                    totalReplacementsPerFile++;
                    containsViolation = true;

                    // Replace the occurrences
                    outputBuilder.Remove(searchIndex, word.Length);
                    outputBuilder.Insert(searchIndex, "*******");
                    searchIndex = documentFile.IndexOf(word, searchIndex + 7, StringComparison.OrdinalIgnoreCase);
                }

                if (occurrences > 0)
                {
                    _wordHitCounters.AddOrUpdate(word, occurrences, (_, current) => current + occurrences);
                }
            }

            if (containsViolation)
            {
                string uniqueId = Guid.NewGuid().ToString("N")[..6];
                string copyName = $"{Path.GetFileNameWithoutExtension(file.Name)}_{uniqueId}{file.Extension}";
                string originalCopyPath = Path.Combine(destinationPath, copyName);
                string redactedCopyPath = Path.Combine(destinationPath,
                    $"{Path.GetFileNameWithoutExtension(file.Name)}_{uniqueId}_Redacted{file.Extension}");
                File.Copy(file.FullName, originalCopyPath, true);
                File.WriteAllText(redactedCopyPath, outputBuilder.ToString());
                lock (_discoveredViolations)
                {
                    _discoveredViolations.Add(
                        $"Path: {file.FullName} / Size: {file.Length} Bytes / Corrections: {totalReplacementsPerFile}");
                }

                logUi($"[ALERT] Forbidden string detected in: {file.Name}");
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            /* Skip on used system files */
        }
    }

    private async Task CompileFinalReportsAsync()
    {
        string path = Path.Combine(destinationPath, "_ReportSummary.txt");
        StringBuilder manifest = new();
        manifest.AppendLine("~~~~ SYSTEM SCAN REPORT ~~~~");
        manifest.AppendLine($"Executed: {DateTime.Now}\n");
        manifest.AppendLine("~~~~ VIOLATIONS ~~~~");
        lock (_discoveredViolations)
        {
            foreach (var record in _discoveredViolations) manifest.AppendLine(record);
        }

        manifest.AppendLine("\n~~~~ TOP 10 ~~~~");
        var leadingViolators = _wordHitCounters.OrderByDescending(p => p.Value).Take(10);
        foreach (var metric in leadingViolators)
        {
            manifest.AppendLine($"Keyword: [{metric.Key}] -> Occurrences: {metric.Value}");
        }

        await File.WriteAllTextAsync(path, manifest.ToString());
        logUi($"[COMPLETE] Scan summary file written to: {path}");
    }
}

public sealed class ActivityMonitor(
    bool collectStats,
    bool enableMod,
    string[] bannedApps,
    string[] bannedKeys,
    string logPath,
    Action<string> uiActivity,
    Action<string> uiViolation)
{
    private CancellationTokenSource? _monitorCts;
    public bool IsRunning { get; private set; }

    // Linux Input Event Structure matching <linux/input.h>
    [StructLayout(LayoutKind.Sequential)]
    private struct InputEvent
    {
        public long TimeSeconds;
        public long TimeMicroseconds;
        public ushort Type;
        public ushort Code;
        public int Value;
    }

    // Char map
    private static readonly Dictionary<ushort, char> KeyMap = new()
    {
        { 16, 'q' },
        { 17, 'w' },
        { 18, 'e' },
        { 19, 'r' },
        { 20, 't' },
        { 21, 'y' },
        { 22, 'u' },
        { 23, 'i' },
        { 24, 'o' },
        { 25, 'p' },
        { 30, 'a' },
        { 31, 's' },
        { 32, 'd' },
        { 33, 'f' },
        { 34, 'g' },
        { 35, 'h' },
        { 36, 'j' },
        { 37, 'k' },
        { 38, 'l' },
        { 44, 'z' },
        { 45, 'x' },
        { 46, 'c' },
        { 47, 'v' },
        { 48, 'b' },
        { 49, 'n' },
        { 50, 'm' },
        { 57, ' ' } // 57 is Spacebar
    };

    public void Start()
    {
        IsRunning = true;
        _monitorCts = new CancellationTokenSource();
        Directory.CreateDirectory(logPath);

        // Parallel monitoring loops
        Task.Run(() => ProcessMonitoringLoopAsync(_monitorCts.Token));
        if (enableMod && bannedKeys.Length > 0)
        {
            Task.Run(() => KeyLoggerAsync(_monitorCts.Token));
        }
    }

    public void Stop()
    {
        IsRunning = false;
        _monitorCts?.Cancel();
    }

    private async Task ProcessMonitoringLoopAsync(CancellationToken token)
    {
        string statisticsFile = Path.Combine(logPath, "RuntimeStatistics.txt");
        string violationFile = Path.Combine(logPath, "SecurityInterventions.txt");
        while (!token.IsCancellationRequested)
        {
            try
            {
                var processes = Process.GetProcesses();
                foreach (var activeProcess in processes)
                {
                    token.ThrowIfCancellationRequested();
                    string name = activeProcess.ProcessName;
                    if (collectStats)
                    {
                        string trackingPayload = $"[{DateTime.Now:T}] Process Init: {name}";
                        uiActivity(trackingPayload);
                        await File.AppendAllTextAsync(statisticsFile, trackingPayload + Environment.NewLine, token);
                    }

                    if (enableMod && bannedApps.Contains(name, StringComparer.OrdinalIgnoreCase))
                    {
                        try
                        {
                            activeProcess.Kill();
                            string alert = $"[BLOCK] Banned program execution terminated: {name} at {DateTime.Now:T}";
                            uiViolation(alert);
                            await File.AppendAllTextAsync(violationFile, alert + Environment.NewLine, token);
                        }
                        catch
                        {
                        }
                    }
                }

                await Task.Delay(2500, token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                uiActivity($"[MONITOR ERROR]: {ex.Message}");
            }
        }
    }

    // Root-based Key Logger
    private async Task KeyLoggerAsync(CancellationToken token)
    {
        string violationFile = Path.Combine(logPath, "SecurityInterventions.txt");
        string targetDevice = "";

        // Get keyboard device
        if (File.Exists("/proc/bus/input/devices"))
        {
            string[] lines = await File.ReadAllLinesAsync("/proc/bus/input/devices", token);
            string currentHandlers = "";
            bool isKeyboard = false;
            foreach (string line in lines)
            {
                if (line.Contains("EV=120013")) isKeyboard = true;
                if (line.StartsWith("H: Handlers=")) currentHandlers = line;
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (isKeyboard && currentHandlers.Contains("event"))
                    {
                        int index = currentHandlers.IndexOf("event");
                        string eventNum = currentHandlers[index..].Split(' ')[0];
                        targetDevice = $"/dev/input/{eventNum}";
                        break;
                    }

                    isKeyboard = false;
                }
            }
        }

        if (string.IsNullOrEmpty(targetDevice) || !File.Exists(targetDevice))
        {
            targetDevice = "/dev/input/event0";
        }

        try
        {
            using var stream = new FileStream(targetDevice, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            // Linux 64-bit input_event structure layout in bytes:
            // Bytes 0-7  : Time Seconds (long)
            // Bytes 8-15 : Time Microseconds (long)
            // Bytes 16-17: Event Type (ushort)
            // Bytes 18-19: Event Code/Key (ushort)
            // Bytes 20-23: Event Value / State (int)
            byte[] buffer = new byte[24];
            StringBuilder rollBuffer = new();
            uiActivity($"[INIT] Monitored input active on: {targetDevice}");
            while (!token.IsCancellationRequested)
            {
                int bytesRead = await stream.ReadAsync(buffer, token);
                if (bytesRead < 24) continue; // Skip
                
                // Combine bytes into numbers by shifting them left
                // Decode Type (2 bytes at index 16)
                ushort eventType = (ushort)(buffer[16] | (buffer[17] << 8));

                // Decode Code (2 bytes at index 18)
                ushort eventCode = (ushort)(buffer[18] | (buffer[19] << 8));

                // Decode Value (4 bytes at index 20)
                int eventValue = buffer[20] | (buffer[21] << 8) | (buffer[22] << 16) | (buffer[23] << 24);
                
                // Type 1 = "EV_KEY" Key event, Value 1 = Key Pressed
                if (eventType == 1 && eventValue == 1)
                {
                    if (eventCode == 14) // Backspace code
                    {
                        if (rollBuffer.Length > 0)
                        {
                            rollBuffer.Remove(rollBuffer.Length - 1, 1);
                        }
                    }
                    else if (KeyMap.TryGetValue(eventCode, out char character))
                    {
                        rollBuffer.Append(character);

                        // Stops overflow
                        if (rollBuffer.Length > 50)
                        {
                            rollBuffer.Remove(0, rollBuffer.Length - 50);
                        }

                        string currentInputState = rollBuffer.ToString();
                        foreach (var key in bannedKeys)
                        {
                            if (!string.IsNullOrWhiteSpace(key) &&
                                currentInputState.Contains(key, StringComparison.OrdinalIgnoreCase))
                            {
                                string alert =
                                    $"[KEYWORD MATCH] Security Intercept: '{key}' detected in typed sequence.";
                                uiViolation(alert);
                                await File.AppendAllTextAsync(violationFile, alert + Environment.NewLine, token);
                                rollBuffer.Clear(); // clean buffer to avoid duplicate triggers
                                break;
                            }
                        }
                    }
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            uiViolation("[ERROR] Access Denied. Run with 'sudo' to allow reading device files.");
        }
        catch (Exception ex)
        {
            uiActivity($"[KEYLOGGER FAILURE]: {ex.Message}");
        }
    }
}