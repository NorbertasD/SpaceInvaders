using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Entities
{
    public abstract class ComplexEntity : EntityBase
    {
        public ComplexEntity(Image[] sprites, int speed) : base(sprites)
        {
            Exploding = false;
            Speed = speed;
        }

        public virtual bool Alive { get; set; }

        public bool Exploding { get; set; }

        protected int Speed { get; }

        public abstract void Shoot(Shot shot);

        public abstract void MoveLeft();

        public abstract void MoveRight();
    }
}
