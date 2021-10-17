using System.Drawing;

namespace SpaceInvaders
{
    public static class GameConfig
    {
        #region Window
        public static int WindowWidth { get; set; }

        public static int WindowHeight { get; set; }

        public static readonly Font GameFont = new Font("BankGothic Md BT", 24);

        public static readonly Font GameFontLarge = new Font("BankGothic Md BT", 42);

        public static readonly Brush GameBrush = new SolidBrush(Color.White);
        #endregion Window

        #region Speed
        public const int GameSpeed = 15; //15

        public const int PlayerMoveSpeed = 5;

        public const int PlayerShotSpeed = 10;//10

        public const int AlienShotSpeed = 6;

        public const int AlienHorizontalMoveSpeed = 15;

        public const int AlienVerticalMoveSpeed = 42;

        public const int AlienMoveIntervalInitial = 32;//32

        public const int AlienMoveIntervalDecrease = 4;

        public const int AlienMoveIntervalMin = 16;

        public const int FlyingSaucerMoveSpeed = 3;
        #endregion Speed

        #region Enemies
        public const int EnemyRows = 5;//5

        public const int EnemyColumns = 11;//11

        public const int FreeWidth = 3;

        public const int VerticalGap = 5;

        public const int AlienShotCooldown = 30;

        public const int FlyingSaucerSpawnInterval = 800;

        public const int FlyingSaucerExplosionFrames = 15;

        public static int AlienMinY { get; set; }

        public static readonly int[] AlienRowScoreValue = { 30, 20, 20, 10, 10 };

        public const int FlyingSaucerScore = 100;
        #endregion Enemies

        #region Player
        public const int InitialLives = 3;//3

        public const int PlayerShotCooldown = 20;//20

        public const int FreePeriodAfterGameOver = 150;//150
        #endregion Player

        #region Blocks
        public static readonly string[] BlockStructure
            = { "####",
                "####",
                "#  #" };

        public const int StructureCount = 4;
        #endregion Blocks
    }
}