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
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1.MapMaker
{
    public partial class MapMakerForm : Form
    {
        public MapMakerForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            _panelgameobjectsform = new PanelGameObjectsForm(this);  //Создаём панель игровых объектов
            _panelgameobjectsform.Show();
 
            locations = new List<WindowsFormsApplication1.Location>();
        }

        private PanelGameObjectsForm _panelgameobjectsform;  //панель с объектами
        private bool _clickedleftmouse;  //нажата ли мышь
        private GameObject _selectedgameobject;  //выбранный объект на редакторе
        private Point _temppoint;

        private Point MP;
        private Point p;

        public GameObject choicegameobject;  //Выбранный объект в панеле игровых объектов
        public List<Location> locations;  //загруженные или созданные локации  (Будет создаваться локация)
        public Level currentlevel;  //выбранный уровень (Будет присваиваться выбранный уровень)

        #region Save And Load
        private void LoadLocations(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    locations = (List<Location>)bf.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                locations = new List<WindowsFormsApplication1.Location>();
            }
        }
        private void SaveLocations(string path)
        {
            Regex theReg = new Regex(@"Content");

            for (int i = 0; i < locations.Count; i++)
                for (int j = 0; j < locations[i].Levels.Count; j++)
                    for (int q = 0; q < locations[i].Levels[j].GameObjects.Count; q++)
                    {
                        if (locations[i].Levels[j].GameObjects[q].GameSprite.PathImage != null)
                        {
                            if (locations[i].Levels[j].GameObjects[q].GameSprite.PathImage[0] != null)
                            {
                                Match m = theReg.Match(locations[i].Levels[j].GameObjects[q].GameSprite.PathImage[0]);
                                if (m.Index != 0)
                                    locations[i].Levels[j].GameObjects[q].GameSprite.PathImage[0] = locations[i].Levels[j].GameObjects[q].GameSprite.PathImage[0].Remove(0, m.Index);

                                if (locations[i].Levels[j].GameObjects[q].GameSprite.PathImage.Length == 2)
                                    if (locations[i].Levels[j].GameObjects[q].GameSprite.PathImage[1] != null)
                                    {
                                        m = theReg.Match(locations[i].Levels[j].GameObjects[q].GameSprite.PathImage[1]);
                                        if (m.Index != 0)
                                            locations[i].Levels[j].GameObjects[q].GameSprite.PathImage[1] = locations[i].Levels[j].GameObjects[q].GameSprite.PathImage[1].Remove(0, m.Index);

                                    }
                            }
                        }
                    }

            for (int i = 0; i < locations.Count; i++)
                for (int j = 0; j < locations[i].Levels.Count; j++)
                    for (int q = 0; q < locations[i].Levels[j].Background.Count; q++)
                    {
                        Match m = theReg.Match(locations[i].Levels[j].Background[q].GameSprite.PathImage[0]);
                        if (m.Index != 0)
                            locations[i].Levels[j].Background[q].GameSprite.PathImage[0] = locations[i].Levels[j].Background[q].GameSprite.PathImage[0].Remove(0, m.Index);

                    }

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, locations);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region MenuItems
        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadLocations(openFileDialog1.FileName);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentlevel == null)
                return;

            if (currentlevel.SpawnHero == null)
                currentlevel.SpawnHero = new GameObject("BeginPoint") { Position = new RectangleF(new PointF(0, 0), new SizeF(0, 0)) };

            currentlevel.FieldSize = pictureBox1.Size;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveLocations(saveFileDialog1.FileName);
            }
        }

        private void локацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new LocationCreateForm(this).ShowDialog();
        }

        private void уровеньToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new LevelCreateForm(this).ShowDialog();
        }

        private void выбратьУровеньToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedgameobject = null;
            new ChoiceLevelForm(this).ShowDialog();

            if (currentlevel != null)
                LoadSprites(currentlevel);
        }
        #endregion

        #region ContexMenuStripMethods
        private void свойстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedgameobject == null)
                return;

            if (_selectedgameobject is DamageObject)
                new PropertiesObject.DamageObjectPropForm((DamageObject)_selectedgameobject).ShowDialog();

            if (_selectedgameobject.Name == "Platform" || _selectedgameobject.Name == "ExitDoor")
                new PropertiesObject.PropForm(_selectedgameobject).ShowDialog();

            if (_selectedgameobject.Name == "CheckPoint")
                new PropertiesObject.TwoStatePropForm(_selectedgameobject).ShowDialog();

            if (_selectedgameobject.Name == "Switcher")
                new PropertiesObject.SwitcherPropForm((Switcher)_selectedgameobject, currentlevel).ShowDialog();

            if (_selectedgameobject.Name == "SwitcherPassword")
                new PropertiesObject.SwitcherPasswordPropForm((SwitcherPassword)_selectedgameobject, currentlevel).ShowDialog();

            if (_selectedgameobject.Name == "Door")
                new PropertiesObject.DoorPropForm((Door)_selectedgameobject).ShowDialog();

            if (_selectedgameobject.Name == "Chest")
                new PropertiesObject.ChestPropForm((Chest)_selectedgameobject).ShowDialog();

            if (_selectedgameobject is Item)
                new PropertiesObject.ItemPropForm((Item)_selectedgameobject).ShowDialog();

            if (_selectedgameobject.Name == "Ladder")
                new PropertiesObject.PropForm(_selectedgameobject).ShowDialog();

            if (_selectedgameobject.Name == "Shop")
                new PropertiesObject.ShopPropForm((Shop)_selectedgameobject).ShowDialog();

            if (_selectedgameobject.Name == "Background")
                new PropertiesObject.PropForm(_selectedgameobject).ShowDialog();

            if (_selectedgameobject.Name == "Bot")
                new PropertiesObject.BotPropForm((Bot)_selectedgameobject).ShowDialog();

            if (_selectedgameobject.Name == "Trigger")
                new PropertiesObject.SwitcherPropForm((Trigger)_selectedgameobject, currentlevel).ShowDialog(); //PropertiesObject.BotPropForm((Bot)_selectedgameobject).ShowDialog();

            if (_selectedgameobject.Name == "Teleport")
                new PropertiesObject.TeleportPropForm((Teleport)_selectedgameobject, currentlevel).ShowDialog();

            //this.Invalidate();
            pictureBox1.Invalidate();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region EventsMouse
        private void MapMakerForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _clickedleftmouse = true;

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
                ClickMouse(new Point(e.X, e.Y), e.Button);
            _temppoint = e.Location;

            MP = PointToClient(MousePosition);
            
        }

        private void MapMakerForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_clickedleftmouse && _selectedgameobject != null)
            {
                p = new Point(MP.X - (int)_selectedgameobject.Position.X, MP.Y - (int)_selectedgameobject.Position.Y);
                MP = PointToClient(MousePosition);
                Point d = new Point(MP.X - p.X, MP.Y - p.Y);
                //_selectedgameobject.Position = new RectangleF(new PointF(e.X - _selectedgameobject.Position.Width /2, e.Y- _selectedgameobject.Position.Height / 2)/* new PointF(e.X, e.Y)*/, _selectedgameobject.Position.Size);
                _selectedgameobject.Position = new RectangleF(new PointF(d.X,d.Y), _selectedgameobject.Position.Size);
                //this.Invalidate();
                // _selectedgameobject.Position = new RectangleF(new PointF(e.X - _tempsize.Width, e.Y - (e.Y - _tempsize.Height))/* new PointF(e.X, e.Y)*/, _selectedgameobject.Position.Size);
                pictureBox1.Invalidate();
            }
        }

        private void MapMakerForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _clickedleftmouse = false;
        }
        #endregion

        private void ClickMouse(Point p, MouseButtons mb)
        {
            if (currentlevel == null)
                return;

            if (mb == MouseButtons.Middle)
            {
                if (choicegameobject != null)//(pgof.SelectNewObject != null && currentLevel != null)
                {
                    if (choicegameobject.Name == "PointSpawn")
                    {
                        if (currentlevel.SpawnHero == null)
                            currentlevel.SpawnHero = choicegameobject;
                    }
                    else if (choicegameobject.Name == "ExitDoor")
                    {
                        foreach (GameObject g in currentlevel.GameObjects)
                            if (g.Name == "ExitDoor")
                                return;

                        currentlevel.GameObjects.Add(choicegameobject);
                    }
                    else if (choicegameobject.Name == "Background")
                    {
                        currentlevel.Background.Add(GetCopyObject(choicegameobject));
                        currentlevel.Background[currentlevel.Background.Count - 1].Position = new RectangleF(new PointF(p.X, p.Y), choicegameobject.Position.Size);
                    }
                    else
                    {
                        if (choicegameobject.Name == "Bot")
                            currentlevel.GameObjects.Insert(0, GetCopyObject(choicegameobject));
                        else
                            currentlevel.GameObjects.Add(GetCopyObject(choicegameobject));
                        currentlevel.GameObjects[currentlevel.GameObjects.Count - 1].Position = new RectangleF(new PointF(p.X, p.Y), choicegameobject.Position.Size);
                    }

                    choicegameobject.Position = new RectangleF(new PointF(p.X, p.Y), choicegameobject.Position.Size);
                }
            }
            else if (mb == MouseButtons.Left)
            {
                bool finded = false;
                for (int i = 0; i < currentlevel.GameObjects.Count; i++)//foreach (GameObject go in currentlevel.GameObjects)
                {
                    if (currentlevel.GameObjects[i].Position.Contains(p.X, p.Y))
                    {
                        if (Keyboard.IsKeyDown(Keys.C))
                        {
                            if (_selectedgameobject == null)
                                return;

                            SizeF tempob = currentlevel.GameObjects[i].Position.Size;

                            if (_selectedgameobject.Name == "Bot")
                            {
                                _selectedgameobject = GetCopyObject(currentlevel.GameObjects[i]);
                                _selectedgameobject.Position = new RectangleF(new PointF(currentlevel.GameObjects[i].Position.X, currentlevel.GameObjects[i].Position.Y), tempob);
                                currentlevel.GameObjects.Insert(0,_selectedgameobject);
                            }
                            else
                            {
                                _selectedgameobject = GetCopyObject(currentlevel.GameObjects[i]);
                                _selectedgameobject.Position = new RectangleF(new PointF(currentlevel.GameObjects[i].Position.X, currentlevel.GameObjects[i].Position.Y), tempob);
                                currentlevel.GameObjects.Add(_selectedgameobject);
                            }
                        }
                        else
                            _selectedgameobject = currentlevel.GameObjects[i];

                        finded = true;
                        break;
                    }
                }

                if (!finded)
                {
                    for (int i = currentlevel.Background.Count - 1; i >= 0; i--)//foreach (GameObject go in currentlevel.GameObjects)
                    {
                        if (currentlevel.Background[i].Position.Contains(p.X, p.Y))
                        {
                            if (Keyboard.IsKeyDown(Keys.C))
                            {
                                SizeF tempob = currentlevel.Background[i].Position.Size;
                                _selectedgameobject = GetCopyObject(currentlevel.Background[i]);
                                _selectedgameobject.Position = new RectangleF(new PointF(currentlevel.Background[i].Position.X, currentlevel.Background[i].Position.Y), tempob);
                                currentlevel.Background.Add(_selectedgameobject);
                            }
                            else
                                _selectedgameobject = currentlevel.Background[i];

                            finded = true;
                            break;
                        }
                    }
                }

                if (!finded)
                    _selectedgameobject = null;

                if (currentlevel.SpawnHero != null)
                {
                    if (currentlevel.SpawnHero.Position.Contains(p.X, p.Y))
                    {
                        _selectedgameobject = currentlevel.SpawnHero;
                    }
                }
            }
            else if (mb == MouseButtons.Right)
            {
                //bool finded = false;
                //foreach (GameObject go in currentlevel.GameObjects)
                //{
                //    if (go.Position.Contains(p.X, p.Y))
                //    {
                //        _selectedgameobject = go;
                //        contextMenuStrip1.Show(p.X + this.Location.X, p.Y + this.Location.Y);
                //        //contextMenuStrip1.Show(p.)
                //        finded = true;
                //        break;
                //    }
                //}

                //if (!finded)
                //    _selectedgameobject = null;

                bool finded = false;
                for (int i = 0; i < currentlevel.GameObjects.Count; i++)//foreach (GameObject go in currentlevel.GameObjects)
                {
                    if (currentlevel.GameObjects[i].Position.Contains(p.X, p.Y))
                    {
                        _selectedgameobject = currentlevel.GameObjects[i];
                        contextMenuStrip1.Show(p.X + this.Location.X, p.Y + this.Location.Y);
                        finded = true;
                        break;
                    }
                }

                if (!finded)
                {
                    for (int i = currentlevel.Background.Count - 1; i > 0; i--)//foreach (GameObject go in currentlevel.GameObjects)
                    {
                        if (currentlevel.Background[i].Position.Contains(p.X, p.Y))
                        {
                            _selectedgameobject = currentlevel.Background[i];
                            contextMenuStrip1.Show(p.X + this.Location.X, p.Y + this.Location.Y);
                            finded = true;
                            break;
                        }
                    }
                }

                if (!finded)
                    _selectedgameobject = null;
            }

            //this.Invalidate();
            pictureBox1.Invalidate();
        }

        private void UpdateDrawLevel(Level level, Graphics g)
        {
            for (int i = 0; i < level.Background.Count; i++)
                if (level.Background[i].GameSprite.Image != null)
                    g.DrawImage(level.Background[i].GameSprite.Image, level.Background[i].Position);

            for (int i = 0; i < level.GameObjects.Count; i++)
                if (level.GameObjects[i].GameSprite.Image != null)
                    g.DrawImage(level.GameObjects[i].GameSprite.Image, level.GameObjects[i].Position);

            if (level.SpawnHero != null && level.SpawnHero.GameSprite != null)
                g.DrawImage(level.SpawnHero.GameSprite.Image, level.SpawnHero.Position);
        }

        private GameObject GetCopyObject(GameObject go)
        {
            GameObject tempgobject;

            using (FileStream fs = new FileStream("tempObject.dat", FileMode.Create))
                (new BinaryFormatter()).Serialize(fs, go);

            using (FileStream fs = new FileStream("tempObject.dat", FileMode.Open))
                tempgobject = (GameObject)(new BinaryFormatter()).Deserialize(fs);

            File.Delete("tempObject.dat");

            ISprite isprite = tempgobject;
            isprite.LoadSprite();  //Загружаем спрайт

            return tempgobject;
        }

        private void DeleteObject(object sender, KeyEventArgs e)
        {
            if (currentlevel != null && _selectedgameobject != null && e.KeyCode == Keys.Delete)
            {
                if (_selectedgameobject.Name == "SpawnHero")
                    currentlevel.SpawnHero = null;
                else if (_selectedgameobject.Name == "Background")
                    currentlevel.Background.Remove(_selectedgameobject);
                else
                    currentlevel.GameObjects.Remove(_selectedgameobject);

                _selectedgameobject = null;
                //this.Invalidate();
                pictureBox1.Invalidate();
            }
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    if (currentlevel == null)
        //        return;

        //    UpdateDrawLevel(currentlevel, e.Graphics);

        //    if (_selectedgameobject != null)
        //        e.Graphics.DrawRectangle(new Pen(Color.LightSkyBlue, 0.2f), new Rectangle((int)_selectedgameobject.Position.X - 2, (int)_selectedgameobject.Position.Y - 2, (int)_selectedgameobject.Position.Width + 3, (int)_selectedgameobject.Position.Height + 3));
        //}

        private void MapMakerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _panelgameobjectsform.Close();
        }

        private void LoadSprites(Level level)
        {
            foreach(GameObject g in level.GameObjects)
            {
                ISprite isprite = g;
                isprite.LoadSprite();
            }

            foreach (GameObject g in level.Background)
            {
                ISprite isprite = g;
                isprite.LoadSprite();
            }

            if (level.SpawnHero != null && level.SpawnHero.GameSprite != null)
            {
                ISprite isprite = level.SpawnHero;
                isprite.LoadSprite();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (currentlevel == null)
                return;

            UpdateDrawLevel(currentlevel, e.Graphics);

            if (_selectedgameobject != null)
                e.Graphics.DrawRectangle(new Pen(Color.LightSkyBlue, 0.2f), new Rectangle((int)_selectedgameobject.Position.X - 2, (int)_selectedgameobject.Position.Y - 2, (int)_selectedgameobject.Position.Width + 3, (int)_selectedgameobject.Position.Height + 3));
        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            string[] temps = toolStripTextBox1.Text.Split(',');
            pictureBox1.Size = new Size(Convert.ToInt32(temps[0]), Convert.ToInt32(temps[1]));
            this.Focus();
        }

        private void MapMakerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad6)
                if (_selectedgameobject != null)
                {
                    _selectedgameobject.Position = new RectangleF(_selectedgameobject.Position.Location, new SizeF(_selectedgameobject.Position.Width + 5, _selectedgameobject.Position.Height));
                    _selectedgameobject.UserSizeImage = _selectedgameobject.Position.Size;
                }
                    

            if (e.KeyCode == Keys.NumPad2)
                if (_selectedgameobject != null)
                {
                    _selectedgameobject.Position = new RectangleF(_selectedgameobject.Position.Location, new SizeF(_selectedgameobject.Position.Width, _selectedgameobject.Position.Height + 5));
                    _selectedgameobject.UserSizeImage = _selectedgameobject.Position.Size;
                }
                   

            if (e.KeyCode == Keys.NumPad8)
                if (_selectedgameobject != null)
                {
                    _selectedgameobject.Position = new RectangleF(_selectedgameobject.Position.Location, new SizeF(_selectedgameobject.Position.Width, _selectedgameobject.Position.Height - 5));
                    _selectedgameobject.UserSizeImage = _selectedgameobject.Position.Size;
                }


            if (e.KeyCode == Keys.NumPad4)
                if (_selectedgameobject != null)
                {
                    _selectedgameobject.Position = new RectangleF(_selectedgameobject.Position.Location, new SizeF(_selectedgameobject.Position.Width - 5, _selectedgameobject.Position.Height));
                    _selectedgameobject.UserSizeImage = _selectedgameobject.Position.Size;
                }

            if (e.KeyCode == Keys.PageUp)
                if (_selectedgameobject != null)
                {
                    //List<GameObject> tempmem;

                    if (currentlevel.GameObjects.IndexOf(_selectedgameobject) != -1)
                    {
                        currentlevel.GameObjects.Remove(_selectedgameobject);
                        currentlevel.GameObjects.Add(_selectedgameobject);
                    }
                    else
                    {
                        currentlevel.Background.Remove(_selectedgameobject);
                        currentlevel.Background.Add(_selectedgameobject);
                    }

                    //currentlevel.GameObjects.Remove(_selectedgameobject);
                    //currentlevel.GameObjects.Add(_selectedgameobject);
                }

            if (e.KeyCode == Keys.PageDown)
                if (_selectedgameobject != null)
                {
                    if (currentlevel.GameObjects.IndexOf(_selectedgameobject) != -1)
                    {
                        currentlevel.GameObjects.Remove(_selectedgameobject);
                        currentlevel.GameObjects.Insert(0, _selectedgameobject);
                    }
                    else
                    {
                        currentlevel.Background.Remove(_selectedgameobject);
                        currentlevel.Background.Insert(0, _selectedgameobject);
                    }
                    //currentlevel.GameObjects.Remove(_selectedgameobject);
                    //currentlevel.GameObjects.Insert(0,_selectedgameobject);
                }

            pictureBox1.Invalidate();
        }
    }
}