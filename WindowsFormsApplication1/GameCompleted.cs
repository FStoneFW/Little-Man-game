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
    public partial class GameCompleted : Form
    {
        public GameCompleted(GameMenu gm, Profile p)
        {
            InitializeComponent();
            this.gm = gm;

            this.label1.Text = p.StatisticGame.CountCoins.ToString();
        }

        GameMenu gm;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            gm.Visible = true;
        }
    }
}
