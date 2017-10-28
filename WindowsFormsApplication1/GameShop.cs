using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class GameShop : Form
    {
        public GameShop(Shop shop, Character character, GameProcess gp)
        {
            InitializeComponent();
            this.shop = shop;
            this.character = character;

            for (int i = 0; i < shop.ShopItems.Count; i++)
                rectangles.Add(new Rectangle());

            this.gp = gp;
        }

        GameProcess gp;
        Shop shop;
        Character character;
        List<Rectangle> rectangles = new List<Rectangle>();

        private void UpdateShop(Graphics g)
        {
            g.DrawLine(new Pen(Color.Black, 3), 0, 0, 0, this.Height);
            g.DrawLine(new Pen(Color.Black, 3), 0, 0, this.Width, 0);
            g.DrawLine(new Pen(Color.Black, 3), this.Width - 17, 0, this.Width-17, this.Height);
            g.DrawLine(new Pen(Color.Black, 3), 0, this.Height-45, this.Width, this.Height-45);

            g.DrawImage(new Bitmap("Content\\Shop\\Backcolor.png"), new PointF(-10,54));
            //g.DrawRectangle(new Pen(Color.Black, 3f), 50, 10, 100, 50);
            //g.DrawLine(new Pen(Color.Black, 3f), 0, 41, this.Width, 41);

            g.DrawImage(new Bitmap(new Bitmap("Content\\Shop\\star-gold.png"), 32,32), 280, 9); 
            g.DrawString("x " + character.Coins.ToString(), new Font("Book Antiqua", 20f), Brushes.Black, 315, 10);

            //Жизнь
            g.DrawImage(new Bitmap(new Bitmap("Content\\Shop\\333.png"), 64, 64), 10, 60/*45*/);
            g.DrawString("x " + character.Life.ToString(), new Font("Book Antiqua", 30f), Brushes.Black, 66, 70);

            //Защита
            g.DrawImage(new Bitmap(new Bitmap("Content\\Shop\\Shield_Blue.png"), 64, 64), 150, 60);
            g.DrawString("= " + character.ResistanceToDamage.ToString(), new Font("Book Antiqua", 30f), Brushes.Black, 210, 70);

            //Здоровье
            
            g.FillRectangle(Brushes.Red, 340, 64, 120 * character.CurrentHealth / character.MaxHealth, 25);
            g.DrawRectangle(Pens.Black, 340, 64, 120, 25);
            g.DrawString(character.CurrentHealth.ToString() + " / " + character.MaxHealth.ToString(), new Font("Book Antiqua", 12f, FontStyle.Bold), Brushes.Black, 345, 90);

            //Контроль
            g.DrawImage(new Bitmap(new Bitmap("Content\\Shop\\ControlImage.png"), 64, 64), 480, 60);
            g.DrawString("= " + character.LevelControlBot.ToString(), new Font("Book Antiqua", 30f), Brushes.Black, 550, 70);

            g.DrawImage(new Bitmap(new Bitmap("Content\\Shop\\cont2.png"), 647, 285), new PointF(2, 150));

            int X = 26;
            int Y = 175;

            for(int i = 0; i < shop.ShopItems.Count; i++)
            {
                DrawItem(shop.ShopItems[i], g, new Point(X, Y));
                rectangles[i] = new Rectangle(520, Y + 10, 507, 47);

                Y += 47;
            }
        }

        private void DrawItem(ShopItem itemshop, Graphics g, Point p)
        {
            g.FillRectangle(Brushes.Yellow, p.X, p.Y, 597, 47);
            g.DrawRectangle(Pens.Black, p.X, p.Y, 597, 47);
            g.DrawString("Купить", new Font("Microsoft Sans Serif", 15), Brushes.Black, new PointF(520, p.Y + 10));
            g.DrawString(itemshop.Price.ToString(), new Font("Microsoft Sans Serif", 15), Brushes.Black, new PointF(370, p.Y + 10));
            g.DrawLine(Pens.Black, 360, p.Y, 360, p.Y + 47);
            g.DrawLine(Pens.Black, 507, p.Y, 507, p.Y + 47);
            g.DrawString(GetStringShopItem(itemshop), new Font("Microsoft Sans Serif", 15), Brushes.Black, new PointF(p.X + 2, p.Y + 10));
        }

        private string GetStringShopItem(ShopItem si)
        {
            switch(si.NameItem)
            {
                case ItemShop.Life:
                    return "Увеличить жизнь на " + si.Value+"ед.";

                case ItemShop.Health:
                    return "Пополнить здоровье на " + si.Value + "ед.";

                case ItemShop.Control:
                    return "Повысить контроль на" + si.Value;

                case ItemShop.Protection:
                    return "Повысить защиту на " + si.Value;

                case ItemShop.UpHealth:
                    return "Увеличить Max здоровье на " + si.Value + "ед.";
                default:
                    return null;
            }
        }

        private void GameShop_Paint(object sender, PaintEventArgs e)
        {
            UpdateShop(e.Graphics);
        }

        private void GameShop_MouseUp(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < rectangles.Count; i++)
                if (rectangles[i].Contains(e.Location))
                {
                    if (!shop.Buy(character, shop.ShopItems[i]))
                        MessageBox.Show("Недостаточно очков!");
                    else
                        this.Invalidate();
                }
        }

        private void GameShop_FormClosed(object sender, FormClosedEventArgs e)
        {
            gp.pause = false;
        }
    }
}
