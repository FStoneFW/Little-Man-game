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

namespace WindowsFormsApplication1
{
    public partial class ChoiceProfile : Form
    {
        public ChoiceProfile()
        {
            InitializeComponent();
            profiles = LoadProfiles();
            ShowProfiles();
        }
        private List<Profile> profiles;

        public Profile SelectedProfile { get; set; }


        private List<Profile> LoadProfiles()  //Загрузка профилей
        {
            List<Profile> tempProfiles = new List<Profile>();

            if (!Directory.Exists("Profiles"))
                Directory.CreateDirectory("Profiles");

            try
            {
                string[] pathprofiles = Directory.GetFiles("Profiles");
                BinaryFormatter bf = new BinaryFormatter();

                for (int i = 0; i < pathprofiles.Length; i++)
                {
                    using (FileStream fs = new FileStream(pathprofiles[i], FileMode.Open))
                    {
                        tempProfiles.Add((Profile)bf.Deserialize(fs));
                    }
                }

                return tempProfiles;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при загрузки профилей!");
                return new List<Profile>();
            }
        }

        private void CreateProfile(string nickname)  //Создание профиля
        {
            using (FileStream fs = new FileStream(string.Format("Profiles\\{0}.dat", nickname), FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Profile p = new Profile(nickname);
                bf.Serialize(fs, p);
                profiles.Add(p);
            }
        }

        private void ShowProfiles()  //Показать все профиля
        {
            listBox1.Items.Clear();

            foreach (Profile p in profiles)
            {
                listBox1.Items.Add(p.Nickname);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox1.Visible = true;
            pictureBox1.Visible = false;
            pictureBox2.Enabled = false;
            pictureBox2.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            SelectedProfile = profiles[listBox1.SelectedIndex];
            new GameMenu(this).Show();
            this.Visible = false;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите свой Nickname!");
                return;
            }

            pictureBox1.Visible = true;
            pictureBox2.Enabled = true;
            pictureBox2.Visible = true;

            groupBox1.Enabled = false;
            groupBox1.Visible = false;

            listBox1.Items.Clear();
            CreateProfile(textBox1.Text);
            ShowProfiles();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            groupBox1.Enabled = false;
            groupBox1.Visible = false;
            pictureBox2.Enabled = true;
            pictureBox2.Visible = true;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо выбрать профиль!");
                return;
            }

            if (MessageBox.Show("Вы точно хотите удалить профиль?", "Удаление профиля", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            File.Delete(@"Profiles\\" + profiles[listBox1.SelectedIndex].Nickname + ".dat");
            profiles.RemoveAt(listBox1.SelectedIndex);
            ShowProfiles();
        }
    }
}
