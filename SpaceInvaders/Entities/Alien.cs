using System;
using System.Drawing;

namespace SpaceInvaders.Entities
{
    public class Alien : EntityBase
    {
        public Alien(Image[] sprites, int speed) : base(sprites)
        {
            Alive = true;
            Exploding = false;
            Speed = speed;
        }

        public bool Alive { get; set; }

        public bool Exploding { get; set; }

        private int Speed { get; }


        public void Shoot(Shot alienShot)
        {
            alienShot.X = X + (Width / 2) - (alienShot.Width / 2);
            alienShot.Y = Y + Height;
            alienShot.Active = true;
            alienShot.Cooldown = alienShot.CooldownSetting;
        }

        public void ChangeAnimation()
        {
            if (Exploding)
            {
                Exploding = false;
            }

            if (AnimationState == 0)
            {
                AnimationState = 1;
            }
            else if (AnimationState == 1)
            {
                AnimationState = 0;
            }
        }

        public void MoveLeft()
        {
            X -= Speed;
        }

        public void MoveRight()
        {
            X += Speed;
        }
    }
}