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
    public partial class ItemPropForm : Form
    {
        public ItemPropForm(Item item)
        {
            InitializeComponent();
            this.item = item;
            LoadItemObject(item);
        }

        private Item item;
        private string[] temppath = new string[1];

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                temppath[0] = openFileDialog1.FileName;
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void LoadItemObject(Item i)
        {
            checkBox1.Checked = i.PhysBody.Gravity;

            if (i.GameSprite.PathImage[0] != null)
                pictureBox1.Image = new Bitmap(i.GameSprite.PathImage[0]);

            temppath[0] = i.GameSprite.PathImage[0];

            textBox1.Text = i.Life.ToString();
            textBox2.Text = i.Health.ToString();
            textBox3.Text = i.Coin.ToString();
            textBox4.Text = i.Control.ToString();

            textBox5.Text = i.UserSizeImage.Width.ToString();
            textBox6.Text = i.UserSizeImage.Width.ToString();

            switch (i.Name)
            {
                case "Life":
                    textBox1.Enabled = true;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    break;

                case "Health":
                    textBox1.Enabled = false;
                    textBox2.Enabled = true;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    break;

                case "Coin":
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox3.Enabled = true;
                    textBox4.Enabled = false;
                    break;

                case "Control":
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = true;
                    break;

                default:
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    break;

            }
        }

        private void Save(Item i)
        {
            i.PhysBody.Gravity = checkBox1.Checked;

            i.Life = Convert.ToInt32(textBox1.Text);
            i.Health = Convert.ToInt32(textBox2.Text);
            i.Coin = Convert.ToInt32(textBox3.Text);
            i.Control = Convert.ToInt32(textBox4.Text);

            i.UserSizeImage = i.GameSprite.PathImage != temppath ? new SizeF(0, 0) : i.UserSizeImage;

            i.GameSprite.PathImage = temppath;

            i.UserSizeImage = textBox5.Text != "" && textBox6.Text != "" ? new SizeF(Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox6.Text)) : new SizeF(0, 0);

            ISprite isprite = i;
            isprite.LoadSprite();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save(item);
        }
    }
}
