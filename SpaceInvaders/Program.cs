using System;
using System.IO;
using System.Windows.Forms;

namespace SpaceInvaders
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string projectDirectory = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, "Images");

            if(!Directory.Exists(projectDirectory))
            {
                projectDirectory = Path.Combine(Environment.CurrentDirectory, "Images");
            }

            try
            {
                Sprites.Instance.LoadFromFile(projectDirectory);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load images.\nThe program will now exit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GameView gameView = new GameView();
            gameView.FormBorderStyle = FormBorderStyle.FixedSingle;
            gameView.MaximizeBox = false;

            Application.Run(gameView);
        }
    }
}
