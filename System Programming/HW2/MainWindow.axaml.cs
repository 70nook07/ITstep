using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using System.IO;

namespace FileCopy_Avalonia
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        // folder picker trigger
        private async void BtnFrom_Click(object sender, RoutedEventArgs e)
        {
            ClearStatus();
            
            var files = await this.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Choose a File to Copy",
                AllowMultiple = false
            });

            if (files != null && files.Count > 0)
            {
                TxtFrom.Text = files[0].Path.LocalPath;
            }
        }
        
        private async void BtnTo_Click(object sender, RoutedEventArgs e)
        {
            ClearStatus();

            var folders = await this.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Choose Directory to Copy to",
                AllowMultiple = false
            });

            if (folders != null && folders.Count > 0)
            {
                TxtTo.Text = folders[0].Path.LocalPath;
            }
        }

        // File copying
        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            string sourceFile = TxtFrom.Text?.Trim() ?? string.Empty;
            string targetFolder = TxtTo.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(sourceFile) || string.IsNullOrEmpty(targetFolder))
            {
                DisplayFeedback("Error: Both fields must contain valid paths.", isError: true);
                return;
            }

            if (!File.Exists(sourceFile))
            {
                DisplayFeedback("Error: Target file does not exist.", isError: true);
                return;
            }

            if (!Directory.Exists(targetFolder))
            {
                DisplayFeedback("Error: Destination directory was not found.", isError: true);
                return;
            }

            try
            {
                string nameOfFile = Path.GetFileName(sourceFile);
                string destinationResultPath = Path.Combine(targetFolder, nameOfFile);

                // Copies file with override
                File.Copy(sourceFile, destinationResultPath, overwrite: true);

                DisplayFeedback($"Success! File saved to:\n{destinationResultPath}", isError: false);
            }
            catch (UnauthorizedAccessException)
            {
                DisplayFeedback("Error: Access denied. Missing permission privileges.", isError: true);
            }
            catch (Exception ex)
            {
                DisplayFeedback($"System Exception: {ex.Message}", isError: true);
            }
        }

        private void DisplayFeedback(string message, bool isError)
        {
            TxtStatus.Foreground = isError ? Avalonia.Media.Brushes.Red : Avalonia.Media.Brushes.Green;
            TxtStatus.Text = message;
        }

        private void ClearStatus()
        {
            TxtStatus.Text = string.Empty;
        }
    }
}