using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1.MapMaker
{
    public partial class LocationCreateForm : Form
    {
        public LocationCreateForm(MapMakerForm mapmakerform)
        {
            InitializeComponent();
            this.mapmakerform = mapmakerform;
        }

        MapMakerForm mapmakerform;

        private void button1_Click(object sender, EventArgs e)
        {
            mapmakerform.locations.Add(new WindowsFormsApplication1.Location(textBox1.Text));
            this.Close();
        }
    }
}
