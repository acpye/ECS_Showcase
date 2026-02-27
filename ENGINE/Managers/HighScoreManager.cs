using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace OpenGL.ENGINE.Managers
{
    public class HighScoreEntry
    {
        public string Initials { get; set; } = "AAA";
        public int Score { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public static class HighScoreManager
    {
        private static readonly List<HighScoreEntry> highScores = new();
        private const int MaxEntries = 10;
        private const string SaveFileName = "highscores.json";

        static HighScoreManager()
        {
            Load();
        }

        public static void AddScore(string initials, int score)
        {
            highScores.Add(new HighScoreEntry
            {
                Initials = initials.ToUpper(),
                Score = score,
                Date = DateTime.Now
            });

            List<HighScoreEntry> sorted = highScores.OrderByDescending(e => e.Score).Take(MaxEntries).ToList();
            highScores.Clear();
            highScores.AddRange(sorted);

            Save();
        }

        public static List<HighScoreEntry> GetHighScores()
        {
            return highScores.OrderByDescending(e => e.Score).ToList();
        }

        public static bool IsHighScore(int score)
        {
            if (highScores.Count < MaxEntries) return true;
            return score > highScores.Min(e => e.Score);
        }

        private static void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(highScores, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SaveFileName, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save high scores: {ex.Message}");
            }
        }

        private static void Load()
        {
            try
            {
                if (File.Exists(SaveFileName))
                {
                    string json = File.ReadAllText(SaveFileName);
                    List<HighScoreEntry> loaded = JsonSerializer.Deserialize<List<HighScoreEntry>>(json);
                    if (loaded != null)
                    {
                        highScores.Clear();
                        highScores.AddRange(loaded);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load high scores: {ex.Message}");
            }
        }
    }
}