using System.Net;
using System.Net.Sockets;

namespace NET_Prog
{
    class HW02
    {
        static async Task Main(string[] args)
        {
            // Server task in the background
            Task serverTask = Task.Run(() => QuoteServer.StartAsync());

            // Start the client on the main thread
            await QuoteClient.StartAsync();

            await serverTask;
        }
    }

    // SERVER
    public static class QuoteServer
    {
        private const int Port = 12345;
        
        private static readonly string[] Quotes = {
            "The only way to do great work is to love what you do. - Steve Jobs",
            "Success is not final, failure is not fatal: it is the courage to continue that counts. - Winston Churchill",
            "Believe you can and you're halfway there. - Theodore Roosevelt",
            "Strive not to be a success, but rather to be of value. - Albert Einstein",
            "Your time is limited, so don't waste it living someone else's life. - Steve Jobs"
        };

        public static async Task StartAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, Port);
            listener.Start();

            // LOG
            Console.WriteLine($"[SERVER] Server started on port {Port}. Listening for connections...");
            Console.WriteLine("[SERVER] Available Quote Dataset Loaded:");
            foreach (var quote in Quotes)
            {
                Console.WriteLine($"  -> \"{quote}\"");
            }
            Console.WriteLine("----------------------------------------------------------------------\n");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                _ = HandleClientAsync(client);
            }
        }

        private static async Task HandleClientAsync(TcpClient client)
        {
            // LOG
            string clientInfo = client.Client.RemoteEndPoint.ToString();
            DateTime connectionTime = DateTime.Now;
            Console.WriteLine($"\n[SERVER] [CONNECT] User: {clientInfo} | Time: {connectionTime}");

            try
            {
                using NetworkStream stream = client.GetStream();
                using StreamReader reader = new StreamReader(stream);
                using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

                Random random = new Random();
                string request;

                // Read incoming commands
                while ((request = await reader.ReadLineAsync()) != null)
                {
                    if (request.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    if (request.Equals("get_quote", StringComparison.OrdinalIgnoreCase))
                    {
                        // Select and send a random quote
                        string randomQuote = Quotes[random.Next(Quotes.Length)];
                        await writer.WriteLineAsync(randomQuote);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                // LOG
                Console.WriteLine($"\n[SERVER] [DISCONNECT] User: {clientInfo} | Time: {DateTime.Now}");
                client.Close();
            }
        }
    }

    // CLIENT
    public static class QuoteClient
    {
        private const string Host = "127.0.0.1";
        private const int Port = 12345;

        public static async Task StartAsync()
        {
            try
            {
                using TcpClient client = new TcpClient();
                await client.ConnectAsync(Host, Port);

                using NetworkStream stream = client.GetStream();
                using StreamReader reader = new StreamReader(stream);
                using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

                Console.WriteLine("[CLIENT] Connected securely to the Quote Server.");
                Console.WriteLine("[CLIENT] Controls: Press [ENTER] to get a quote. Type 'exit' to quit.\n");

                while (true)
                {
                    Console.Write("[CLIENT] Enter command: ");
                    string input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        await writer.WriteLineAsync("get_quote");
                        
                        string receivedQuote = await reader.ReadLineAsync();
                        Console.WriteLine($"[CLIENT] Quote received: \"{receivedQuote}\"\n");
                    }
                    else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        
                        await writer.WriteLineAsync("exit");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("[CLIENT] Command not recognized. Press [ENTER] or type 'exit'.\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CLIENT ERROR] Connection failed: {ex.Message}");
            }

            Console.WriteLine("[CLIENT] Session ended. Goodbye!");
        }
    }
}
