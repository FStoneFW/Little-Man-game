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
    public partial class InputPasswordForm : Form
    {
        public InputPasswordForm(SwitcherPassword spassword, GameProcess gameprocess)
        {
            InitializeComponent();
            this.spassword = spassword;
            this.gameprocess = gameprocess;
        }

        GameProcess gameprocess;
        SwitcherPassword spassword;

        private void button1_Click(object sender, EventArgs e)
        {
            PictureBox b = sender as PictureBox;

            if (b.Name == "pictureBox9")
                textBox1.Text += "1";
            if (b.Name == "pictureBox10")
                textBox1.Text += "2";
            if (b.Name == "pictureBox11")
                textBox1.Text += "3";
            if (b.Name == "pictureBox6")
                textBox1.Text += "4";
            if (b.Name == "pictureBox7")
                textBox1.Text += "5";
            if (b.Name == "pictureBox8")
                textBox1.Text += "6";
            if (b.Name == "pictureBox5")
                textBox1.Text += "7";
            if (b.Name == "pictureBox4")
                textBox1.Text += "8";
            if (b.Name == "pictureBox3")
                textBox1.Text += "9";
            if (b.Name == "pictureBox2")
                textBox1.Text += "0";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (spassword.CheckPassword(textBox1.Text))
                this.Close();
        }

        private void InputPasswordForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            gameprocess.pause = false;
        }
    }
}
