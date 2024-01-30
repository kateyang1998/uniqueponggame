using KYMRFinal.Level1;
using KYMRFinal.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace KYMRFinal.Core
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameStateManager _gameStateManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Initialize screen width and height
            _graphics.PreferredBackBufferWidth = Data.ScreenW;
            _graphics.PreferredBackBufferHeight = Data.ScreenH;
            _graphics.ApplyChanges();
            _gameStateManager = new GameStateManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameStateManager.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            _gameStateManager.Update(gameTime);

            if (Data.Exit)
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            _spriteBatch.Begin();
            _gameStateManager.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}