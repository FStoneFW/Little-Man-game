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
    public partial class GameOver : Form
    {
        public GameOver(GameMenu gm)
        {
            InitializeComponent();
            this.gm = gm;
        }

        GameMenu gm;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            gm.Visible = true;
        }
    }
}
