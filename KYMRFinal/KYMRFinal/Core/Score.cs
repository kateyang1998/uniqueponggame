using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KYMRFinal.Core
{
    public class Score
    {
        public int Score1; // Computer score
        public int Score2; // Player score
        public List<int> AllScores2; // List to track all player scores
        public List<int> TopScores2; // List to track top 5 player scores
        public List <string> playerName;
        public bool ShowInputDialog = false;


        private SpriteFont _font;

        public Score(SpriteFont font)
        {
            _font = font;
            AllScores2 = new List<int>(); // Initialize the list
            TopScores2 = new List<int>(5); // Initialize the list with a capacity of 5
            playerName = new List<string>(5);
        }

        // Method to update top 5 scores
        public void UpdateTopScores2(string newPlayerName)
        {
            if (TopScores2.Count < 5) 
            {
                TopScores2.Add(Score2);
                playerName.Add(newPlayerName);
            }
            else if (TopScores2.Min() < Score2)
            {
                int nameIndex = TopScores2.IndexOf(TopScores2.Min());
                playerName.RemoveAt(nameIndex);
                TopScores2.Remove(TopScores2.Min());
                TopScores2.Add(Score2);
                playerName.Add(newPlayerName);
            }
        }

        // Method to save player scores
        public void SaveHighScores()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("highscores.txt"))
                { 
                    for (int i = 0; i < 5; i++) 
                    {
                        writer.WriteLine($"{playerName[i]}:{TopScores2[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving high scores: {ex.Message}");
            }
        }

        // Method to load top 5 scores
        public void LoadHighScores()
        {
            if (File.Exists("highscores.txt"))
            {
                using (StreamReader reader = new StreamReader("highscores.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(":");
                        this.playerName.Add(parts.First());
                       if (int.TryParse(parts.Last(), out int score))
                        {
                            TopScores2.Add(score);
                        }
                        else
                        {
                            Console.WriteLine($"Invalid score format: {line}");
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, Score1.ToString(), new Vector2(445, 70), Color.White);
            spriteBatch.DrawString(_font, Score2.ToString(), new Vector2(720, 70), Color.White);
        }
    }
}

