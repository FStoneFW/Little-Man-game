using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class GameLevelComplete : Form
    {
        public GameLevelComplete(GameMenu gm, Statistic s, Profile p)
        {
            InitializeComponent();

            this.s = s;
            this.gm = gm;
            this.p = p;

            pictureBox1.Focus();

            LoadStatistic(s);
        }

        Statistic s;
        GameMenu gm;
        Profile p;


        private void LoadStatistic(Statistic st)
        {
            label1.Text = st.CountCoins.ToString();
            label2.Text = st.SpentCoins.ToString();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            gm.Visible = true;
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            gm.LoadLevel(p.CurrentLocAndLev.CurrentLocation, p.CurrentLocAndLev.CurrentLevel, false);
        }
    }
}
