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
    public partial class BotPropForm : Form
    {
        public BotPropForm(Bot bot)
        {
            InitializeComponent();
            this.bot = bot;
            if (bot.Anim.PathTileset != null)
                pictureBox1.Image = new Bitmap(bot.Anim.PathTileset);
            temppath[0] = bot.Anim.PathTileset;
            LoadProp(bot);
        }

        Bot bot;
        string[] temppath = new string[1];

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //damageobject.GameSprite.PathImage[0] = openFileDialog1.FileName;
                temppath[0] = openFileDialog1.FileName;
                // dobject.ObjectSprite.Image = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                //pb.Size = new Bitmap(openFileDialog1.FileName).Size;
            }
        }
        private void LoadProp(Bot b)
        {
            textBox1.Text = b.Damage.ToString();
            textBox2.Text = b.LevelControl.ToString();
            textBox3.Text = b.PhysBody.MaxSpeed.ToString();
            textBox4.Text = b.Anim.SpriteSpeed.ToString();
            textBox5.Text = b.PhysBody.MaxJump.ToString();
        }

        private void SaveProp(Bot b)
        {
            b.Damage = Convert.ToInt32(textBox1.Text);
            b.LevelControl = Convert.ToInt32(textBox2.Text);
            b.PhysBody.MaxSpeed = Convert.ToInt32(textBox3.Text);
            b.Anim.PathTileset = temppath[0];
            b.Anim.SpriteSpeed = float.Parse(textBox4.Text); //(float)Convert.ToDouble(textBox4.Text);
            b.PhysBody.MaxJump = float.Parse(textBox5.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveProp(bot);
        }
    }
}
