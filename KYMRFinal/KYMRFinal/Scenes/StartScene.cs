using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using KYMRFinal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KYMRFinal.Scenes
{
    // Main menu scene
    internal class StartScene : Component
    {
        // Buttons
        private const int maxBtns = 6;
        private Texture2D[] btns = new Texture2D[maxBtns];
        private Rectangle[] btnRectangles = new Rectangle[maxBtns];

        private Texture2D mainTitle;

        private MouseState mouseState, oldMouseState;
        private Rectangle mouseStateRectangle;

        internal override void LoadContent(ContentManager Content)
        {
            Song backgroundMusic = Content.Load<Song>("Sounds/bgm1");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);

            mainTitle = Content.Load<Texture2D>("maintitle");

            const int incrementValue = 100;
            for (int i = 0; i < btns.Length; i++)
            {
                btns[i] = Content.Load<Texture2D>($"Btn{i}");
                btnRectangles[i] = new Rectangle(730, 50 + (incrementValue * i), btns[i].Width/3, btns[i].Height/3);
            }
        }

        internal override void Update(GameTime gameTime) 
        {
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();
            mouseStateRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

            // Enable mouse and button action
            if (mouseState.LeftButton == ButtonState.Pressed && mouseStateRectangle.Intersects(btnRectangles[0])) 
            {
                Data.CurrentState = Data.Scenes.Play;
            }
            if (mouseState.LeftButton == ButtonState.Pressed && mouseStateRectangle.Intersects(btnRectangles[1]))
            {
                Data.CurrentState = Data.Scenes.LevelOption;
            }
            if (mouseState.LeftButton == ButtonState.Pressed && mouseStateRectangle.Intersects(btnRectangles[2]))
            {
                Data.CurrentState = Data.Scenes.HighScore;
            }
            if (mouseState.LeftButton == ButtonState.Pressed && mouseStateRectangle.Intersects(btnRectangles[3]))
            {
                Data.CurrentState = Data.Scenes.Help;
            }
            if (mouseState.LeftButton == ButtonState.Pressed && mouseStateRectangle.Intersects(btnRectangles[4]))
            {
                Data.CurrentState = Data.Scenes.About;
            }
            if (mouseState.LeftButton == ButtonState.Pressed && mouseStateRectangle.Intersects(btnRectangles[5]))
            {
                Data.Exit = true;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            // Display the title image with specific size
            float scale = 0.9f;
            spriteBatch.Draw(mainTitle, new Vector2(120, 50), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            // Display buttons
            for (int i = 0; i < btns.Length; i++) 
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
