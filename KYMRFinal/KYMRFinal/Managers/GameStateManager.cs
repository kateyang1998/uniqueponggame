using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KYMRFinal.Core;
using KYMRFinal.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KYMRFinal.Managers
{
    internal partial class GameStateManager : Component
    {
        private StartScene startScene = new StartScene();
        private PlayScene playScene = new PlayScene();
        private LevelOptionScene levelOptionScene = new LevelOptionScene();
        private HighScoreScene highScoreScene = new HighScoreScene();
        private HelpScene helpScene = new HelpScene();
        private AboutScene aboutScene = new AboutScene();
        private PlayTwoScene playTwoScene = new PlayTwoScene();

        internal override void LoadContent(ContentManager Content)
        {
            //throw new NotImplementedException();
            startScene.LoadContent(Content);
            playScene.LoadContent(Content);
            levelOptionScene.LoadContent(Content);
            highScoreScene.LoadContent(Content);
            helpScene.LoadContent(Content);
            aboutScene.LoadContent(Content);
            playTwoScene.LoadContent(Content);
        }

        internal override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
            switch(Data.CurrentState) 
            {
                case Data.Scenes.Start:
                    startScene.Update(gameTime);
                    break;
                case Data.Scenes.Play: 
                    playScene.Update(gameTime);
                    break;
                case Data.Scenes.LevelOption:
                    levelOptionScene.Update(gameTime); 
                    break;
                case Data.Scenes.HighScore:
                    highScoreScene.Update(gameTime);
                    break;
                case Data.Scenes.Help: 
                    helpScene.Update(gameTime);
                    break;
                case Data.Scenes.About: 
                    aboutScene.Update(gameTime);
                    break;
                case Data.Scenes.PlayTwo:
                    playTwoScene.Update(gameTime);
                    break;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
            switch (Data.CurrentState)
            {
                case Data.Scenes.Start:
                    startScene.Draw(spriteBatch);
                    break;
                case Data.Scenes.Play:
                    playScene.Draw(spriteBatch);
                    break;
                case Data.Scenes.LevelOption:
                    levelOptionScene.Draw(spriteBatch);
                    break;
                case Data.Scenes.HighScore:
                    highScoreScene.Draw(spriteBatch);
                    break;
                case Data.Scenes.Help:
                    helpScene.Draw(spriteBatch);
                    break;
                case Data.Scenes.About:
                    aboutScene.Draw(spriteBatch);
                    break;
                case Data.Scenes.PlayTwo:
                    playTwoScene.Draw(spriteBatch);
                    break;
            }
        }
    }
}
