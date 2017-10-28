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
    public partial class DoorPropForm : Form
    {
        public DoorPropForm(Door door)
        {
            InitializeComponent();
            this.door = door;
            LoadDoor(door);
        }

        Door door;
        string[] temppath = new string[2];

        private void LoadDoor(Door d)
        {
            checkBox1.Checked = d.Opened;
            checkBox2.Checked = d.OpenHorizontal;
            checkBox3.Checked = d.DirectionOpenDoorRight;


            if (d.GameSprite.PathImage[0] != null)
                pictureBox1.Image = new Bitmap(d.GameSprite.PathImage[0]);

            if (d.GameSprite.PathImage[1] != null)
                pictureBox2.Image = new Bitmap(d.GameSprite.PathImage[1]);

            temppath[0] = d.GameSprite.PathImage[0];
            temppath[1] = d.GameSprite.PathImage[1];
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

        private void Save(Door d)
        {
            d.Opened = checkBox1.Checked;
            d.OpenHorizontal = checkBox2.Checked;
            d.DirectionOpenDoorRight = checkBox3.Checked;

            d.UserSizeImage = d.GameSprite.PathImage[0] != temppath[0] ? new SizeF(0, 0) : d.UserSizeImage;

            d.GameSprite.PathImage[0] = temppath[0];
            d.GameSprite.PathImage[1] = temppath[1];

            ISprite isprite = d;
            isprite.LoadSprite();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save(door);
        }
    }
}
