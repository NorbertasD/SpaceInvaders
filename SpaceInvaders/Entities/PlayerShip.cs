using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Entities
{
    public class PlayerShip : ComplexEntity
    {
        public PlayerShip(Image[] sprites, int lives, int speed, int leftBoundary, int rightBoundary) : base(sprites, speed)
        {
            Lives = lives;
            LeftBoundary = leftBoundary;
            RightBoundary = rightBoundary;
            ExplosionFrameCounter = 0;
        }

        public int Lives { get; set; }

        private int LeftBoundary { get; }

        private int RightBoundary { get; }

        public override bool Alive
        {
            get
            {
                return Lives > 0;
            }
        }

        private int ExplosionFrameCounter { get; set; }

        public override void Shoot(Shot playerShot)
        {
            playerShot.X = X + (Width / 2) - (playerShot.Width / 2);
            playerShot.Y = Y - playerShot.Height;
            playerShot.Active = true;
            playerShot.Cooldown = playerShot.CooldownSetting;
        }

        public override void MoveLeft()
        {
            if (X - Speed >= LeftBoundary)
            {
                X -= Speed;
            }
        }

        public override void MoveRight()
        {
            if (X + Width + Speed <= RightBoundary)
            {
                X += Speed;
            }
        }

        public void AdvanceExplosion()
        {
            ExplosionFrameCounter++;

            switch (ExplosionFrameCounter)
            {
                case 5:
                    AnimationState = 2;
                    break;
                case 20:
                    AnimationState = 1;
                    break;
                case 25:
                    Exploding = false;
                    break;
                default:
                    break;
            }
        }
    }
}