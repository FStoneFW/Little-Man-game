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
    public partial class TwoStatePropForm : Form
    {
        public TwoStatePropForm(GameObject gameobject)
        {
            InitializeComponent();
            this.gameobject = gameobject;
            LoadObject(gameobject);
        }

        GameObject gameobject;
        string[] temppath = new string[2];

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

        private void LoadObject(GameObject dobject)
        {
            if (dobject.GameSprite.PathImage[0] != null)
                pictureBox1.Image = new Bitmap(dobject.GameSprite.PathImage[0]);

            if (dobject.GameSprite.PathImage[1] != null)
                pictureBox2.Image = new Bitmap(dobject.GameSprite.PathImage[1]);

            temppath[0] = dobject.GameSprite.PathImage[0];
            temppath[1] = dobject.GameSprite.PathImage[1];
        }

        private void Save(GameObject dobject)
        {
            dobject.UserSizeImage = dobject.GameSprite.PathImage[0] != temppath[0] ? new SizeF(0, 0) : dobject.UserSizeImage;

            dobject.GameSprite.PathImage[0] = temppath[0];
            dobject.GameSprite.PathImage[1] = temppath[1];

            ISprite isprite = dobject;
            isprite.LoadSprite();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save(gameobject);
        }
    }
}
