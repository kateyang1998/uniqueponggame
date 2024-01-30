using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using KYMRFinal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KYMRFinal.Scenes
{
    // Top 5 score board scene
    internal class HighScoreScene : Component
    {
        private List<string> playerNames;
        private List<int> highScores;
        SpriteFont font;
        private List<HighScoreEntry> highScoreEntries;  // This is declared here
        private Texture2D scoreTitle;

        public HighScoreScene()
        {
            highScoreEntries = new List<HighScoreEntry>();
        }
        internal override void LoadContent(ContentManager Content)
        {
            scoreTitle = Content.Load<Texture2D>("scoretitle");

            // Initialize the lists
            font = Content.Load<SpriteFont>("Fonts/Font");
            playerNames = new List<string>();
            highScores = new List<int>();


            // Load high scores and sort them
            LoadAndSortHighScores();
        }

        private void LoadAndSortHighScores()
        {
            // Clear existing data
            highScoreEntries.Clear();  // Remove this line

            // Load high scores from the file
            if (File.Exists("highscores.txt"))
            {
                using (StreamReader reader = new StreamReader("highscores.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(":");
                        if (int.TryParse(parts.Last(), out int score))
                        {
                            var entry = new HighScoreEntry
                            {
                                PlayerName = parts.First(),
                                Score = score
                            };
                            highScoreEntries.Add(entry);
                        }
                        else
                        {
                            Console.WriteLine($"Invalid score format: {line}");
                        }
                    }
                }
            }

            // Sort high scores in descending order
            var sortedHighScores = highScoreEntries.OrderByDescending(entry => entry.Score).ToList();

            // Update the list with sorted data
            highScoreEntries = sortedHighScores;
        }
        internal override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Home))
            {
                Data.CurrentState = Data.Scenes.Start;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            // Display the title image with specific size
            float scale = 0.5f;
            spriteBatch.Draw(scoreTitle, new Vector2(320, 50), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            Vector2 position = new Vector2(420, 200); // Adjust the position as needed

            for (int i = 0; i < highScoreEntries.Count; i++)
            {
                string entry = $"{i + 1}. {highScoreEntries[i].PlayerName}: {highScoreEntries[i].Score}";
                spriteBatch.DrawString(font, entry, position, Color.White);
                position.Y += font.LineSpacing;
            }
        }
    }
}


