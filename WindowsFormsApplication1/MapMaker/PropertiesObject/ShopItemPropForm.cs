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
    public partial class ShopItemPropForm : Form
    {
        public ShopItemPropForm(ShopItem shopitem)
        {
            InitializeComponent();
            this.shopitem = shopitem;
            LoadShopItemObject(shopitem);
        }

        private ShopItem shopitem;
        private string[] temppath = new string[1];

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                temppath[0] = openFileDialog1.FileName;
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void LoadShopItemObject(ShopItem si)
        {
            label1.Text = si.NameItem.ToString();

            if (si.GameImage.PathImage[0] != null)
                pictureBox1.Image = new Bitmap(si.GameImage.PathImage[0]);

            temppath[0] = si.GameImage.PathImage[0];

            textBox1.Text = si.Value.ToString();
            textBox2.Text = si.Price.ToString();
            textBox3.Text = si.Plusprice.ToString();
            textBox4.Text = si.Description;
        }
        private void Save(ShopItem si)
        {
            si.Value = Convert.ToInt32(textBox1.Text);
            si.Price = Convert.ToInt32(textBox2.Text);
            si.Plusprice = Convert.ToInt32(textBox3.Text);
            si.Description = textBox4.Text;


            si.GameImage.PathImage = temppath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save(shopitem);
        }
    }
}
