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
    public partial class GameRating : Form
    {
        public GameRating(string nickname)
        {
            InitializeComponent();
            profiles = LoadProfiles();

            profiles.Sort();
            this.nickname = nickname;


            LoadBest(profiles);
        }

        List<Profile> profiles;
        int position;
        string nickname;

        private List<Profile> LoadProfiles()  //Загрузка профилей
        {
            List<Profile> tempProfiles = new List<Profile>();

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
                MessageBox.Show("Произошла ошибка!");
                return new List<Profile>();
            }
        }

        private void LoadBest(List<Profile> p)
        {
            for(int i = 0; i < p.Count; i++)
            {
                if (p[i].Nickname == nickname)
                    position = i;

                listBox1.Items.Add(string.Format("#{0} {1}", i + 1, p[i].Nickname));
                listBox2.Items.Add(string.Format("{0}", p[i].StatisticGame.CountCoins));
            }

            listBox1.SelectedIndex = position;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.SelectedIndex = listBox1.SelectedIndex;
        }
    }
}
