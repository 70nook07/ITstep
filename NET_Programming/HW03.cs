using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private const int Port = 8888;
    private const string Host = "127.0.0.1";
    private const int TotalRounds = 5;

    static async Task Main(string[] args)
    {
        Console.Title = "Rock-Paper-Scissors Game";
        Console.WriteLine("Start 3 consoles to play the game");
        Console.WriteLine("1. Start Server (Host)");
        Console.WriteLine("2. Start Client (Player)");
        Console.Write("Choose option (1-2): ");
        
        string choice = Console.ReadLine()?.Trim();
        if (choice == "1")
        {
            await RunServerAsync();
        }
        else if (choice == "2")
        {
            await RunClientAsync();
        }
        else
        {
            Console.WriteLine("Invalid selection. Exiting.");
        }
    }

    #region Server (Referee Host)
    static async Task RunServerAsync()
    {
        TcpListener server = new TcpListener(IPAddress.Any, Port);
        server.Start();
        Console.WriteLine($"[Server] Started on port {Port}. Waiting for players...");

        using TcpClient client1 = await server.AcceptTcpClientAsync();
        Console.WriteLine("[Server] Player 1 connected.");
        using TcpClient client2 = await server.AcceptTcpClientAsync();
        Console.WriteLine("[Server] Player 2 connected. Starting game...");

        await using var p1Stream = client1.GetStream();
        await using var p2Stream = client2.GetStream();
        using var r1 = new StreamReader(p1Stream, Encoding.UTF8);
        await using var w1 = new StreamWriter(p1Stream, Encoding.UTF8) { AutoFlush = true };
        using var r2 = new StreamReader(p2Stream, Encoding.UTF8);
        await using var w2 = new StreamWriter(p2Stream, Encoding.UTF8) { AutoFlush = true };

        int p1Score = 0, p2Score = 0;

        for (int round = 1; round <= TotalRounds; round++)
        {
            // Send game state to players: Round|YourScore|OpponentScore
            await w1.WriteLineAsync($"{round}|{p1Score}|{p2Score}");
            await w2.WriteLineAsync($"{round}|{p2Score}|{p1Score}");

            // Read choices
            Task<string> p1ChoiceTask = r1.ReadLineAsync();
            Task<string> p2ChoiceTask = r2.ReadLineAsync();
            await Task.WhenAll(p1ChoiceTask, p2ChoiceTask);

            string c1 = p1ChoiceTask.Result;
            string c2 = p2ChoiceTask.Result;

            string res1 = Evaluate(c1, c2);
            string res2 = Evaluate(c2, c1);

            if (res1 == "Win") p1Score++;
            if (res2 == "Win") p2Score++;

            // Send round results: OpponentChoice|Result
            await w1.WriteLineAsync($"{c2}|{res1}");
            await w2.WriteLineAsync($"{c1}|{res2}");
        }

        // Send game over state
        await w1.WriteLineAsync($"GameOver|{p1Score}|{p2Score}");
        await w2.WriteLineAsync($"GameOver|{p2Score}|{p1Score}");
        
        server.Stop();
        Console.WriteLine("[Server] Game finished. Shutting down.");
    }
    #endregion

    // Players
    static async Task RunClientAsync()
    {
        using TcpClient client = new TcpClient();
        try
        {
            await client.ConnectAsync(Host, Port);
            Console.WriteLine("Connected! Waiting for opponent...");
        }
        catch
        {
            Console.WriteLine("Could not connect to server. Is host running?");
            return;
        }

        using var stream = client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

        while (true)
        {
            string state = await reader.ReadLineAsync();
            if (state == null) break;

            string[] stateParts = state.Split('|');
            if (stateParts[0] == "GameOver")
            {
                int myFinal = int.Parse(stateParts[1]);
                int oppFinal = int.Parse(stateParts[2]);
                PrintFinalResult(myFinal, oppFinal);
                break;
            }

            int round = int.Parse(stateParts[0]);
            int myScore = int.Parse(stateParts[1]);
            int oppScore = int.Parse(stateParts[2]);

            Console.Clear();
            Console.WriteLine($"\tROUND {round} / {TotalRounds}");
            Console.WriteLine($"Score -> You: {myScore} | Opponent: {oppScore}\n");

            string myChoice = GetValidChoice();
            await writer.WriteLineAsync(myChoice);

            Console.WriteLine("\nWaiting for opponent's move...");
            string[] outcomeParts = (await reader.ReadLineAsync()).Split('|');
            string oppChoice = outcomeParts[0];
            string result = outcomeParts[1];

            Console.Clear();
            Console.WriteLine($"\tROUND {round} RESULT");
            Console.WriteLine($"You chose: {myChoice}");
            Console.WriteLine($"Opponent chose: {oppChoice}");
            Console.WriteLine($"Result: {result.ToUpper()}");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    // Methods
    static string GetValidChoice()
    {
        while (true)
        {
            Console.Write("Enter (R)ock, (P)aper, or (S)cissors: ");
            string input = Console.ReadLine()?.Trim().ToUpper();
            if (input == "R" || input == "P" || input == "S") return input;
            Console.WriteLine("Invalid input. Use R, P, or S.");
        }
    }

    static string Evaluate(string me, string opponent)
    {
        if (me == opponent) return "Draw";
        if ((me == "R" && opponent == "S") || 
            (me == "P" && opponent == "R") || 
            (me == "S" && opponent == "P")) return "Win";
        return "Lose";
    }

    static void PrintFinalResult(int mine, int opp)
    {
        Console.Clear();
        Console.WriteLine("\tFINAL MATCH RESULT");
        Console.WriteLine($"Final Score -> You: {mine} | Opponent: {opp}\n");
        if (mine == opp) Console.WriteLine("The match is a DRAW!");
        else if (mine > opp) Console.WriteLine("VICTORY!");
        else Console.WriteLine("DEFEAT!");
        Console.WriteLine("\nDisconnected. Press any key to exit.");
        Console.ReadLine();
    }
}
