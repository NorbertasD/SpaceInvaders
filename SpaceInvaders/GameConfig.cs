using System.Drawing;

namespace SpaceInvaders
{
    public class GameConfig
    {
        private GameConfig() { }

        private static GameConfig instance;

        public static GameConfig Instance
        {
            get 
            {
                if(instance == null)
                {
                    instance = new GameConfig();
                }
                return instance;
            }
        }


        #region Window
        public int WindowWidth { get; set; }

        public int WindowHeight { get; set; }

        public Font GameFont { get; set; } = new Font("BankGothic Md BT", 24);

        public Font GameFontLarge { get; set; } = new Font("BankGothic Md BT", 42);

        public Brush GameBrush { get; set; } = new SolidBrush(Color.White);
        #endregion Window

        #region Speed
        public int GameSpeed { get; set; } = 15; //15

        public int PlayerMoveSpeed { get; set; } = 5;

        public int PlayerShotSpeed { get; set; } = 10; //10

        public int AlienShotSpeed { get; set; } = 6; //6

        public int AlienHorizontalMoveSpeed { get; set; } = 15;

        public int AlienVerticalMoveSpeed { get; set; } = 42;

        public int AlienMoveIntervalInitial { get; set; } = 32; //32

        public int AlienMoveIntervalDecrease { get; set; } = 4;

        public int AlienMoveIntervalMin { get; set; } = 16;

        public int FlyingSaucerMoveSpeed { get; set; } = 3;
        #endregion Speed

        #region Enemies
        public int EnemyRows { get; set; } = 5; //5

        public int EnemyColumns { get; set; } = 11; //11

        public int FreeWidth { get; set; } = 3;

        public int VerticalGap { get; set; } = 5;

        public int AlienShotCooldown { get; set; } = 30; //30

        public int FlyingSaucerSpawnInterval { get; set; } = 800;

        public int FlyingSaucerExplosionFrames { get; set; } = 15;

        public int AlienMinY { get; set; }

        public int[] AlienRowScoreValue { get; set; } = { 30, 20, 20, 10, 10 };

        public int FlyingSaucerScore { get; set; } = 100;
        #endregion Enemies

        #region Player
        public int InitialLives { get; set; } = 3; //3

        public int PlayerShotCooldown { get; set; } = 20; //20

        public int FreePeriodAfterGameOver { get; set; } = 150; //150
        #endregion Player

        #region Blocks
        public string[] BlockStructure { get; set; }
            = { "####",
                "####",
                "#  #" };

        public int StructureCount { get; set; } = 4;
        #endregion Blocks
    }
}