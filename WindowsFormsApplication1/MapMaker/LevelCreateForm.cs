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
    public partial class LevelCreateForm : Form
    {
        public LevelCreateForm(MapMakerForm mapmakerform)
        {
            InitializeComponent();
            this.mapmakerform = mapmakerform;

            ShowAllLocations(mapmakerform.locations, listBox1);
        }

        MapMakerForm mapmakerform;

        private void ShowAllLocations(List<Location> gamelocations, ListBox list)
        {
            foreach (Location l in gamelocations)
                list.Items.Add(l.Name);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                mapmakerform.locations[listBox1.SelectedIndex].Levels.Add(new Level());
                mapmakerform.locations[listBox1.SelectedIndex].Levels[mapmakerform.locations[listBox1.SelectedIndex].Levels.Count - 1].FieldSize = mapmakerform.pictureBox1.Size;
            }
        }
    }
}
