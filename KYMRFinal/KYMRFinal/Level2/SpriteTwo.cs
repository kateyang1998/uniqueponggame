using KYMRFinal.Core;
using KYMRFinal.Level1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KYMRFinal.Level2
{
    public class SpriteTwo
    {
        public Texture2D _texture;

        public Vector2 Position;
        public Vector2 Velocity;
        public float Speed;
        public Input Input;

        public Rectangle Rectangle // Declaring a public Rectangle property named Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height); // Returns a new Rectangle object
            }
        }

        public SpriteTwo(Texture2D texture) // Declaring a public constructor for the SpriteOne class
        {
            _texture = texture; // Assigning the texture parameter to the _texture variable
        }

        public virtual void Update(GameTime gameTime, List<SpriteTwo> spritesTwo) // Declaring a public virtual method named Update
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch) // Declaring a public virtual method named Draw
        {
            spriteBatch.Draw(_texture, Position, Color.White); // Drawing the sprite
        }

        #region Colloision // Starting a region named Colloision
        protected bool IsTouchingLeft(SpriteTwo spriteTwo) // Declaring a protected method named IsTouchingLeft
        {
            return this.Rectangle.Right + this.Velocity.X > spriteTwo.Rectangle.Left &&
              this.Rectangle.Left < spriteTwo.Rectangle.Left &&
              this.Rectangle.Bottom > spriteTwo.Rectangle.Top &&
              this.Rectangle.Top < spriteTwo.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(SpriteTwo spriteTwo) // Declaring a protected method named IsTouchingRight
        {
            return this.Rectangle.Left + this.Velocity.X < spriteTwo.Rectangle.Right &&
              this.Rectangle.Right > spriteTwo.Rectangle.Right &&
              this.Rectangle.Bottom > spriteTwo.Rectangle.Top &&
              this.Rectangle.Top < spriteTwo.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(SpriteTwo spriteTwo) // Declaring a protected method named IsTouchingTop
        {
            return this.Rectangle.Bottom + this.Velocity.Y > spriteTwo.Rectangle.Top &&
              this.Rectangle.Top < spriteTwo.Rectangle.Top &&
              this.Rectangle.Right > spriteTwo.Rectangle.Left &&
              this.Rectangle.Left < spriteTwo.Rectangle.Right;
        }

        protected bool IsTouchingBottom(SpriteTwo spriteTwo) // Declaring a protected method named IsTouchingBottom
        {
            return this.Rectangle.Top + this.Velocity.Y < spriteTwo.Rectangle.Bottom &&
              this.Rectangle.Bottom > spriteTwo.Rectangle.Bottom &&
              this.Rectangle.Right > spriteTwo.Rectangle.Left &&
              this.Rectangle.Left < spriteTwo.Rectangle.Right;
        }
        #endregion // Ending the region named Colloision
    }
}
