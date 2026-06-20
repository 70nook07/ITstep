using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NET_Prog
{
    class HW01
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\tSOCKETS TASKS");
            Console.WriteLine("1. Run Task 1: Server (Greeting)");
            Console.WriteLine("2. Run Task 1: Client (Greeting)");
            Console.WriteLine("3. Run Task 2: Server (Date/Time Provider)");
            Console.WriteLine("4. Run Task 2: Client (Date/Time Requester)");
            Console.WriteLine("--------------------");
            Console.Write("Select an option (1-4): ");
            
            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    RunTask1Server();
                    break;
                case "2":
                    RunTask1Client();
                    break;
                case "3":
                    RunTask2Server();
                    break;
                case "4":
                    RunTask2Client();
                    break;
                default:
                    Console.WriteLine("Invalid option selected. Exiting.");
                    break;
            }

            Console.WriteLine("\nPress a key to close...");
            Console.ReadLine();
        }
        
        // SOCKET METHODS
        static void SendString(Socket socket, string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            socket.Send(data);
        }

        static string ReceiveString(Socket socket)
        {
            byte[] buffer = new byte[1024];
            int bytesReceived = socket.Receive(buffer);
            return Encoding.UTF8.GetString(buffer, 0, bytesReceived);
        }
        
        // TASK 1
        static void RunTask1Server()
        {
            Console.WriteLine("[Task 1 Server] Init...");

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);
                Console.WriteLine("[Task 1 Server] Service online. Waiting for client on 127.0.0.1...");

                Socket handler = listenSocket.Accept();

                string receivedMessage = ReceiveString(handler);
                
                string clientIp = "127.0.0.1";
                string currentTime = DateTime.Now.ToString("HH:mm");

                Console.WriteLine($"At {currentTime} from [{clientIp}] received string: {receivedMessage}");

                SendString(handler, "Hello, client!");

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server Error: {ex.Message}");
            }
            finally
            {
                listenSocket.Close();
            }
        }

        static void RunTask1Client()
        {
            Console.WriteLine("[Task 1 Client] Preparing connection...");
            
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(ipPoint);
                Console.WriteLine("[Task 1 Client] Connected to server at 127.0.0.1.");

                SendString(socket, "Hello, server!");

                string responseMessage = ReceiveString(socket);
                
                string serverIp = "127.0.0.1";
                string currentTime = DateTime.Now.ToString("HH:mm");

                Console.WriteLine($"At {currentTime} from [{serverIp}] received string: {responseMessage}");

                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client Error: {ex.Message}");
            }
            finally
            {
                socket.Close();
            }
        }

        // TASK 2: DATE / TIME 
        static void RunTask2Server()
        {
            Console.WriteLine("[Task 2 Server] Init...");
            
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);
                Console.WriteLine("[Task 2 Server] Service online. Awaiting requests on 127.0.0.1...");

                Socket handler = listenSocket.Accept();

                string clientRequest = ReceiveString(handler).Trim().ToLower();
                string responseData = "";

                if (clientRequest == "time")
                {
                    responseData = DateTime.Now.ToString("HH:mm:ss");
                }
                else if (clientRequest == "date")
                {
                    responseData = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    responseData = "Error: Unknown Request command. Use 'time' or 'date'.";
                }

                SendString(handler, responseData);
                Console.WriteLine($"[Task 2 Server] Processed request: '{clientRequest}' for client 127.0.0.1. Disconnecting.");

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server Error: {ex.Message}");
            }
            finally
            {
                listenSocket.Close();
            }
        }

        static void RunTask2Client()
        {
            Console.Write("Enter request type ('time' or 'date'): ");
            string userInput = Console.ReadLine();

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(ipPoint);

                SendString(socket, userInput);

                string receivedData = ReceiveString(socket);
                
                Console.WriteLine(receivedData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client Error: {ex.Message}");
            }
            finally
            {
                socket.Close();
            }
        }
    }
}
