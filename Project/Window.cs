using Project.GameClasses;
using Project.GameClasses.EnviromentObjects;
using System.Collections.Concurrent;

namespace Project
{
    public partial class Window : Form
    {
        public const double ScreenScale = 1.2;
        public List<Entity> Entities = new List<Entity>();      //отрисовываемые объекты
        public object EntityLock = new object(); //для синхронизации изменений объектов
        //public ConcurrentBag<Entity> Entities = new();
        public Window()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None; //1920x1080
            WindowState = FormWindowState.Maximized;

            this.Text = "Rougelike";

            this.Paint += new System.Windows.Forms.PaintEventHandler(Window_Paint);
            this.DoubleBuffered = true;

            this.KeyDown += Window_KeyDown;
            this.KeyUp += Window_KeyUp;
            this.MouseClick += Window_Click;
        }

        private void Window_KeyUp(object? sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D)
            {
                Game.Player.Stop(e.KeyCode);
            }
        }

        private void Window_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D)
            {
                Game.Player.Move(e.KeyCode);
            }
            if(e.KeyCode == Keys.Space)
            {
                Game.Player.UseArmor();
            }
            if (e.KeyCode == Keys.Q)
            {
                Game.Player.UseConsumable();
            }
        }

        private void Window_Click(object? sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left) {
                Game.Player.UseFirstWeapon();
            }
            if (e.Button == MouseButtons.Right)
            {
                Game.Player.UseSecondWeapon();
            }

        }


        private void Window_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 3);

            //поле 1600x900, масштабирование 1.2

            lock(EntityLock) {
                foreach (Entity entity in Entities) 
                {
                    if (entity.IsVisible == false) { continue; }
                    Rectangle box = new Rectangle(Convert.ToInt32((entity.X - entity.Size / 2) * ScreenScale),
                        Convert.ToInt32((entity.Y - entity.Size / 2) * ScreenScale),
                        Convert.ToInt32(entity.Size * ScreenScale),
                        Convert.ToInt32(entity.Size * ScreenScale));
                    Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 3);

                    if (entity is SquareEnviromentObject)
                    {
                        e.Graphics.DrawRectangle(pen, box);
                    }
                    else
                    {
                        e.Graphics.DrawEllipse(pen, box);
                    }
                    e.Graphics.DrawString(entity.Name, new Font("Arial", 12), new SolidBrush(Color.Black),
                        Convert.ToInt32((entity.X - entity.Size / 2) * ScreenScale),
                        Convert.ToInt32((entity.Y - entity.Size / 2) * ScreenScale));

                    e.Graphics.DrawString(Convert.ToInt32(Game.Player.Health).ToString()+"/"+ Game.Player.MaxHealth.ToString(), new Font("Arial", 16), new SolidBrush(Color.Red),
                        15, 15);
                    e.Graphics.DrawString(Convert.ToInt32(Game.Player.Mana).ToString() + "/" + Game.Player.MaxMana.ToString(), new Font("Arial", 16), new SolidBrush(Color.Blue),
                        15, 40);
                    e.Graphics.DrawString("Level: " + Game.Level.ToString(), new Font("Arial", 16), new SolidBrush(Color.Green),
                        1800, 15);
                }
            }
            
        }

        public void Window_Close()
        {
            Environment.Exit(0);
        }

    }
}
