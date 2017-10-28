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
    public partial class ChoiceGameObjectToAddListActivateForm : Form
    {
        public ChoiceGameObjectToAddListActivateForm(List<GameObject> gameobjects, Level currentlevel)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            this.gameobjects = gameobjects;
            this.currentlevel = currentlevel;
        }

        private GameObject _selectedgameobject;  //выбранный объект на редакторе
        private Level currentlevel;  //выбранный уровень (Будет присваиваться выбранный уровень)
        private List<GameObject> gameobjects;

        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateDrawLevel(currentlevel, e.Graphics);

            //if (_selectedgameobject != null)
            //    e.Graphics.DrawRectangle(new Pen(Color.LightSkyBlue, 0.2f), new Rectangle((int)_selectedgameobject.Position.X - 2, (int)_selectedgameobject.Position.Y - 2, (int)_selectedgameobject.Position.Width + 3, (int)_selectedgameobject.Position.Height + 3));
            //else
            //{
            //    e.Graphics.DrawRectangle(new Pen(Color.Red, 0.2f), new Rectangle((int)_selectedgameobject.Position.X - 2, (int)_selectedgameobject.Position.Y - 2, (int)_selectedgameobject.Position.Width + 3, (int)_selectedgameobject.Position.Height + 3));
            //    e.Graphics.DrawLine(Pens.Red, _selectedgameobject.Position.X, _selectedgameobject.Position.Y, _selectedgameobject.Position.Right, _selectedgameobject.Position.Bottom);
            //    e.Graphics.DrawLine(Pens.Red, _selectedgameobject.Position.Right, _selectedgameobject.Position.Y, _selectedgameobject.Position.X, _selectedgameobject.Position.Bottom);
            //}
            if (_selectedgameobject != null)
            {
                IState istate;
                try
                {
                    istate = (IState)_selectedgameobject;
                    e.Graphics.DrawRectangle(new Pen(Color.Green, 2f), new Rectangle((int)_selectedgameobject.Position.X - 2, (int)_selectedgameobject.Position.Y - 2, (int)_selectedgameobject.Position.Width + 5, (int)_selectedgameobject.Position.Height + 5));
                }
                catch
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 0.2f), new Rectangle((int)_selectedgameobject.Position.X - 2, (int)_selectedgameobject.Position.Y - 2, (int)_selectedgameobject.Position.Width + 3, (int)_selectedgameobject.Position.Height + 3));
                    e.Graphics.DrawLine(Pens.Red, _selectedgameobject.Position.X, _selectedgameobject.Position.Y, _selectedgameobject.Position.Right, _selectedgameobject.Position.Bottom);
                    e.Graphics.DrawLine(Pens.Red, _selectedgameobject.Position.Right, _selectedgameobject.Position.Y, _selectedgameobject.Position.X, _selectedgameobject.Position.Bottom);
                }
            }
        }

        private void UpdateDrawLevel(Level level, Graphics g)
        {
            for (int i = 0; i < level.GameObjects.Count; i++)
                if (level.GameObjects[i].GameSprite.Image != null)
                {
                    g.DrawImage(level.GameObjects[i].GameSprite.Image, level.GameObjects[i].Position);
                    if (gameobjects.IndexOf(level.GameObjects[i]) != -1)
                        g.DrawRectangle(new Pen(Color.Green, 2f), new Rectangle((int)level.GameObjects[i].Position.X - 2, (int)level.GameObjects[i].Position.Y - 2, (int)level.GameObjects[i].Position.Width + 5, (int)level.GameObjects[i].Position.Height + 5));
                    //g.DrawRectangle(new Pen(Color.Green, 2f), new Rectangle((int)level.GameObjects[i].Position.X - 5, (int)level.GameObjects[i].Position.Y - 5, (int)level.GameObjects[i].Position.Width + 6, (int)level.GameObjects[i].Position.Height + 6));
                }

            if (level.SpawnHero != null)
            {
                g.DrawImage(level.SpawnHero.GameSprite.Image, level.SpawnHero.Position);
            }
        }

        private void ClickMouse(Point p, MouseButtons mb)
        {
            if (mb == MouseButtons.Left)
            {
                foreach (GameObject go in currentlevel.GameObjects)
                {
                    if (go.Position.Contains(p.X, p.Y))
                    {
                        try
                        {
                            IState istate = (IState)go;
                            _selectedgameobject = go;
                            if (gameobjects.IndexOf(_selectedgameobject) == -1)
                                gameobjects.Add(_selectedgameobject);
                        }
                        catch
                        {
                            _selectedgameobject = go;
                            break;
                        }

                        //IState istate = (IState)go;
                        //if (istate == null)
                        //    break;
                        //_selectedgameobject = go;
                        //if (gameobjects.IndexOf(_selectedgameobject) != -1)
                        //    gameobjects.Add(_selectedgameobject);
                        //finded = true;
                        //break;
                    }
                }

                //if (!finded)
                //    _selectedgameobject = null;
            }
            else if (mb == MouseButtons.Right)
            {
                bool finded = false;
                foreach (GameObject go in currentlevel.GameObjects)
                {
                    if (go.Position.Contains(p.X, p.Y))
                    {
                        _selectedgameobject = go;
                        //contextMenuStrip1.Show(p.X + this.Location.X, p.Y + this.Location.Y);
                        //contextMenuStrip1.Show(p.)

                        if (gameobjects.IndexOf(_selectedgameobject) != -1)
                            gameobjects.Remove(_selectedgameobject);
                        break;
                        //finded = true;
                        //break;
                    }
                }

                if (!finded)
                    _selectedgameobject = null;
            }

            this.Invalidate();
        }

        private void ChoiceGameObjectToAddListActivateForm_MouseUp(object sender, MouseEventArgs e)
        {
            ClickMouse(e.Location, e.Button);
        }

    }
}
