using KYMRFinal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KYMRFinal.Level1
{
    public class SpriteOne
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

        public SpriteOne(Texture2D texture) // Declaring a public constructor for the SpriteOne class
        {
            _texture = texture; // Assigning the texture parameter to the _texture variable
        }

        public virtual void Update(GameTime gameTime, List<SpriteOne> spritesOne) // Declaring a public virtual method named Update
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch) // Declaring a public virtual method named Draw
        {
            spriteBatch.Draw(_texture, Position, Color.White); // Drawing the sprite
        }

        #region Colloision // Starting a region named Colloision
        protected bool IsTouchingLeft(SpriteOne spriteOne) // Declaring a protected method named IsTouchingLeft
        {
            return this.Rectangle.Right + this.Velocity.X > spriteOne.Rectangle.Left &&
              this.Rectangle.Left < spriteOne.Rectangle.Left &&
              this.Rectangle.Bottom > spriteOne.Rectangle.Top &&
              this.Rectangle.Top < spriteOne.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(SpriteOne spriteOne) // Declaring a protected method named IsTouchingRight
        {
            return this.Rectangle.Left + this.Velocity.X < spriteOne.Rectangle.Right &&
              this.Rectangle.Right > spriteOne.Rectangle.Right &&
              this.Rectangle.Bottom > spriteOne.Rectangle.Top &&
              this.Rectangle.Top < spriteOne.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(SpriteOne spriteOne) // Declaring a protected method named IsTouchingTop
        {
            return this.Rectangle.Bottom + this.Velocity.Y > spriteOne.Rectangle.Top &&
              this.Rectangle.Top < spriteOne.Rectangle.Top &&
              this.Rectangle.Right > spriteOne.Rectangle.Left &&
              this.Rectangle.Left < spriteOne.Rectangle.Right;
        }

        protected bool IsTouchingBottom(SpriteOne spriteOne) // Declaring a protected method named IsTouchingBottom
        {
            return this.Rectangle.Top + this.Velocity.Y < spriteOne.Rectangle.Bottom &&
              this.Rectangle.Bottom > spriteOne.Rectangle.Bottom &&
              this.Rectangle.Right > spriteOne.Rectangle.Left &&
              this.Rectangle.Left < spriteOne.Rectangle.Right;
        }
        #endregion // Ending the region named Colloision
    }
}
