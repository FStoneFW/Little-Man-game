using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1.MapMaker.PropertiesObject
{
    public partial class ChestPropForm : Form
    {
        public ChestPropForm(Chest chest)
        {
            InitializeComponent();
            this.chest = chest;
            LoadChestObject(chest);
            LoadItems(chest.Items);
        }

        private Chest chest;
        private string[] temppath = new string[2];

        private void LoadChestObject(Chest c)
        {
            checkBox1.Checked = c.Locked;

            if (c.GameSprite.PathImage[0] != null)
                pictureBox1.Image = new Bitmap(c.GameSprite.PathImage[0]);

            if (c.GameSprite.PathImage[1] != null)
                pictureBox2.Image = new Bitmap(c.GameSprite.PathImage[1]);

            temppath[0] = c.GameSprite.PathImage[0];
            temppath[1] = c.GameSprite.PathImage[1];
        }

        private void Save(Chest c)
        {
            c.Locked = checkBox1.Checked;

            c.UserSizeImage = c.GameSprite.PathImage[0] != temppath[0] ? new SizeF(0, 0) : c.UserSizeImage;

            c.GameSprite.PathImage[0] = temppath[0];
            c.GameSprite.PathImage[1] = temppath[1];

            ISprite isprite = c;
            isprite.LoadSprite();
        }

        private void LoadItems(List<Item> items)
        {
            listBox1.Items.Clear();

            foreach (Item i in items)
                listBox1.Items.Add(LabelItem(i));
        }

        private string LabelItem(Item i)
        {
            switch(i.Name)
            {
                case "Life":
                    return "Life +" + i.Life;
                case "Health":
                    return "Health +" + i.Health;
                case "Coin":
                    return "Coin +" + i.Coin;
                case "Control":
                    return "Control +" + i.Control;
                default:
                    return null;
            }
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (pb.Name == "pictureBox1")
                {
                    //damageobject.GameSprite.PathImage[0] = openFileDialog1.FileName;
                    temppath[0] = openFileDialog1.FileName;
                    // dobject.ObjectSprite.Image = new Bitmap(openFileDialog1.FileName);
                    pb.Image = new Bitmap(openFileDialog1.FileName);
                    //pb.Size = new Bitmap(openFileDialog1.FileName).Size;
                }
                else if (pb.Name == "pictureBox2")
                {
                    //damageobject.GameSprite.PathImage[1] = openFileDialog1.FileName;
                    temppath[1] = openFileDialog1.FileName;
                    //dobject.States[0].Image = new Bitmap(openFileDialog1.FileName);
                    pb.Image = new Bitmap(openFileDialog1.FileName);
                    //pb.Size = new Bitmap(openFileDialog1.FileName).Size;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save(chest);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            chest.Items.Add(new Item("Life") { Position = new RectangleF(new PointF(), new Bitmap("Content\\OtherObjects\\Life.png").Size), GameSprite = new Sprite(new string[] { "Content\\OtherObjects\\Life.png" }) });
            chest.Items[chest.Items.Count - 1].PhysBody.Gravity = true;
            LoadItems(chest.Items);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            chest.Items.Add(new Item("Health") { Position = new RectangleF(new PointF(), new Bitmap("Content\\OtherObjects\\Health.png").Size), GameSprite = new Sprite(new string[] { "Content\\OtherObjects\\Health.png" }) });
            chest.Items[chest.Items.Count - 1].PhysBody.Gravity = true;
            LoadItems(chest.Items);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            chest.Items.Add(new Item("Coin") { Position = new RectangleF(new PointF(), new Bitmap("Content\\OtherObjects\\Money.png").Size), GameSprite = new Sprite(new string[] { "Content\\OtherObjects\\Money.png" }) });
            chest.Items[chest.Items.Count - 1].PhysBody.Gravity = true;
            LoadItems(chest.Items);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            chest.Items.Add(new Item("Control") { Position = new RectangleF(new PointF(), new Bitmap("Content\\OtherObjects\\ControlBall.png").Size), GameSprite = new Sprite(new string[] { "Content\\OtherObjects\\ControlBall.png" }) });
            chest.Items[chest.Items.Count - 1].PhysBody.Gravity = true;
            LoadItems(chest.Items);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                chest.Items.RemoveAt(listBox1.SelectedIndex);
                LoadItems(chest.Items);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                new ItemPropForm(chest.Items[listBox1.SelectedIndex]).ShowDialog();
            }
        }
    }
}
