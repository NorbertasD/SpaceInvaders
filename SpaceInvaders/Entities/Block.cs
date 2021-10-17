using System.Drawing;

namespace SpaceInvaders.Entities
{
    public class Block : EntityBase
    {
        public Block(Image[] sprites, int maxDamage) : base(sprites)
        {
            DamageLevel = 0;
            MaxDamage = maxDamage;
        }

        private int MaxDamage { get; }

        private int damageLevel;
        public int DamageLevel
        {
            get
            {
                return damageLevel;
            }
            set
            {
                damageLevel = value;
                AnimationState = value;
            }
        }

        public bool Active
        {
            get
            {
                return DamageLevel < MaxDamage;
            }
        }
    }
}
