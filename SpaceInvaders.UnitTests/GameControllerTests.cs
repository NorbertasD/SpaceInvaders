using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace SpaceInvaders.UnitTests
{
    [TestClass]
    public class GameControllerTests
    {
        private GameController InitGame()
        {
            string projectDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, "Images");

            Sprites.Instance.LoadFromFile(projectDirectory);

            GameConfig.Instance.WindowWidth = 1084;
            GameConfig.Instance.WindowHeight = 761;

            GameController gameController = new GameController();
            gameController.Initialize();
            return gameController;
        }

        [TestMethod]
        public void UpdateFrame_PlayerLivesDepleted_PlayerExploding()
        {
            GameController gameController = InitGame();

            gameController.AlienShot.Active = true;
            gameController.AlienShot.X = gameController.PlayerShip.X;
            gameController.AlienShot.Y = gameController.PlayerShip.Y - GameConfig.Instance.AlienShotSpeed - gameController.AlienShot.Height;

            gameController.PlayerShip.Lives = 1;

            gameController.UpdateFrame();

            Assert.IsTrue(gameController.PlayerShip.Exploding == true);
        }

        [TestMethod]
        public void UpdateFrame_AlienSaucerIsHit_PlayerLivesIncreased()
        {
            GameController gameController = InitGame();

            gameController.FlyingSaucer.Active = true;

            gameController.PlayerShot.Active = true;
            gameController.PlayerShot.X = gameController.FlyingSaucer.X;
            gameController.PlayerShot.Y = gameController.FlyingSaucer.Y + gameController.FlyingSaucer.Height + GameConfig.Instance.PlayerShotSpeed - 1;
            
            int playerLives = gameController.PlayerShip.Lives;

            gameController.UpdateFrame();

            Assert.IsTrue(gameController.PlayerShip.Lives == playerLives + 1);
        }

        [TestMethod]
        public void UpdateFrame_AliensMoveDown_AlienYIncreased()
        {
            GameController gameController = InitGame();

            GameConfig.Instance.AlienShotSpeed = 0;

            int distanceToWall = (GameConfig.Instance.WindowWidth - gameController.Aliens[0][0].X) 
                - (gameController.Aliens[0][0].Width * GameConfig.Instance.EnemyColumns 
                + GameConfig.Instance.FreeWidth * (GameConfig.Instance.EnemyColumns - 1));

            int movesUntilMoveDown = distanceToWall / GameConfig.Instance.AlienHorizontalMoveSpeed;

            int alienInitialY = gameController.Aliens[0][0].Y;

            for (int i = 0; i < movesUntilMoveDown * gameController.AlienMoveInterval; i++)
            {
                gameController.UpdateFrame();
            }            

            Assert.IsTrue(gameController.Aliens[0][0].Y == alienInitialY + GameConfig.Instance.AlienVerticalMoveSpeed);
        }
    }
}
