using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;        // for reading process data directly from /proc/[pid]
using System.Linq;      // for sorting the list

namespace AvaloniaApplication2;

public partial class MainWindow : Window
{
    private List<ProcessInfo> processes = new List<ProcessInfo>();
    private DispatcherTimer autoRefreshTimer;
    private int currentIntervalSec = 2;

    public MainWindow()
    {
        InitializeComponent();

        ProcessesList.ItemsSource = processes;
        DataContext = this;

        RefreshButton.Click += OnRefreshClicked;
        SigTermButton.Click += OnSigTermClicked;
        SigKillButton.Click += OnSigKillClicked;
        DetailsButton.Click += OnDetailsClicked;
        LaunchButton.Click += OnLaunchClicked;
        IntervalCombo.SelectionChanged += OnIntervalChanged;

        StartAutoRefresh(2);
        RefreshProcessList();
    }

    private void OnIntervalChanged(object sender, SelectionChangedEventArgs e)
    {
        if (IntervalCombo.SelectedItem is ComboBoxItem item)
        {
            string text = item.Content.ToString();

            if (text == "Paused")
            {
                StopAutoRefresh();
                StatusText.Text = "Auto-refresh paused";
                return;
            }

            int seconds = int.Parse(text.Split(' ')[0]);
            StartAutoRefresh(seconds);
        }
    }

    private void StartAutoRefresh(int seconds)
    {
        StopAutoRefresh();

        currentIntervalSec = seconds;
        autoRefreshTimer = new DispatcherTimer();
        autoRefreshTimer.Interval = TimeSpan.FromSeconds(seconds);
        autoRefreshTimer.Tick += TimerTick;
        autoRefreshTimer.Start();

        StatusText.Text = "Auto-refresh every " + seconds + " sec";
    }

    private void TimerTick(object sender, EventArgs e)
    {
        RefreshProcessList();
    }

    private void StopAutoRefresh()
    {
        if (autoRefreshTimer != null)
        {
            autoRefreshTimer.Stop();
            autoRefreshTimer = null;
        }
    }

    private void OnRefreshClicked(object sender, RoutedEventArgs e)
    {
        RefreshProcessList();
    }

    private void OnSigTermClicked(object sender, RoutedEventArgs e)
    {
        if (ProcessesList.SelectedItem is not ProcessInfo selected)
        {
            StatusText.Text = "No process selected";
            return;
        }

        try
        {
            Process.Start("kill", "-15 " + selected.Pid);   // -15 == SIGTERM

            StatusText.Text = "SIGTERM sent";
            RefreshProcessList();
        }
        catch (Exception ex)
        {
            StatusText.Text = ex.Message;
        }
    }

    private void OnSigKillClicked(object sender, RoutedEventArgs e)
    {
        if (ProcessesList.SelectedItem is not ProcessInfo selected)
        {
            StatusText.Text = "No process selected";
            return;
        }

        try
        {
            Process process = Process.GetProcessById(selected.Pid);
            // process.Start("kill", "-9 " + selected.Pid);   // -9 == SIGKILL
            process.Kill();

            StatusText.Text = "SIGKILL sent";
            RefreshProcessList();
        }
        catch (Exception ex)
        {
            StatusText.Text = ex.Message;
        }
    }

    private void OnDetailsClicked(object sender, RoutedEventArgs e)
    {
        if (ProcessesList.SelectedItem is not ProcessInfo selected)
        {
            StatusText.Text = "No process selected";
            return;
        }

        string details =
            "Process: " + selected.Name + "\n" +
            "PID: " + selected.Pid + "\n" +
            "Memory: " + selected.MemoryKB + " KB\n" +
            "Threads: " + selected.ThreadCount + "\n" +
            "Priority: " + GetProcessPriority(selected.Pid) + "\n" +
            "State: " + selected.Status + "\n" +
            "Command line: " + GetCommandLine(selected.Pid);

        ShowMessageBoxSync("Details", details);
    }

    private void OnLaunchClicked(object? sender, RoutedEventArgs e)
    {
        TextBox pathBox = new TextBox();
        TextBox argsBox = new TextBox();

        TextBlock pathLabel = new TextBlock();
        pathLabel.Text = "Executable path:";

        TextBlock argsLabel = new TextBlock();
        argsLabel.Text = "Arguments:";

        Button launchButton = new Button();
        launchButton.Content = "Launch";
        launchButton.Width = 80;

        Button cancelButton = new Button();
        cancelButton.Content = "Cancel";
        cancelButton.Width = 80;

        StackPanel buttonPanel = new StackPanel();
        buttonPanel.Orientation = Orientation.Horizontal;
        buttonPanel.Spacing = 10;
        buttonPanel.Children.Add(launchButton);
        buttonPanel.Children.Add(cancelButton);

        StackPanel mainPanel = new StackPanel();
        mainPanel.Margin = new Thickness(10);
        mainPanel.Spacing = 10;

        mainPanel.Children.Add(pathLabel);
        mainPanel.Children.Add(pathBox);
        mainPanel.Children.Add(argsLabel);
        mainPanel.Children.Add(argsBox);
        mainPanel.Children.Add(buttonPanel);

        Window dialog = new Window();
        dialog.Title = "Launch Process";
        dialog.Width = 450;
        dialog.Height = 200;
        dialog.Content = mainPanel;

        launchButton.Click += (s, ev) =>
        {
            try
            {
                Process.Start(pathBox.Text, argsBox.Text);
                dialog.Close();
                RefreshProcessList();
            }
            catch (Exception ex)
            {
                ShowMessageBoxSync("Error", ex.Message);
            }
        };

        cancelButton.Click += (s, ev) =>
        {
            dialog.Close();
        };

        dialog.ShowDialog(this);
    }

    private void ShowMessageBoxSync(string title, string message)
    {
        Window msgBox = new Window();
        msgBox.Title = title;
        msgBox.Width = 400;
        msgBox.Height = 200;

        StackPanel panel = new StackPanel();
        panel.Margin = new Thickness(10);

        TextBlock text = new TextBlock();
        text.Text = message;

        Button button = new Button();
        button.Content = "OK";
        button.Click += CloseMessageBox;

        panel.Children.Add(text);
        panel.Children.Add(button);

        msgBox.Content = panel;
        msgBox.ShowDialog(this);

        void CloseMessageBox(object sender, RoutedEventArgs e)
        {
            msgBox.Close();
        }
    }

    private void RefreshProcessList()       // recreating list for each refresh
{
    try
    {
        processes.Clear();

        Process[] allProcesses = Process.GetProcesses();

        foreach (Process p in allProcesses.OrderBy(x => x.Id))
        {
            ProcessInfo info = new ProcessInfo();
            info.Pid = p.Id;
            info.Name = SafeGetProcessName(p);
            info.MemoryKB = SafeGetMemoryKB(p);
            info.ThreadCount = SafeGetThreadCount(p);
            info.StartTime = SafeGetStartTime(p);
            info.Status = GetProcessStatus(p.Id);
            info.Priority = GetProcessPriority(p.Id);

            processes.Add(info);
        }

        ProcessesList.ItemsSource = null;
        ProcessesList.ItemsSource = processes;

        StatusText.Text = "Updated: " + processes.Count + " processes";
    }
    catch (Exception ex)
    {
        StatusText.Text = ex.Message;
    }
}

    // Statuses
    private string GetProcessStatus(int pid)
    {
        try
        {
            string stat = File.ReadAllText("/proc/" + pid + "/stat");

            int end = stat.LastIndexOf(')');
            if (end == -1)
                return "Unknown";

            string after = stat.Substring(end + 2);
            char state = after[0];

            if (state == 'R') return "Running";
            if (state == 'S') return "Sleeping";
            if (state == 'D') return "Waiting";
            if (state == 'T') return "Stopped";
            if (state == 'Z') return "Zombie";
            if (state == 'X') return "Dead";

            return "Unknown";
        }
        catch
        {
            return "Unknown";
        }
    }

    private int GetProcessPriority(int pid)
    {
        try
        {
            string stat = File.ReadAllText("/proc/" + pid + "/stat");

            int end = stat.LastIndexOf(')');
            if (end == -1)
                return 0;

            string after = stat.Substring(end + 2);
            string[] parts = after.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 19 && int.TryParse(parts[18], out int nice))
                return nice;

            return 0;
        }
        catch
        {
            return 0;
        }
    }

    private string GetCommandLine(int pid)
    {
        try
        {
            string cmdline = File.ReadAllText("/proc/" + pid + "/cmdline");

            if (string.IsNullOrWhiteSpace(cmdline))
                return "[kernel thread]";

            return cmdline.Replace('\0', ' ');
        }
        catch
        {
            return "N/A";
        }
    }

    private static string SafeGetProcessName(Process p)
    {
        try { return p.ProcessName; }
        catch { return "Unknown"; }
    }

    private static long SafeGetMemoryKB(Process p)
    {
        try { return p.WorkingSet64 / 1024; }
        catch { return 0; }
    }

    private static int SafeGetThreadCount(Process p)
    {
        try { return p.Threads.Count; }
        catch { return 0; }
    }

    private static DateTime? SafeGetStartTime(Process p)
    {
        try { return p.StartTime; }
        catch { return null; }
    }
}

public class ProcessInfo
{
    public int Pid { get; set; }
    public string Name { get; set; }
    public long MemoryKB { get; set; }
    public int ThreadCount { get; set; }
    public DateTime? StartTime { get; set; }
    public string Status { get; set; }
    public int Priority { get; set; }
}