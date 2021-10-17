using System.Drawing;

namespace SpaceInvaders.Entities
{
    public class FlyingSaucer : EntityBase
    {
        public FlyingSaucer(Image[] sprites, int moveSpeed, int movementBoundary) : base(sprites)
        {
            Active = false;
            Exploding = false;
            Speed = moveSpeed;
            MovementBoundary = movementBoundary;
        }

        public bool Active { get; set; }

        public bool Exploding { get; set; }

        public int Speed { get; }

        public int MovementBoundary { get; }

        public int ExplosionFrameCounter { get; set; } = 0;

        public void MoveLeft()
        {
            if(X - Speed >= MovementBoundary)
            {
                X -= Speed;
            }
            else
            {
                Active = false;
            }
        }
    }
}
