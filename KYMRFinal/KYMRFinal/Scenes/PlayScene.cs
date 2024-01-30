using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KYMRFinal.Core;
using KYMRFinal.Level1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using static KYMRFinal.Level1.Ball;
using static KYMRFinal.Core.Data;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Microsoft.Xna.Framework.Audio;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Media;
using KYMRFinal.Managers;
using System.Text.RegularExpressions;

namespace KYMRFinal.Scenes
{
    // Easy mode scene
    internal class PlayScene : Component
    {
        public static Random Random;
        public Score _score;
        private List<SpriteOne> _spritesOne;
        SpriteFont font;
        StringBuilder capturedInput = new StringBuilder();
        Keys[] lastPressedKeys;
        StringBuilder playerNameInput = new StringBuilder();

        internal override void LoadContent(ContentManager Content)
        {
            _score = new Score(Content.Load<SpriteFont>("Fonts/Font"));
            _score.LoadHighScores(); // Load the high scores

            // Load the "click" sound effect
            SoundEffect clickSound = Content.Load<SoundEffect>("Sounds/click");
            SoundEffect dingSound = Content.Load<SoundEffect>("Sounds/ding");

            font = Content.Load<SpriteFont>("Fonts/Font");
            Random = new Random();

            var batOneTexture = LoadAndScaleTexture(Content, "bat1", 0.3f);
            var ballTexture = LoadAndScaleTexture(Content, "ball", 0.4f);

            _spritesOne = new List<SpriteOne>()
            {
                new SpriteOne(LoadAndScaleTexture(Content, "background", 0.342f)),
                new BatOne(batOneTexture)
                {
          Position = new Vector2(20, (Data.ScreenH / 2) - (batOneTexture.Height / 2)),
          Input = new Input()
          {

          }
                },
                new BatOne(batOneTexture)
                {
          Position = new Vector2(Data.ScreenW
            -20 - batOneTexture.Width, (Data.ScreenH / 2) - (batOneTexture.Height / 2)),
          Input = new Input()
          {
              Up = Keys.Up,
              Down = Keys.Down,
          }
                },
                new Ball(ballTexture, clickSound, dingSound)
                {
                    Position = new Vector2((Data.ScreenW / 2) - (ballTexture.Width / 2), (Data.ScreenH / 2) - (ballTexture.Height / 2)),
                    Score = _score,
        }
      };
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

        private float speed = 210.0f; // Computer bat speed
        private bool gameOverPopupShown = false;

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
                    Data.CurrentState = Data.Scenes.Start;
                    Reset();
                    return;
                }

                var ball = _spritesOne.OfType<Ball>().FirstOrDefault(); // Get the ball from the sprites
                var firstBat = _spritesOne.OfType<BatOne>().FirstOrDefault(); // Automatic movement for the first bat

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
                foreach(var spriteOne in _spritesOne)
                {
                    spriteOne.Update(gameTime, _spritesOne);
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
                foreach(var spriteOne in _spritesOne)
                    spriteOne.Draw(spriteBatch);
                _score.Draw(spriteBatch);
            }
        }

        // Method to reset the play panel
        public void Reset()
        {
            _score.Score1 = 0;
            _score.Score2 = 0;
            CurrentGameState = GameState.Playing;
            foreach(var spriteOne in _spritesOne)
            {
                if(spriteOne is Ball ball)
                {
                    ball.Restart();
                }
            }
            gameOverPopupShown = false;
        }
    }
}
