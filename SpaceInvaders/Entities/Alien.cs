using System;
using System.Drawing;

namespace SpaceInvaders.Entities
{
    public class Alien : ComplexEntity
    {
        public Alien(Image[] sprites, int speed) : base(sprites, speed)
        {
            Alive = true;
        }

        public override void Shoot(Shot alienShot)
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

        public override void MoveLeft()
        {
            X -= Speed;
        }

        public override void MoveRight()
        {
            X += Speed;
        }
    }
}