using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApplication1.MapMaker.PropertiesObject
{
    public partial class SwitcherPropForm : Form
    {
        public SwitcherPropForm(Switcher switcher, Level level)
        {
            InitializeComponent();
            this.switcher = switcher;
            this.level = level;
            LoadSwitcherObject(switcher);
            ShowActivObjects(switcher.CanActivateObject);
            if (switcher is Trigger)
            {
                checkBox1.Visible = false;
                groupBox2.Visible = false;
            }
        }

        private Switcher switcher;
        private string[] temppath = new string[2];
        private Level level;

        private void LoadSwitcherObject(Switcher gswitcher)
        {
            if (gswitcher.Activated)
                checkBox1.Checked = true;
            if (gswitcher.Inversion)
                checkBox2.Checked = true;

            if (gswitcher.GameSprite.PathImage[0] != null)
                pictureBox1.Image = new Bitmap(gswitcher.GameSprite.PathImage[0]);

            if (gswitcher.GameSprite.PathImage.Length > 1)
              if (gswitcher.GameSprite.PathImage[1] != null )
                  pictureBox2.Image = new Bitmap(gswitcher.GameSprite.PathImage[1]);

            temppath[0] = gswitcher.GameSprite.PathImage[0];
            if (gswitcher.GameSprite.PathImage.Length > 1)
                temppath[1] = gswitcher.GameSprite.PathImage[1];
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

        private void Save(Switcher gswitcher)
        {
            gswitcher.Inversion = checkBox2.Checked;
            gswitcher.Activated = checkBox1.Checked;
            gswitcher.UserSizeImage = gswitcher.GameSprite.PathImage[0] != temppath[0] ? new SizeF(0, 0) : gswitcher.UserSizeImage;

            gswitcher.GameSprite.PathImage = temppath;
            //gswitcher.GameSprite.PathImage[0] = temppath[0];
            //gswitcher.GameSprite.PathImage[1] = temppath[1];

            ISprite isprite = gswitcher;
            isprite.LoadSprite();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = checkBox2.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save(switcher);
        }

        //private List<GameObject> CopyList(List<GameObject> listwithobjects)
        //{
        //    List<GameObject> templistewithobjects;

        //    using (FileStream fs = new FileStream("templistGameObjects.dat", FileMode.Create))
        //        (new BinaryFormatter()).Serialize(fs, listwithobjects);

        //    using (FileStream fs = new FileStream("templistGameObjects.dat", FileMode.Open))
        //        templistewithobjects =  (List<GameObject>)(new BinaryFormatter()).Deserialize(fs);

        //    File.Delete("templistGameObjects.dat");

        //    return templistewithobjects;
        //}

        private void ShowActivObjects(List<GameObject> gobjects)
        {
            listBox1.Items.Clear();

            foreach (GameObject go in gobjects)
                listBox1.Items.Add(go.Name);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Save(switcher);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new ChoiceGameObjectToAddListActivateForm(switcher.CanActivateObject, level).ShowDialog();
            ShowActivObjects(switcher.CanActivateObject);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new ChoiceGameObjectToAddListActivateForm(switcher.CanActivateObject, level).ShowDialog();
            ShowActivObjects(switcher.CanActivateObject);
        }
    }
}
