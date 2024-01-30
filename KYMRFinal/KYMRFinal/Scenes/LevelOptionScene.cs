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
using Microsoft.Xna.Framework.Media;
using static System.Formats.Asn1.AsnWriter;

namespace KYMRFinal.Scenes
{
    // Mode menu scene
    internal class LevelOptionScene : Component
    {
        // Buttons and title image
        private const int maxBtns = 8;
        private Texture2D[] btns = new Texture2D[maxBtns];
        private Rectangle[] btnRectangles = new Rectangle[maxBtns];
        private Texture2D title;

        private MouseState mouseState, oldMouseState;
        private Rectangle mouseStateRectangle;

        internal override void LoadContent(ContentManager Content)
        {
            title = Content.Load<Texture2D>("choosetitle");

            const int incrementValue = 100;
            for (int i = 0; i < btns.Length; i++)
            {
                btns[i] = Content.Load<Texture2D>($"Btn{i}");
                btnRectangles[i] = new Rectangle(490, -280 + (incrementValue * i), btns[i].Width / 3, btns[i].Height / 3);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();
            mouseStateRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

            // Enable mouse and key and button action
            if (mouseState.LeftButton == ButtonState.Pressed && mouseStateRectangle.Intersects(btnRectangles[6]))
            {
                Data.CurrentState = Data.Scenes.Play;
            }
            if (mouseState.LeftButton == ButtonState.Pressed && mouseStateRectangle.Intersects(btnRectangles[7]))
            {
                Data.CurrentState = Data.Scenes.PlayTwo;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Home))
            {
                Data.CurrentState = Data.Scenes.Start; // Assuming 'Start' is the enum value for StartScene
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            // Display the title image with specific size
            float scale = 0.4f;
            spriteBatch.Draw(title, new Vector2(120, 120), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            // Display the specific buttons
            int[] visibleButtonIndices = { 6, 7 };

            foreach (int i in visibleButtonIndices)
            {
                spriteBatch.Draw(btns[i], btnRectangles[i], Color.White);

                if (mouseStateRectangle.Intersects(btnRectangles[i]))
                {
                    spriteBatch.Draw(btns[i], btnRectangles[i], Color.LightGray);
                }
            }
        }
    }
}
