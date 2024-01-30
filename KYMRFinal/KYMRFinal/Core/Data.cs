using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KYMRFinal.Core
{
    // Set data for project
    public static class Data
    {
        public static int ScreenW { get; set; } = 1200;
        public static int ScreenH { get; set; } = 675;
        public static bool Exit { get; set; } = false;
        public enum Scenes { Start, Play, LevelOption, HighScore, Help, About, PlayTwo }
        public enum GameState { Playing, GameOver }
        public static Scenes CurrentState { get; set; } = Scenes.Start;

        public static GameState CurrentGameState = GameState.Playing;
    }
 
}
