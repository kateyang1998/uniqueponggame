using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KYMRFinal.Core;
using KYMRFinal.Level1;
using KYMRFinal.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using static KYMRFinal.Core.Data;

namespace KYMRFinal.Level2
{
    public class BallTwo : SpriteTwo
    {
        private float _timer = 0f; // Incrementing the speed over time
        private Vector2? _startPosition = null;
        private float? _startSpeed;
        public bool _isPlaying;
        private SoundEffect clickSound;
        private SoundEffect dingSound;


        public Score Score;
        public int SpeedIncrementSpan = 10; // How often the speed will increment

        // Ball speed
        public BallTwo(Texture2D texture, SoundEffect clickSound, SoundEffect dingSound) : base(texture)
        {
            Speed = 6f;
            this.clickSound = clickSound;
            this.dingSound = dingSound;
        }


        public override void Update(GameTime gameTime, List<SpriteTwo> spritesTwo)
        {
            if (_startPosition == null)
            {
                _startPosition = Position;
                _startSpeed = Speed;

                Restart();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                _isPlaying = true;

            if (!_isPlaying)
                return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > SpeedIncrementSpan)
            {
                Speed++;
                _timer = 0;
            }

            foreach (var spriteTwo in spritesTwo)
            {
                if (spriteTwo == this)
                    continue;

                if (this.Velocity.X > 0 && this.IsTouchingLeft(spriteTwo))
                {
                    this.Velocity.X = -this.Velocity.X;
                    PlayHitSound();
                }
                if (this.Velocity.X < 0 && this.IsTouchingRight(spriteTwo))
                {
                    this.Velocity.X = -this.Velocity.X;
                    PlayHitSound();
                }
                if (this.Velocity.Y > 0 && this.IsTouchingTop(spriteTwo))
                {
                    this.Velocity.Y = -this.Velocity.Y;
                    PlayHitSound();
                }
                if (this.Velocity.Y < 0 && this.IsTouchingBottom(spriteTwo))
                {
                    this.Velocity.Y = -this.Velocity.Y;
                    PlayHitSound();
                }
            
            if (spriteTwo is Barrier barrier && IsTouchingLeft(barrier))
                {
                    // Reverse the ball's direction or apply other logic as needed
                    Velocity *= new Vector2(-1, 1);
                }
            }

            if (Position.Y <= 0 || Position.Y + _texture.Height >= Game1._graphics.PreferredBackBufferHeight)
                Velocity.Y = -Velocity.Y;

            if (Position.X <= 0)
            {
                Score.Score2++;
                Restart();
            }

            if (Position.X + _texture.Width >= Game1._graphics.PreferredBackBufferWidth)
            {
                Score.Score1++;
                Restart();
                if (Score.Score1 == 2)
                {
                    CurrentGameState = GameState.GameOver;
                }
            }

            Position += Velocity * Speed;
        }


        // Method to restart the ball position
        public void Restart()
        {
            var direction = PlayTwoScene.Random.Next(0, 4);

            switch (direction)
            {
                case 0:
                    Velocity = new Vector2(1, 1);
                    break;
                case 1:
                    Velocity = new Vector2(1, -1);
                    break;
                case 2:
                    Velocity = new Vector2(-1, -1);
                    break;
                case 3:
                    Velocity = new Vector2(-1, 1);
                    break;
            }

            Position = (Vector2)_startPosition;
            Speed = (float)_startSpeed;
            _timer = 0;
            _isPlaying = false;
        }
        private void PlayHitSound()
        {
            // Check if the sound effect is not null before playing
            if (_isPlaying) // Check if the ball is currently in play
            {
                dingSound?.Play();
            }
        }
    }
}
