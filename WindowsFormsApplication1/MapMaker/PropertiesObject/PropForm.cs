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
    public partial class PropForm : Form
    {
        public PropForm(GameObject gameobject)
        {
            InitializeComponent();
            this.gameobject = gameobject;
            pictureBox1.Image = new Bitmap(gameobject.GameSprite.PathImage[0]);
            temppath[0] = gameobject.GameSprite.PathImage[0];
        }

        GameObject gameobject;
        string[] temppath = new string[1];

        private void PictureBox_Click(object sender, EventArgs e)
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

        private void Save(GameObject gobject)
        {
            gobject.UserSizeImage = gobject.GameSprite.PathImage[0] != temppath[0] ? new SizeF(0, 0) : gobject.UserSizeImage;

            gobject.GameSprite.PathImage[0] = temppath[0];

            ISprite isprite = gobject;
            isprite.LoadSprite();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save(gameobject);
        }
    }
}
