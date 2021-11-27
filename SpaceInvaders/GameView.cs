using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class GameView : Form
    {
        private GameController GameController { get; set; }

        public GameView()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            GameConfig.Instance.WindowWidth = this.pbMain.Width;
            GameConfig.Instance.WindowHeight = this.pbMain.Height;

            timer.Interval = GameConfig.Instance.GameSpeed;

            GameController = new GameController();

            GameController.Initialize();
        }

        private void ShowStart()
        {
            pbMain.Invalidate();

            this.KeyDown += StartScreen_KeyDown;
        }

        private void ShowGame()
        {
            pbMain.Paint -= StartScreen_Paint;
            pbMain.Paint += pbMain_Paint;

            this.KeyDown -= StartScreen_KeyDown;
            this.KeyDown -= EndScreen_KeyDown;
            this.KeyDown += GameView_KeyDown;
            this.KeyUp += GameView_KeyUp;

            timer.Start();
        }

        private void ShowEnd()
        {
            pbMain.Invalidate();
            this.KeyDown -= GameView_KeyDown;
            this.KeyUp -= GameView_KeyUp;
            this.KeyDown += EndScreen_KeyDown;
        }

        private void GameOver()
        {
            timer.Stop();
            if (GameController.PlayerWon)
            {
                int lives = GameController.PlayerShip.Lives;
                int score = GameController.Score;
                int alienMoveInterval = GameController.AlienMoveInterval;
                if (alienMoveInterval > GameConfig.Instance.AlienMoveIntervalMin)
                {
                    alienMoveInterval -= GameConfig.Instance.AlienMoveIntervalDecrease;
                }
                GameController = new GameController();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GameController.Initialize();
                GameController.PlayerShip.Lives = lives;
                GameController.Score = score;
                GameController.AlienMoveInterval = alienMoveInterval;
                timer.Start();
            }
            else
            {
                ShowEnd();
            }
        }

        private void GameView_Shown(object sender, EventArgs e)
        {
            ShowStart();
        }

        private void StartScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowGame();
            }
        }

        private void EndScreen_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    GameController.GameCounter = 0;
                    GameController = new GameController();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GameController.Initialize();
                    ShowGame();
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
                default:
                    break;
            }
        }

        private void GameView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    GameController.ControlIsActive[PlayerControls.Left] = true;
                    break;
                case Keys.Right:
                    GameController.ControlIsActive[PlayerControls.Right] = true;
                    break;
                case Keys.Space:
                    GameController.ControlIsActive[PlayerControls.Shoot] = true;
                    break;
                default:
                    break;
            }
        }

        private void GameView_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    GameController.ControlIsActive[PlayerControls.Left] = false;
                    break;
                case Keys.Right:
                    GameController.ControlIsActive[PlayerControls.Right] = false;
                    break;
                case Keys.Space:
                    GameController.ControlIsActive[PlayerControls.Shoot] = false;
                    break;
                default:
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            GameController.UpdateFrame();

            pbMain.Invalidate();

            if (GameController.GameOver)
            {
                GameOver();
            }
        }

        private void StartScreen_Paint(object sender, PaintEventArgs e)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            Rectangle rectangle = new Rectangle(0, pbMain.Height/6, this.Width, this.Height);
            e.Graphics.DrawString("SPACE\nINVADERS", GameConfig.Instance.GameFontLarge, GameConfig.Instance.GameBrush, rectangle, sf);
            rectangle.Y = pbMain.Height - pbMain.Height / 6;
            e.Graphics.DrawString("Press Enter", GameConfig.Instance.GameFont, GameConfig.Instance.GameBrush, rectangle, sf);
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            DrawGameObjects(e);

            DrawInfo(e);

            if(GameController.PlayerWon)
            {
                DrawVictoryMessage(e);
            }

            if (GameController.GameOver && !GameController.PlayerWon)
            {
                DrawGameOver(e);
            }
        }

        private void DrawVictoryMessage(PaintEventArgs e)
        {
            int x = pbMain.Width / 4;
            int y = (int)(pbMain.Height / 2.5);
            int width = pbMain.Width - x * 2;
            int height = pbMain.Height - y * 2;
            int margin = 6;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Rectangle rectangle = new Rectangle(x + margin, y + margin, width - margin * 2, height - margin * 2);
            e.Graphics.FillRectangle(new SolidBrush(Color.Black), x, y, width, height);
            e.Graphics.DrawRectangle(new Pen(Color.White, 3), rectangle);
            e.Graphics.DrawString("Wave complete!\nOnto the next wave", GameConfig.Instance.GameFont, GameConfig.Instance.GameBrush, rectangle, sf);
        }

        private void DrawGameObjects(PaintEventArgs e)
        {
            if (GameController.PlayerShip.Alive || GameController.PlayerShip.Exploding)
            {
                e.Graphics.DrawImage(GameController.PlayerShip.Sprite, GameController.PlayerShip.X, GameController.PlayerShip.Y);
            }            

            if (GameController.PlayerShot.Active)
            {
                e.Graphics.DrawImage(GameController.PlayerShot.Sprite, GameController.PlayerShot.X, GameController.PlayerShot.Y);
            }

            if (GameController.AlienShot.Active)
            {
                e.Graphics.DrawImage(GameController.AlienShot.Sprite, GameController.AlienShot.X, GameController.AlienShot.Y);
            }

            for (int i = 0; i < GameController.Aliens.Count; i++)
            {
                for (int j = 0; j < GameController.Aliens[i].Count; j++)
                {
                    if (GameController.Aliens[i][j].Alive || GameController.Aliens[i][j].Exploding)
                    {
                        e.Graphics.DrawImage(GameController.Aliens[i][j].Sprite, GameController.Aliens[i][j].X, GameController.Aliens[i][j].Y);
                    }
                }
            }

            for (int i = 0; i < GameController.Blocks.Count; i++)
            {
                if (GameController.Blocks[i].Active)
                {
                    e.Graphics.DrawImage(GameController.Blocks[i].Sprite, GameController.Blocks[i].X, GameController.Blocks[i].Y);
                }
            }

            if (GameController.FlyingSaucer.Active || GameController.FlyingSaucer.Exploding)
            {
                e.Graphics.DrawImage(GameController.FlyingSaucer.Sprite, GameController.FlyingSaucer.X, GameController.FlyingSaucer.Y);
            }
        }

        private void DrawInfo(PaintEventArgs e)
        {
            e.Graphics.DrawString($"Score: {GameController.Score}<{GameController.GameCounter - 1}>", GameConfig.Instance.GameFont, GameConfig.Instance.GameBrush, 30, 20);

            int x = 30;
            int y = pbMain.Height - Sprites.Instance.PlayerHealth[0].Height - 20;
            for (int i = 0; i < GameController.PlayerShip.Lives; i++)
            {
                e.Graphics.DrawImage(Sprites.Instance.PlayerHealth[0], x, y);
                x += Sprites.Instance.PlayerHealth[0].Width + 15;
            }
        }

        private void DrawGameOver(PaintEventArgs e)
        {
            int x = pbMain.Width / 4;
            int y = (int)(pbMain.Height / 2.5);
            int width = pbMain.Width - x * 2;
            int height = pbMain.Height - y * 2;
            int margin = 6;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Rectangle rectangle = new Rectangle(x + margin, y + margin, width - margin * 2, height - margin * 2);
            e.Graphics.FillRectangle(new SolidBrush(Color.Black), x, y, width, height);
            e.Graphics.DrawRectangle(new Pen(Color.White, 3), rectangle);
            e.Graphics.DrawString("Game Over\nPress Enter to restart\nor Esc to exit", GameConfig.Instance.GameFont, GameConfig.Instance.GameBrush, rectangle, sf);
        }
    }
}
