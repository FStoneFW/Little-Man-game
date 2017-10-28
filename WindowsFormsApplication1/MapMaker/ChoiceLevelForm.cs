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
    public partial class ChoiceLevelForm : Form
    {
        public ChoiceLevelForm(MapMakerForm mapmakerform)
        {
            InitializeComponent();
            this.mapmakerform = mapmakerform;
            this.FormClosing += delegate (object sender, FormClosingEventArgs e) { mapmakerform.Invalidate(); };
            LoadLocations(mapmakerform.locations);
        }

        MapMakerForm mapmakerform;

        private void LoadLocations(List<Location> loc)
        {
            foreach (Location l in loc)
                comboBox1.Items.Add(l.Name);
        }

        private void LoadLevel(Location l)
        {
            if (comboBox1.SelectedIndex == -1)
                return;

            comboBox2.Items.Clear();

            for (int i = 0; i < l.Levels.Count; i++)
                comboBox2.Items.Add(i + 1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLevel(mapmakerform.locations[comboBox1.SelectedIndex]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
            {
                //if (mm.currentLevel != null && mm.Spawn != null)
                //{
                //    mm.currentLevel.SpawnHero = new PointF(mm.Spawn.Position.X, mm.Spawn.Position.Y);
                //}

                mapmakerform.currentlevel = mapmakerform.locations[comboBox1.SelectedIndex].Levels[comboBox2.SelectedIndex];

                ISprite isprite;

                foreach (GameObject go in mapmakerform.currentlevel.GameObjects)
                {
                    isprite = go;
                    isprite.LoadSprite();
                }

                /*
                if (mm.currentLevel.SpawnHero != PointF.Empty)
                    mm.Spawn = new GameObject("SpawnHero", new RectangleF(new PointF(mm.currentLevel.SpawnHero.X, mm.currentLevel.SpawnHero.Y), new Bitmap("SpawnHero.png").Size));*/

                //mapmakerform.Invalidate();

                mapmakerform.toolStripTextBox1.Text = mapmakerform.currentlevel.FieldSize.Width.ToString() + "," + mapmakerform.currentlevel.FieldSize.Height.ToString();
                mapmakerform.pictureBox1.Invalidate();
                this.Close();
            }
        }
    }
}
