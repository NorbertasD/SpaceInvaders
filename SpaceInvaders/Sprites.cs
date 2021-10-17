using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SpaceInvaders
{
    public class Sprites
    {
        private static Sprites instance = null;

        public static Sprites Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Sprites();
                }
                return instance;
            }
        }

        private Sprites()
        {

        }

        public void LoadFromFile(string projectDirectory)
        {
            PlayerShip[0] = Image.FromFile(Path.Combine(projectDirectory, "ship_0.png"));
            PlayerShip[1] = Image.FromFile(Path.Combine(projectDirectory, "ship_1.png"));
            PlayerShip[2] = Image.FromFile(Path.Combine(projectDirectory, "ship_2.png"));

            PlayerShot[0] = Image.FromFile(Path.Combine(projectDirectory, "player_shot.png"));

            Image[] alien1 = new Image[3];
            alien1[0] = Image.FromFile(Path.Combine(projectDirectory, "Aliens", "alien1_0.png"));
            alien1[1] = Image.FromFile(Path.Combine(projectDirectory, "Aliens", "alien1_1.png"));
            alien1[2] = Image.FromFile(Path.Combine(projectDirectory, "Aliens", "alien1_2.png"));
            Aliens.Add(alien1);

            Image[] alien2 = new Image[3];
            alien2[0] = Image.FromFile(Path.Combine(projectDirectory, "Aliens", "alien2_0.png"));
            alien2[1] = Image.FromFile(Path.Combine(projectDirectory, "Aliens", "alien2_1.png"));
            alien2[2] = Image.FromFile(Path.Combine(projectDirectory, "Aliens", "alien2_2.png"));
            Aliens.Add(alien2);
            Aliens.Add(alien2);

            Image[] alien3 = new Image[3];
            alien3[0] = Image.FromFile(Path.Combine(projectDirectory, "Aliens", "alien3_0.png"));
            alien3[1] = Image.FromFile(Path.Combine(projectDirectory, "Aliens", "alien3_1.png"));
            alien3[2] = Image.FromFile(Path.Combine(projectDirectory, "Aliens", "alien3_2.png"));
            Aliens.Add(alien3);
            Aliens.Add(alien3);

            AlienShot[0] = Image.FromFile(Path.Combine(projectDirectory, "alien_shot.png"));

            //All block sprites must be the same lenght
            Block[0] = Image.FromFile(Path.Combine(projectDirectory, "Blocks", "block0_0.png"));
            Block[1] = Image.FromFile(Path.Combine(projectDirectory, "Blocks", "block0_1.png"));
            Block[2] = Image.FromFile(Path.Combine(projectDirectory, "Blocks", "block0_2.png"));
            Block[3] = Image.FromFile(Path.Combine(projectDirectory, "Blocks", "block0_3.png"));

            PlayerHealth[0] = Image.FromFile(Path.Combine(projectDirectory, "ship_health.png"));

            FlyingSaucer[0] = Image.FromFile(Path.Combine(projectDirectory, "flying_saucer_0.png"));
            FlyingSaucer[1] = Image.FromFile(Path.Combine(projectDirectory, "flying_saucer_1.png"));
        }

        public Image[] PlayerShip { get; private set; } = new Image[3];

        public Image[] PlayerShot { get; private set; } = new Image[1];

        public Image[] AlienShot { get; private set; } = new Image[1];

        public List<Image[]> Aliens { get; private set; } = new List<Image[]>();

        public Image[] Block { get; private set; } = new Image[4];

        public Image[] PlayerHealth { get; private set; } = new Image[1];

        public Image[] FlyingSaucer { get; private set; } = new Image[2];
    }
}
