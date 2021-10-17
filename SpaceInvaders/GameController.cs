using SpaceInvaders.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceInvaders
{
    public class GameController
    {
        //public Sprites Sprites { get; set; }

        public PlayerShip PlayerShip { get; set; }

        public Shot PlayerShot { get; set; }

        public Shot AlienShot { get; set; }

        public List<List<Alien>> Aliens { get; set; }

        public List<Block> Blocks { get; set; }

        public FlyingSaucer FlyingSaucer { get; set; }

        public Dictionary<PlayerControls, bool> ControlIsActive { get; set; }

        private Random Random { get; set; }

        public int Score { get; set; } = 0;

        public bool GameOver { get; set; } = false;

        public bool PlayerWon { get; set; } = false;

        public static int GameCounter { get; set; } = 0;

        public int AlienMoveInterval { get; set; }

        private int AlienMovementFrameCounter { get; set; } = 0;

        private int FlyingSaucerSpawnFrameCounter { get; set; } = 0;

        private bool AliensMovingRight { get; set; } = true;

        private bool ReachedEndOfScreen { get; set; } = false;

        private int TimerBeforeGameOver { get; set; } = 0;

        public GameController()
        {
            GameCounter++;
        }

        public void Initialize()
        {
            InitializeControls();

            Random = new Random();

            AlienMoveInterval = GameConfig.AlienMoveIntervalInitial;

            PlayerShip = new PlayerShip(Sprites.Instance.PlayerShip, GameConfig.InitialLives, GameConfig.PlayerMoveSpeed, 0, GameConfig.WindowWidth);
            PlayerShip.X = (GameConfig.WindowWidth / 2) - (PlayerShip.Width / 2);
            PlayerShip.Y = (GameConfig.WindowHeight - GameConfig.WindowHeight / 8);

            GameConfig.AlienMinY = PlayerShip.Y;

            PlayerShot = new Shot(Sprites.Instance.PlayerShot, -GameConfig.PlayerShotSpeed, 0, GameConfig.PlayerShotCooldown);

            AlienShot = new Shot(Sprites.Instance.AlienShot, GameConfig.AlienShotSpeed, GameConfig.WindowHeight, GameConfig.AlienShotCooldown);

            Aliens = new List<List<Alien>>();

            int witdhtForEnemy = (GameConfig.WindowWidth - (GameConfig.WindowWidth / GameConfig.FreeWidth)) / GameConfig.EnemyColumns;

            int alienY = GameConfig.WindowHeight / 8;

            for (int i = 0; i < GameConfig.EnemyRows; i++)
            {
                Aliens.Add(new List<Alien>());
                for (int j = 0; j < GameConfig.EnemyColumns; j++)
                {
                    Aliens[i].Add(new Alien(Sprites.Instance.Aliens[i]));
                    Aliens[i][j].X = j * witdhtForEnemy + (witdhtForEnemy - Aliens[i][j].Width) / 2;
                    Aliens[i][j].Y = alienY;
                }
                alienY += Aliens[i][0].Height + GameConfig.VerticalGap;
            }

            FlyingSaucer = new FlyingSaucer(Sprites.Instance.FlyingSaucer, GameConfig.FlyingSaucerMoveSpeed, 0);

            Blocks = new List<Block>();

            int longestBlockRow = 0;

            for (int i = 0; i < GameConfig.BlockStructure.Length; i++)
            {
                if (GameConfig.BlockStructure[i].Length > longestBlockRow)
                {
                    longestBlockRow = GameConfig.BlockStructure[i].Length;
                }
            }

            int structureLenght = longestBlockRow * Sprites.Instance.Block[0].Width;

            int gap = (GameConfig.WindowWidth - (structureLenght * GameConfig.StructureCount)) / (GameConfig.StructureCount + 1);

            int structureX = gap;

            for (int i = 0; i < GameConfig.StructureCount; i++)
            {
                CreateStructure(structureX);
                structureX += structureLenght + gap;
            }
        }

        public void UpdateFrame()
        {
            if (AlienShot.Active)
            {
                AlienShot.Move();
                CheckIfBlockIsHit(AlienShot);
                CheckIfPlayerIsHit();
            }
            else if (AlienShot.Cooldown == 0 && Aliens.Count > 0 && Aliens[0].Count > 0)
            {
                AliensShoot();
            }

            if (PlayerShot.Active)
            {
                PlayerShot.Move();
                CheckIfBlockIsHit(PlayerShot);
                CheckIfAlienIsHit();
                CheckIfFlyingSaucerIsHit();
            }

            HandleShotCollision();

            HandleFlyingSaucer();

            if (Aliens.Count > 0 && Aliens[0].Count > 0)
            {
                HandleAlienMovement();
                HandleBlockAlienCollision();
            }

            if (PlayerShip.Alive)
            {
                HandlPlayerControls();
            }

            if (PlayerShot.Cooldown > 0)
            {
                PlayerShot.Cooldown--;
            }

            if (AlienShot.Cooldown > 0)
            {
                AlienShot.Cooldown--;
            }

            AlienMovementFrameCounter++;

            if (PlayerShip.Exploding)
            {
                PlayerShip.AdvanceExplosion();
                if (!PlayerShip.Exploding)
                {
                    GameOver = true;
                }
            }

            if (TimerBeforeGameOver != 0)
            {
                if (TimerBeforeGameOver == GameConfig.FreePeriodAfterGameOver)
                {
                    GameOver = true;
                }
                TimerBeforeGameOver++;
            }
        }

        private void HandleBlockAlienCollision()
        {
            int lowestAlienY = Aliens[Aliens.Count - 1][0].Y + Aliens[Aliens.Count - 1][0].Height;

            for (int i = 0; i < Blocks.Count; i++)
            {
                if (Blocks[i].Y < lowestAlienY)
                {
                    Blocks.RemoveAt(i);
                    i--;
                }
            }
        }

        private void HandlPlayerControls()
        {
            if (ControlIsActive[PlayerControls.Left])
            {
                PlayerShip.MoveLeft();
            }
            if (ControlIsActive[PlayerControls.Right])
            {
                PlayerShip.MoveRight();
            }
            if (ControlIsActive[PlayerControls.Shoot])
            {
                if (!PlayerShot.Active && PlayerShot.Cooldown == 0)
                {
                    PlayerShip.Shoot(PlayerShot);
                }
            }
        }

        private void HandleAlienMovement()
        {
            if (AlienMovementFrameCounter == AlienMoveInterval)
            {
                if (!ReachedEndOfScreen)
                {
                    if (AliensMovingRight)
                    {
                        AliensMoveRight();
                    }
                    else
                    {
                        AliensMoveLeft();
                    }
                }

                AlienMovementFrameCounter = 0;
            }

            if (ReachedEndOfScreen)
            {
                AliensMovingRight = !AliensMovingRight;
                ReachedEndOfScreen = false;
                AliensMoveDown();
            }
        }

        private void CheckIfFlyingSaucerIsHit()
        {
            if (FlyingSaucer.Active && CheckCollision(FlyingSaucer, PlayerShot))
            {
                PlayerShot.Active = false;
                FlyingSaucer.Active = false;
                FlyingSaucer.Exploding = true;
                FlyingSaucer.AnimationState = 1;
                if(PlayerShip.Lives <= GameConfig.InitialLives)
                {
                    PlayerShip.Lives++;
                }
                else
                {
                    Score += GameConfig.FlyingSaucerScore;
                }                
            }
        }

        private void HandleFlyingSaucer()
        {
            if (FlyingSaucer.Active)
            {
                FlyingSaucer.MoveLeft();
            }
            else
            {
                FlyingSaucerSpawnFrameCounter++;
            }

            if (FlyingSaucer.Exploding)
            {
                FlyingSaucer.ExplosionFrameCounter++;
                if (FlyingSaucer.ExplosionFrameCounter == GameConfig.FlyingSaucerExplosionFrames)
                {
                    FlyingSaucer.Exploding = false;
                    FlyingSaucer.ExplosionFrameCounter = 0;
                }
            }

            if (FlyingSaucerSpawnFrameCounter == GameConfig.FlyingSaucerSpawnInterval)
            {
                FlyingSaucerSpawnFrameCounter = 0;
                FlyingSaucer.AnimationState = 0;
                FlyingSaucer.X = GameConfig.WindowWidth - FlyingSaucer.Width;
                FlyingSaucer.Y = GameConfig.WindowHeight / 12;
                FlyingSaucer.Active = true;
            }
        }

        private void CreateStructure(int structureX)
        {
            int blockWidth = Sprites.Instance.Block[0].Width;
            int blockHeight = Sprites.Instance.Block[0].Height;

            int blockY = (GameConfig.WindowHeight - GameConfig.WindowHeight / 4);

            for (int i = 0; i < GameConfig.BlockStructure.Length; i++)
            {
                for (int j = 0; j < GameConfig.BlockStructure[i].Length; j++)
                {
                    if (GameConfig.BlockStructure[i][j] == '#')
                    {
                        Block block = new Block(Sprites.Instance.Block, Sprites.Instance.Block.Length);
                        block.X = structureX + j * blockWidth;
                        block.Y = blockY;
                        Blocks.Add(block);
                    }
                }
                blockY += blockHeight;
            }
        }

        private void CheckIfBlockIsHit(Shot shot)
        {
            for (int i = 0; i < Blocks.Count; i++)
            {
                if (CheckCollision(Blocks[i], shot))
                {
                    shot.Active = false;

                    Blocks[i].DamageLevel++;

                    if (!Blocks[i].Active)
                    {
                        Blocks.RemoveAt(i);
                    }

                    return;
                }
            }
        }

        private void HandleShotCollision()
        {
            if (PlayerShot.Active && AlienShot.Active && CheckCollision(AlienShot, PlayerShot))
            {
                AlienShot.Active = false;
                PlayerShot.Active = false;
            }
        }

        private void CheckIfPlayerIsHit()
        {
            if (CheckCollision(PlayerShip, AlienShot))
            {
                AlienShot.Active = false;
                PlayerShip.Lives--;
                if (PlayerShip.Lives == 0)
                {
                    PlayerWon = false;
                    PlayerShip.Exploding = true;
                    PlayerShip.AnimationState = 1;
                }
            }
        }

        private bool CheckCollision(EntityBase hitEntity, EntityBase hittingEntity)
        {
            if ((hitEntity.Y <= hittingEntity.Y + hittingEntity.Height && hitEntity.Y + hitEntity.Height >= hittingEntity.Y + hittingEntity.Height)
                || (hitEntity.Y <= hittingEntity.Y && hitEntity.Y + hitEntity.Height >= hittingEntity.Y))
            {
                if ((hitEntity.X <= hittingEntity.X + hittingEntity.Width || hitEntity.X <= hittingEntity.X)
                && (hitEntity.X + hitEntity.Width >= hittingEntity.X || hitEntity.X + hitEntity.Width >= hittingEntity.X + hittingEntity.Width))
                {
                    return true;
                }
            }

            return false;
        }

        private void AliensShoot()
        {
            int x;
            int y;

            for (int i = 0; i < 10000; i++)
            {
                y = Random.Next(Aliens.Count);
                x = Random.Next(Aliens[y].Count);

                if (Aliens[y][x].Alive)
                {
                    Aliens[y][x].Shoot(AlienShot);
                    break;
                }
            }
        }

        private void AliensMoveDown()
        {
            for (int i = 0; i < Aliens.Count; i++)
            {
                for (int j = 0; j < Aliens[i].Count; j++)
                {
                    if (Aliens[i][j].Alive)
                    {
                        Aliens[i][j].Y += GameConfig.AlienVerticalMoveSpeed;
                    }
                }
            }
            if (Aliens[Aliens.Count - 1][0].Y + Aliens[Aliens.Count - 1][0].Height + GameConfig.AlienVerticalMoveSpeed >= GameConfig.AlienMinY)
            {
                GameOver = true;
            }
        }

        private void CheckIfAlienIsHit()
        {
            for (int i = Aliens.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < Aliens[i].Count; j++)
                {
                    if (PlayerShot.Active && Aliens[i][j].Alive && CheckCollision(Aliens[i][j], PlayerShot))
                    {
                        Aliens[i][j].Alive = false;
                        PlayerShot.Active = false;
                        Aliens[i][j].Exploding = true;
                        Aliens[i][j].AnimationState = 2;
                        Score += GameConfig.AlienRowScoreValue[i];
                    }

                    if ((!Aliens[i][j].Alive && !Aliens[i][j].Exploding)
                        || ((Aliens.Count == 1 && Aliens[0].Count == 1) && !Aliens[0][0].Alive))
                    {
                        Aliens[i].RemoveAt(j);
                        if (Aliens[i].Count == 0)
                        {
                            Aliens.RemoveAt(i);
                            if (Aliens.Count == 0 && PlayerShip.Alive)
                            {
                                PlayerWon = true;
                                TimerBeforeGameOver++;
                            }
                        }
                        return;
                    }
                }
            }
        }

        private void AliensMoveLeft()
        {
            for (int i = 0; i < Aliens.Count; i++)
            {
                for (int j = 0; j < Aliens[i].Count; j++)
                {
                    Aliens[i][j].X -= GameConfig.AlienHorizontalMoveSpeed;
                    Aliens[i][j].ChangeAnimation();
                }
            }
            if (Aliens[0][0].X - GameConfig.AlienHorizontalMoveSpeed < 0)
            {
                ReachedEndOfScreen = true;
            }
        }

        private void AliensMoveRight()
        {
            for (int i = 0; i < Aliens.Count; i++)
            {
                for (int j = 0; j < Aliens[i].Count; j++)
                {
                    Aliens[i][j].X += GameConfig.AlienHorizontalMoveSpeed;
                    Aliens[i][j].ChangeAnimation();
                }
            }
            if (Aliens[0][Aliens[0].Count - 1].X + Aliens[0][Aliens[0].Count - 1].Width + GameConfig.AlienHorizontalMoveSpeed >= GameConfig.WindowWidth)
            {
                ReachedEndOfScreen = true;
            }
        }

        private void InitializeControls()
        {
            ControlIsActive = new Dictionary<PlayerControls, bool>();
            ControlIsActive[PlayerControls.Left] = false;
            ControlIsActive[PlayerControls.Right] = false;
            ControlIsActive[PlayerControls.Shoot] = false;
        }
    }
}