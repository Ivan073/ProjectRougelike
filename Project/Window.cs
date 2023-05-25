using Project.GameClasses;
using Project.GameClasses.Enemies;
using Project.GameClasses.EnviromentObjects.RoundEnvirometObjects;
using Project.GameClasses.EnviromentObjects.SquareEnviromentObjects;
using Project.GameClasses.Items;
using Project.GameClasses.Items.Weapons;
using System.Collections.Concurrent;
using System.Drawing;
using System.Reflection;
using System.Security.Policy;

namespace Project
{
    public partial class Window : Form
    {
        public const double ScreenScale = 1.2;
        public List<Entity> Entities = new List<Entity>();      //отрисовываемые объекты
        public object EntityLock = new object(); //для синхронизации изменений объектов

        public new static Image? Background = Image.FromFile("Images/background.jpg");
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


            Label healthLabel = new Label();
            healthLabel.Location = new Point(10, 10);
            healthLabel.Height = 30;
            healthLabel.Width = 120;
            healthLabel.BackColor = Color.Red;
            healthLabel.Name = "healthLabel";
            this.Controls.Add(healthLabel);

            Label manaLabel = new Label();
            manaLabel.Location = new Point(10, 40);
            manaLabel.Height = 30;
            manaLabel.Width = 120;
            manaLabel.BackColor = Color.Cyan;
            manaLabel.Name = "manaLabel";
            this.Controls.Add(manaLabel);

            this.BackgroundImage = Image.FromFile("Images/background.jpg");
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
            if (e.KeyCode == Keys.R)
            {
                Game.RemovePickup();
            }
            if (e.KeyCode == Keys.E)
            {
                Game.Player.PickupItem();
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
            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0), 3);

            //поле 1600x900, масштабирование 1.2

            Player? player = null;
            Controls.RemoveByKey("itemInfo");
            lock (EntityLock)
            {

                foreach (Entity entity in Entities)
                {
                    if (entity.IsVisible == false) { continue; }
                    if (entity is Player) { player = (Player)entity; continue; }
                    if (entity is PickableItemEntity && Math.Sqrt(Math.Pow(entity.Y - Game.Player.Y, 2) + Math.Pow(entity.X - Game.Player.X, 2)) <= entity.Size / 2 + Game.Player.Size / 2)
                    {
                        Label info = new Label();
                        info.Location = new Point(1670, 0);
                        info.Height = 160;
                        info.Width = 250;
                        info.BackColor = Color.Gray;
                        info.Name = "itemInfo";
                        info.Font = new Font("Times New Roman",20);

                        var item = ((PickableItemEntity)entity).AssociatedItem;
                        if (item is Weapon)
                        {
                            var w = (Weapon)item;
                            info.Text += "Cost: " + w.Cost+"\n";
                            info.Text += "Damage: " + w.Damage + "\n";
                            info.Text += "Duration: " + w.Duration + "\n";
                            info.Text += "Size: " + w.Size + "\n";
                        }
                        if (item is Armor)
                        {
                            var w = (Armor)item;
                            info.Text += "Cost: " + w.Cost + "\n";
                            info.Text += "Defence: " + Math.Round(w.DefencePercent * 100)+ "%\n";
                        }
                        if (item is ConsumableHeal)
                        {
                            var w = (ConsumableHeal)item;
                            info.Text += "Cost: " + w.Cost + "\n";
                            info.Text += "Heal: " + w.Heal + "\n";
                        }


                        this.Controls.Add(info);
                    }

                    Type type = entity.GetType();
                    FieldInfo field = type.GetField("Sprite", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    Image? sprite = (Image?)field.GetValue(null);

                    if (sprite != null)
                    {
                        e.Graphics.DrawImage(sprite,
                            new Rectangle(Convert.ToInt32((entity.X - entity.Size / 2) * ScreenScale - 5),
                            Convert.ToInt32((entity.Y - entity.Size / 2) * ScreenScale - 5),
                            Convert.ToInt32(entity.Size * ScreenScale + 6),
                            Convert.ToInt32(entity.Size * ScreenScale + 6)));
                    }
                    else
                    {
                        Rectangle box = new Rectangle(Convert.ToInt32((entity.X - entity.Size / 2) * ScreenScale),
                        Convert.ToInt32((entity.Y - entity.Size / 2) * ScreenScale),
                        Convert.ToInt32(entity.Size * ScreenScale),
                        Convert.ToInt32(entity.Size * ScreenScale));

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

                    }

                    Label healthLabel = (Label)this.Controls["healthLabel"];
                    healthLabel.Width = (int)(120 * (Game.Player.Health / 100));

                    Label manaLabel = (Label)this.Controls["manaLabel"];
                    manaLabel.Width = (int)(120 * (Game.Player.Mana / 100));

                    e.Graphics.DrawString("Level: " + Game.Level.ToString(), new Font("Arial", 16), new SolidBrush(Color.White),
                        1800, 15);
                }

                

                if (Game.Player.FirstWeapon != null)
                {
                    Rectangle box = new Rectangle(50, 1000, 50, 50);
                    e.Graphics.DrawRectangle(pen, box);
                    Image? sprite = null;
                    if (Game.Player.FirstWeapon is AreaWeapon) { sprite = AreaWeaponEntity.Sprite; }
                    if (Game.Player.FirstWeapon is ProjectileWeapon) { sprite = ProjectileWeaponEntity.Sprite; }
                    e.Graphics.DrawImage(sprite, box);
                    if (Game.Player.FirstWeaponCooldownTimer != null)
                    {
                        Brush br = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
                        e.Graphics.FillRectangle(br, box);
                    }
                }
                if (Game.Player.SecondWeapon != null)
                {
                    Image? sprite = null;
                    Rectangle box = new Rectangle(150, 1000, 50, 50);
                    e.Graphics.DrawRectangle(pen, box);
                    if (Game.Player.SecondWeapon is AreaWeapon) { sprite = AreaWeaponEntity.Sprite; }
                    if (Game.Player.SecondWeapon is ProjectileWeapon) { sprite = ProjectileWeaponEntity.Sprite; }

                    e.Graphics.DrawImage(sprite, box);
                    if (Game.Player.SecondWeaponCooldownTimer!=null)
                    {
                        Brush br = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
                        e.Graphics.FillRectangle(br, box);
                    }
                }
                if (Game.Player.Armor != null)
                {
                    Rectangle box = new Rectangle(250, 1000, 50, 50);
                    e.Graphics.DrawRectangle(pen, box);
                    e.Graphics.DrawImage(Armor.Sprite, box);
                    if (Game.Player.Armor.Active)
                    {
                        Brush br = new SolidBrush(Color.FromArgb(127, 0, 255, 0));
                        e.Graphics.FillRectangle(br, box);
                    }
                }
                e.Graphics.DrawRectangle(pen, new Rectangle(350, 1000, 50, 50));
                if (Game.Player.Consumable != null)
                {
                    
                    e.Graphics.DrawImage(ConsumableHeal.Sprite, new Rectangle(350, 1000, 50, 50));
                }

                

                Type ptype = player.GetType();
                FieldInfo pfield = ptype.GetField("Sprite", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                Image? psprite = (Image?)pfield.GetValue(null);

                e.Graphics.DrawImage(psprite,
                               new Rectangle(Convert.ToInt32((player.X - player.Size / 2) * ScreenScale - 3),
                               Convert.ToInt32((player.Y - player.Size / 2) * ScreenScale - 3),
                               Convert.ToInt32(player.Size * ScreenScale + 6),
                               Convert.ToInt32(player.Size * ScreenScale + 6)));
                 }
            }

            public void Window_Close()
            {
                Environment.Exit(0);
            }
        
    }
}
