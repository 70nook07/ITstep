using System.Text.Json;

class HW4
{
    private static readonly HttpClient Client = new();

    static async Task Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Star Wars API Viewer ===");
            Console.WriteLine("1. People");
            Console.WriteLine("2. Films");
            Console.WriteLine("3. Starships");
            Console.WriteLine("4. Vehicles");
            Console.WriteLine("5. Species");
            Console.WriteLine("6. Planets");
            Console.WriteLine("7. Exit");
            Console.Write("Select a category: ");

            string choice = Console.ReadLine();
            if (choice == "7") break;

            string? endpoint = choice switch
            {
                "1" => "people",
                "2" => "films",
                "3" => "starships",
                "4" => "vehicles",
                "5" => "species",
                "6" => "planets",
                _ => null
            };

            if (endpoint == null)
            {
                Console.WriteLine("Invalid choice. Press any key...");
                Console.ReadKey();
                continue;
            }

            await FetchData(endpoint);
        }
    }

    static async Task FetchData(string endpoint)
    {
        Console.WriteLine($"\nLoading {endpoint} from API...");
        try
        {
            string url = $"https://swapi.py4e.com/api/{endpoint}/";
            string response = await Client.GetStringAsync(url);

            using (JsonDocument doc = JsonDocument.Parse(response))
            {
                JsonElement root = doc.RootElement;
                JsonElement results = root.GetProperty("results");

                Console.WriteLine($"\n~~~ {endpoint.ToUpper()} LIST ~~~");
                foreach (JsonElement item in results.EnumerateArray())
                {
                    string displayProp = endpoint == "films" ? "title" : "name";
                    Console.WriteLine($"- {item.GetProperty(displayProp).GetString()}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}
