using System.Drawing;

namespace SpaceInvaders.Entities
{
    public abstract class EntityBase
    {
        public EntityBase(Image[] sprites)
        {
            this.Sprites = sprites;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width
        {
            get { return Sprites[AnimationState].Width; }
        }

        public int Height
        {
            get { return Sprites[AnimationState].Height; }
        }

        public int AnimationState { get; set; } = 0;

        public Image[] Sprites { get; }

        public Image Sprite
        {
            get { return Sprites[AnimationState]; }
            set { Sprites[AnimationState] = value; }
        }
    }
}