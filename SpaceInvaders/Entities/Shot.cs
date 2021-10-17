using System.Drawing;

namespace SpaceInvaders.Entities
{
    public class Shot : EntityBase
    {
        public Shot(Image[] sprites, int speed, int movementBoundary, int cooldown) : base(sprites)
        {
            Active = false;
            Speed = speed;
            Cooldown = 0;
            MovementBoundary = movementBoundary;
            CooldownSetting = cooldown;
        }

        public bool Active { get; set; }

        public int Speed { get; }

        public int Cooldown { get; set; }

        public int CooldownSetting { get; }

        private int MovementBoundary { get; }

        private bool CanMove()
        {
            if (Speed < 0)
            {
                return Y + Speed >= MovementBoundary;
            }
            else
            {
                return Y + Height + Speed <= MovementBoundary;
            }
        }

        public void Move()
        {
            if (CanMove())
            {
                Y += Speed;
            }
            else
            {
                Active = false;
            }
        }
    }
}