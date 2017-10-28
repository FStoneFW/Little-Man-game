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
    public partial class SwitcherPasswordPropForm : Form
    {
        public SwitcherPasswordPropForm(SwitcherPassword switcherpassword, Level level)
        {
            InitializeComponent();
            this.switcherpassword = switcherpassword;
            this.level = level;
            LoadSwitcherObject(switcherpassword);
            ShowActivObjects(switcherpassword.CanActivateObject);
        }


        private SwitcherPassword switcherpassword;
        private string[] temppath = new string[2];
        private Level level;

        private void LoadSwitcherObject(SwitcherPassword gswitcherp)
        {
            textBox1.Text = gswitcherp.Password;

            if (gswitcherp.Activated)
                checkBox1.Checked = true;
            if (gswitcherp.Inversion)
                checkBox2.Checked = true;

            if (gswitcherp.GameSprite.PathImage[0] != null)
                pictureBox1.Image = new Bitmap(gswitcherp.GameSprite.PathImage[0]);

            if (gswitcherp.GameSprite.PathImage[1] != null)
                pictureBox2.Image = new Bitmap(gswitcherp.GameSprite.PathImage[1]);

            temppath[0] = gswitcherp.GameSprite.PathImage[0];
            temppath[1] = gswitcherp.GameSprite.PathImage[1];
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

        private void Save(SwitcherPassword gswitcherp)
        {
            gswitcherp.Inversion = checkBox2.Checked;
            gswitcherp.Activated = checkBox1.Checked;

            gswitcherp.UserSizeImage = gswitcherp.GameSprite.PathImage[0] != temppath[0] ? new SizeF(0, 0) : gswitcherp.UserSizeImage;

            gswitcherp.GameSprite.PathImage[0] = temppath[0];
            gswitcherp.GameSprite.PathImage[1] = temppath[1];
            gswitcherp.Password = textBox1.Text;

            ISprite isprite = gswitcherp;
            isprite.LoadSprite();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = checkBox2.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save(switcherpassword);
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

        private void button2_Click(object sender, EventArgs e)
        {
            new ChoiceGameObjectToAddListActivateForm(switcherpassword.CanActivateObject, level).ShowDialog();
            ShowActivObjects(switcherpassword.CanActivateObject);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new ChoiceGameObjectToAddListActivateForm(switcherpassword.CanActivateObject, level).ShowDialog();
            ShowActivObjects(switcherpassword.CanActivateObject);
        }
    }
}
