using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KYMRFinal.Core;
using KYMRFinal.Level2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using static KYMRFinal.Level2.BallTwo;
using static KYMRFinal.Core.Data;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using System.Net.WebSockets;
using KYMRFinal.Level1;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Audio;

namespace KYMRFinal.Scenes
{
    internal class PlayTwoScene : Component
    {
        public static Random Random;
        public Score _score;
        private List<SpriteTwo> _spritesTwo;
        SpriteFont font;
        StringBuilder capturedInput = new StringBuilder();
        Keys[] lastPressedKeys;
        StringBuilder playerNameInput = new StringBuilder();
        private bool gameOverPopupShown = false;

        internal override void LoadContent(ContentManager Content)
        {
            _score = new Score(Content.Load<SpriteFont>("Fonts/Font"));
            _score.LoadHighScores(); // Load the high scores

            // Load the "click" sound effect
            SoundEffect clickSound = Content.Load<SoundEffect>("Sounds/click");
            SoundEffect dingSound = Content.Load<SoundEffect>("Sounds/ding");

            font = Content.Load<SpriteFont>("Fonts/Font");
            Random = new Random();

            var batTwoTexture = LoadAndScaleTexture(Content, "bat2", 0.3f);
            var leftBatTexture = LoadAndScaleTexture(Content, "bat3", 0.3f);
            var ballTexture = LoadAndScaleTexture(Content, "ball", 0.35f);
            var barrierTexture = LoadAndScaleTexture(Content, "barrier", 0.23f);

            _spritesTwo = new List<SpriteTwo>()
            {
                new SpriteTwo(LoadAndScaleTexture(Content, "background2", 0.342f)),
                new BatTwo(leftBatTexture)
                {
          Position = new Vector2(20, (Data.ScreenH / 2) - (leftBatTexture.Height / 2)),
          Input = new Input()
          {

          }
                },
                new BatTwo(batTwoTexture)
                {
          Position = new Vector2(Data.ScreenW
            -20 - batTwoTexture.Width, (Data.ScreenH / 2) - (batTwoTexture.Height / 2)),
          Input = new Input()
          {
              Up = Keys.Up,
              Down = Keys.Down,
          }
                },
                new BallTwo(ballTexture, clickSound, dingSound)
                {
                    Position = new Vector2((Data.ScreenW / 2) - (ballTexture.Width / 2), (Data.ScreenH / 2) - (ballTexture.Height / 2)),
                    Score = _score,
        }
      };
            var barrier1 = new Barrier(barrierTexture)
            {
                Position = new Vector2(Data.ScreenW - 420, 45), // Adjust the position
            };

            _spritesTwo.Add(barrier1);

            var barrier2 = new Barrier(barrierTexture)
            {
                Position = new Vector2(Data.ScreenW - 420, 505), // Adjust the position
            };

            _spritesTwo.Add(barrier2);

            var barrier3 = new Barrier(barrierTexture)
            {
                Position = new Vector2(Data.ScreenW - 300, 260), // Adjust the position
            };

            _spritesTwo.Add(barrier3);

        }

        // Method to load the image with specific size
        private Texture2D LoadAndScaleTexture(ContentManager content, string assetName, float scale)
        {
            var originalTexture = content.Load<Texture2D>(assetName);

            int scaledWidth = (int)(originalTexture.Width * scale);
            int scaledHeight = (int)(originalTexture.Height * scale);

            RenderTarget2D renderTarget = new RenderTarget2D(originalTexture.GraphicsDevice, scaledWidth, scaledHeight);

            originalTexture.GraphicsDevice.SetRenderTarget(renderTarget);
            originalTexture.GraphicsDevice.Clear(Color.Transparent);

            SpriteBatch spriteBatch = new SpriteBatch(originalTexture.GraphicsDevice);
            spriteBatch.Begin();

            spriteBatch.Draw(originalTexture, new Rectangle(0, 0, scaledWidth, scaledHeight), Color.White);

            spriteBatch.End();

            originalTexture.GraphicsDevice.SetRenderTarget(null);

            Texture2D scaledTexture = new Texture2D(originalTexture.GraphicsDevice, scaledWidth, scaledHeight);
            Color[] data = new Color[scaledWidth * scaledHeight];
            renderTarget.GetData(data);
            scaledTexture.SetData(data);
            return scaledTexture;
        }

        private float speed = 180.0f; // Computer bat speed
        Keys[] previousFrameKeys = new Keys[0];
        bool gameStarted = false;

        internal override void Update(GameTime gameTime)
        {
            // Check if the game is not started and the spacebar key is pressed
            if(!gameStarted && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                gameStarted = true;
            }

            if(gameStarted)
            {
                // Check if the player's score is in the top 5
                if((_score.TopScores2.Any() && _score.Score2 > _score.TopScores2.Min()) || _score.TopScores2.Count < 5)
                {
                    _score.ShowInputDialog = true;
                    gameOverPopupShown = true;
                }

                if(_score.ShowInputDialog)
                {
                    Keys[] keys = Keyboard.GetState().GetPressedKeys();
                    foreach(Keys key in keys)
                    {
                        if(!previousFrameKeys.Contains(key))
                        {
                            HandleKeyPress(key);
                        }
                    }
                    previousFrameKeys = keys;
                }

                if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Data.Exit = true;
                    return;
                }

                if(Keyboard.GetState().IsKeyDown(Keys.Home))
                {
                    Reset();
                    Data.CurrentState = Data.Scenes.Start;
                    return;
                }

                var ball = _spritesTwo.OfType<BallTwo>().FirstOrDefault(); // Get the ball from the sprites
                var firstBat = _spritesTwo.OfType<BatTwo>().FirstOrDefault(); // Automatic movement for the first bat

                if(firstBat != null && ball != null && ball._isPlaying) // Check if the ball is playing
                {
                    // Adjust the speed and direction as needed
                    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    // Update the bat position for continuous movement
                    firstBat.Position += new Vector2(0, speed * elapsed);

                    // Change direction when reaching the top or bottom
                    if(firstBat.Position.Y <= 0 || firstBat.Position.Y >= Data.ScreenH - firstBat.Height)
                    {
                        firstBat.Position.Y = MathHelper.Clamp(firstBat.Position.Y, 0, Data.ScreenH - firstBat.Height);
                        speed *= -1; // Reverse the direction
                    }
                }

                // Handle updates for other sprites
                foreach(var spriteTwo in _spritesTwo)
                {
                    spriteTwo.Update(gameTime, _spritesTwo);
                }
            }
        }

        public void HandleKeyPress(Keys key)
        {
            var alphabetKeys = new List<Keys>() { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z };

            if(_score.ShowInputDialog)
            {
                if(key == Keys.Back && playerNameInput.Length > 0)
                {
                    playerNameInput.Remove(playerNameInput.Length - 1, 1);
                }
                else if(key == Keys.Enter)
                {
                    _score.UpdateTopScores2(playerNameInput.ToString());
                    _score.SaveHighScores();
                    _score.ShowInputDialog = false;
                    playerNameInput.Clear();
                    Data.CurrentState = Data.Scenes.Start;
                    Reset();
                }
                else if(alphabetKeys.Contains(key))
                    playerNameInput.Append(key.ToString());
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            if(_score.ShowInputDialog && ((_score.TopScores2.Any() && _score.Score2 > _score.TopScores2.Min()) || _score.TopScores2.Count < 5) && CurrentGameState == GameState.GameOver)
            {
                spriteBatch.DrawString(font, $"New score record! \nEnter your name: {playerNameInput} \nPress Enter to Save \nReturn to Menu after Save", new Vector2(80, 110), Color.White);
            }
            else if(CurrentGameState == GameState.GameOver)
            {
                spriteBatch.DrawString(font, $"Game Over \nYour score is {_score.Score2} \nPress Home for Menu \nPress Escape to Exit", new Vector2(300, 110), Color.White);
            }
            else
            {
                foreach(var spriteTwo in _spritesTwo)
                    spriteTwo.Draw(spriteBatch);
                _score.Draw(spriteBatch);
            }
        }

        // Method to reset the play panel
        public void Reset()
        {
            _score.Score1 = 0;
            _score.Score2 = 0;
            CurrentGameState = GameState.Playing;
            foreach(var spriteTwo in _spritesTwo)
            {
                if(spriteTwo is BallTwo ballTwo)
                {
                    ballTwo.Restart();
                }
            }
            gameOverPopupShown = false;
        }
    }
}
