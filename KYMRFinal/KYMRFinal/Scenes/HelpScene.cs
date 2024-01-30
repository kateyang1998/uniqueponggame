using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KYMRFinal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KYMRFinal.Scenes
{
    // Game instruction scene
    internal class HelpScene : Component
    {
        // Help image
        private Texture2D help;

        internal override void LoadContent(ContentManager Content)
        {
            help = Content.Load<Texture2D>("helpscene");
        }

        internal override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Home))
            {
                Data.CurrentState = Data.Scenes.Start; // Assuming 'Start' is the enum value for StartScene
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            // Display the title image with specific size
            float scale = 0.3f;
            spriteBatch.Draw(help, new Vector2(75, 45), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
