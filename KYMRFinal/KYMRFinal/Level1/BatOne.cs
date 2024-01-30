using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KYMRFinal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;

namespace KYMRFinal.Level1
{
    public class BatOne : SpriteOne
    {
        public BatOne(Texture2D texture) : base(texture)
        {
            Speed = 5f;
        }

        // Add a height property
        public int Height => _texture.Height;
        public override void Update(GameTime gameTime, List<SpriteOne> spritesOne)
        {
            if (Input == null)
                throw new Exception("Please give a value to 'Input'");

            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Velocity.Y = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
                Velocity.Y = Speed;

            Position += Velocity;

            Position.Y = MathHelper.Clamp(Position.Y, 0, Game1._graphics.PreferredBackBufferHeight - _texture.Height);

            Velocity = Vector2.Zero;
        }
    }
}

