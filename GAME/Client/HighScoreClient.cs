using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;

namespace OpenGL.ENGINE.Managers
{
    public static class HighScoreClient
    {
        private const string ServerHost = "localhost";
        private const int ServerPort = 8080;

        public static List<HighScoreEntry> GetHighScores()
        {
            try
            {
                string response = SendCommand("GET");
                if (response.StartsWith("SCORES|"))
                {
                    string json = response.Substring(7);
                    return JsonSerializer.Deserialize<List<HighScoreEntry>>(json) ?? new();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get high scores from server: {ex.Message}");
            }

             return HighScoreManager.GetHighScores();
        }

        public static bool AddHighScore(string initials, int score)
        {
            try
            {
                string response = SendCommand($"ADD|{initials}|{score}");
                if (response.StartsWith("OK|"))
                {
                    HighScoreManager.AddScore(initials, score);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add high score to server: {ex.Message}");
                HighScoreManager.AddScore(initials, score);
            }
            return false;
        }

        public static bool IsHighScore(int score)
        {
            try
            {
                string response = SendCommand($"CHECK|{score}");
                if (response.StartsWith("ISHIGH|"))
                {
                    return bool.Parse(response.Substring(7));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to check high score with server: {ex.Message}");
            }

            return HighScoreManager.IsHighScore(score);
        }

        private static string SendCommand(string command)
        {
            using TcpClient client = new TcpClient(ServerHost, ServerPort);
            using NetworkStream stream = client.GetStream();
            using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
            using StreamReader reader = new StreamReader(stream);

            writer.WriteLine(command);
            string? response = reader.ReadLine();
            return response ?? "ERROR|No response";
        }
    }
}