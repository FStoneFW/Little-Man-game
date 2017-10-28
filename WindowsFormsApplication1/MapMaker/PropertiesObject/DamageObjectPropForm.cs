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
    public partial class DamageObjectPropForm : Form
    {
        public DamageObjectPropForm(DamageObject damageobject)
        {
            InitializeComponent();
            this.damageobject = damageobject;
            LoadDamageObject(damageobject);
        }

        DamageObject damageobject;
        string[] temppath = new string[2];

        private void LoadDamageObject(DamageObject dobject)
        {
            if (dobject.Destructible)
                checkBox1.Checked = true;
            if (dobject.Cripple)
                checkBox2.Checked = true;
            else
                groupBox1.Enabled = false;
            textBox1.Text = dobject.Damage.ToString();

            if (dobject.GameSprite.PathImage[0] != null)
                pictureBox1.Image = new Bitmap(dobject.GameSprite.PathImage[0]);

            if (dobject.GameSprite.PathImage[1] != null)
                pictureBox2.Image = new Bitmap(dobject.GameSprite.PathImage[1]);

            temppath[0] = dobject.GameSprite.PathImage[0];
            temppath[1] = dobject.GameSprite.PathImage[1];
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

        private void Save(DamageObject dobject)
        {
            dobject.Destructible = checkBox1.Checked;
            dobject.Cripple = checkBox2.Checked;
            dobject.Damage = Convert.ToInt32(textBox1.Text);

            dobject.UserSizeImage = dobject.GameSprite.PathImage[0] != temppath[0] ? new SizeF(0, 0) : dobject.UserSizeImage;

            dobject.GameSprite.PathImage[0] = temppath[0];
            dobject.GameSprite.PathImage[1] = temppath[1];

            ISprite isprite = dobject;
            isprite.LoadSprite();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = checkBox2.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save(damageobject);
        }
    }
}
