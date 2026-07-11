class HW014
{
    static void Main()
    {
        // Start at the app's relative dir
        string currentDir = Directory.GetCurrentDirectory();

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Current Directory: {currentDir}");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            try
            {
                // 1. Show all files and folders
                string[] directories = Directory.GetDirectories(currentDir);
                string[] files = Directory.GetFiles(currentDir);

                Console.WriteLine("Folders:");
                if (directories.Length == 0) Console.WriteLine("  (No folders)");
                foreach (string dir in directories)
                {
                    Console.WriteLine($"  [DIR]  {Path.GetFileName(dir)}");
                }

                Console.WriteLine("\nFiles:");
                if (files.Length == 0) Console.WriteLine("  (No files)");
                foreach (string file in files)
                {
                    Console.WriteLine($"  [FILE] {Path.GetFileName(file)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing contents: {ex.Message}");
            }

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Commands:");
            Console.WriteLine("  cd <dir_name>    - Open folder (Move forward)");
            Console.WriteLine("  cd ..            - Move backward");
            Console.WriteLine("  view <file_name> - Open file and show content");
            Console.WriteLine("  del <name>       - Delete file or folder");
            Console.WriteLine("  ren <old> <new>  - Rename file or folder");
            Console.WriteLine("  exit             - Close application");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.Write("Enter command: ");

            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            // Command split
            string[] parts = input.Split(new[] { ' ' }, 2);
            string command = parts[0].ToLower();
            string args = parts.Length > 1 ? parts[1].Trim() : "";

            try
            {
                if (command == "exit")
                {
                    break;
                }
                // 2 & 4. Navigation (Forward/Backward) and Opening Folders
                else if (command == "cd")
                {
                    if (args == "..")
                    {
                        DirectoryInfo parent = Directory.GetParent(currentDir);
                        if (parent != null)
                        {
                            currentDir = parent.FullName;
                        }
                    }
                    else
                    {
                        string targetDir = Path.Combine(currentDir, args);
                        if (Directory.Exists(targetDir))
                        {
                            currentDir = targetDir;
                        }
                        else
                        {
                            Console.WriteLine("Directory not found.");
                            KeyPause();
                        }
                    }
                }
                // 3. File opener
                else if (command == "view")
                {
                    string filePath = Path.Combine(currentDir, args);
                    if (File.Exists(filePath))
                    {
                        Console.Clear();
                        Console.WriteLine($"~~~~~ Content of: {args} ~~~~~");
                        Console.WriteLine(File.ReadAllText(filePath));
                        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                        KeyPause();
                    }
                    else
                    {
                        Console.WriteLine("File not found.");
                        KeyPause();
                    }
                }
                // 5. Deleter
                else if (command == "del")
                {
                    string targetPath = Path.Combine(currentDir, args);
                    if (File.Exists(targetPath))
                    {
                        File.Delete(targetPath);
                        Console.WriteLine("File deleted successfully.");
                        KeyPause();
                    }
                    else if (Directory.Exists(targetPath))
                    {
                        Directory.Delete(targetPath, true);
                        Console.WriteLine("Directory deleted successfully.");
                        KeyPause();
                    }
                    else
                    {
                        Console.WriteLine("Target not found.");
                        KeyPause();
                    }
                }
                // 6. Renaming a file or a folder using Move()
                else if (command == "ren")
                {
                    string[] renArgs = args.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (renArgs.Length < 2)
                    {
                        Console.WriteLine("Usage: ren <old_name> <new_name>");
                        KeyPause();
                        continue;
                    }

                    string oldPath = Path.Combine(currentDir, renArgs[0]);
                    string newPath = Path.Combine(currentDir, renArgs[1]);

                    if (File.Exists(oldPath))
                    {
                        File.Move(oldPath, newPath);
                        Console.WriteLine("File renamed successfully.");
                        KeyPause();
                    }
                    else if (Directory.Exists(oldPath))
                    {
                        Directory.Move(oldPath, newPath);
                        Console.WriteLine("Directory renamed successfully.");
                        KeyPause();
                    }
                    else
                    {
                        Console.WriteLine("Source file or folder not found.");
                        KeyPause();
                    }
                }
                else
                {
                    Console.WriteLine("Unknown command.");
                    KeyPause();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                KeyPause();
            }
        }
    }

    static void KeyPause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}
