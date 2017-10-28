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
    public partial class PanelGameObjectsForm : Form
    {
        public PanelGameObjectsForm(MapMakerForm mapmakerform)
        {
            InitializeComponent();
            this.mapmakerform = mapmakerform;
            LoadPathesOfPictures(pathes);
        }

        private MapMakerForm mapmakerform;
        private List<string[]> pathes = new List<string[]>();

        private void pictureBox_Click(object sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            if (e.Button == MouseButtons.Left)
            {
                if ((string)pb.Tag == "PointSpawn")
                    mapmakerform.choicegameobject = new GameObject("PointSpawn") { Position = new RectangleF(new PointF(), new Bitmap(pathes[0][0]).Size), GameSprite = new Sprite(pathes[0]) };

                if ((string)pb.Tag == "Platform")
                    mapmakerform.choicegameobject = new GameObject("Platform") { Position = new RectangleF(new PointF(), new Bitmap(pathes[1][0]).Size), GameSprite = new Sprite(pathes[1]) };

                if ((string)pb.Tag == "ExitDoor")
                    mapmakerform.choicegameobject = new GameObject("ExitDoor") { Position = new RectangleF(new PointF(), new Bitmap(pathes[2][0]).Size), GameSprite = new Sprite(pathes[2]) };

                if ((string)pb.Tag == "DamageObject")
                    mapmakerform.choicegameobject = new DamageObject("DamageObject", false, true, 20) { Position = new RectangleF(new PointF(), new Bitmap(pathes[3][0]).Size), GameSprite = new Sprite(pathes[3]) };

                if ((string)pb.Tag == "CheckPoint")
                    mapmakerform.choicegameobject = new PointCheck("CheckPoint") { Position = new RectangleF(new PointF(), new Bitmap(pathes[4][0]).Size), GameSprite = new Sprite(pathes[4]) };

                if ((string)pb.Tag == "Switcher")
                    mapmakerform.choicegameobject = new Switcher("Switcher") { Position = new RectangleF(new PointF(), new Bitmap(pathes[5][0]).Size), GameSprite = new Sprite(pathes[5]) };

                if ((string)pb.Tag == "SwitcherPassword")
                    mapmakerform.choicegameobject = new SwitcherPassword("SwitcherPassword") { Position = new RectangleF(new PointF(), new Bitmap(pathes[6][0]).Size), GameSprite = new Sprite(pathes[6]) };

                if ((string)pb.Tag == "Door")
                    mapmakerform.choicegameobject = new Door("Door") { Position = new RectangleF(new PointF(), new Bitmap(pathes[7][0]).Size), GameSprite = new Sprite(pathes[7]) };

                if ((string)pb.Tag == "Chest")
                    mapmakerform.choicegameobject = new Chest("Chest") { Position = new RectangleF(new PointF(), new Bitmap(pathes[8][0]).Size), GameSprite = new Sprite(pathes[8]) };

                if ((string)pb.Tag == "Life")
                    mapmakerform.choicegameobject = new Item("Life") { Position = new RectangleF(new PointF(), new Bitmap(pathes[9][0]).Size), GameSprite = new Sprite(pathes[9]) };

                if ((string)pb.Tag == "Health")
                    mapmakerform.choicegameobject = new Item("Health") { Position = new RectangleF(new PointF(), new Bitmap(pathes[10][0]).Size), GameSprite = new Sprite(pathes[10]) };

                if ((string)pb.Tag == "Coin")
                    mapmakerform.choicegameobject = new Item("Coin") { Position = new RectangleF(new PointF(), new Bitmap(pathes[11][0]).Size), GameSprite = new Sprite(pathes[11]) };

                if ((string)pb.Tag == "Control")
                    mapmakerform.choicegameobject = new Item("Control") { Position = new RectangleF(new PointF(), new Bitmap(pathes[12][0]).Size), GameSprite = new Sprite(pathes[12]) };

                if ((string)pb.Tag == "Ladder")
                    mapmakerform.choicegameobject = new GameObject("Ladder") { Position = new RectangleF(new PointF(), new Bitmap(pathes[13][0]).Size), GameSprite = new Sprite(pathes[13]) };

                if ((string)pb.Tag == "Shop")
                    mapmakerform.choicegameobject = new Shop("Shop") { Position = new RectangleF(new PointF(), new Bitmap(pathes[14][0]).Size), GameSprite = new Sprite(pathes[14]) };

                if ((string)pb.Tag == "Background")
                    mapmakerform.choicegameobject = new GameObject("Background") { Position = new RectangleF(new PointF(), new Bitmap(pathes[15][0]).Size), GameSprite = new Sprite(pathes[15]) };

                if ((string)pb.Tag == "Bot")
                    mapmakerform.choicegameobject = new Bot("Bot") { Position = new RectangleF(new PointF(), new Bitmap(pathes[16][0]).Size), GameSprite = new Sprite(pathes[16]) };

                if ((string)pb.Tag == "Trigger")
                    mapmakerform.choicegameobject = new Trigger("Trigger") { Position = new RectangleF(new PointF(), new Bitmap(pathes[17][0]).Size), GameSprite = new Sprite(pathes[17]) };

                if ((string)pb.Tag == "Teleport")
                    mapmakerform.choicegameobject = new Teleport("Teleport") { Position = new RectangleF(new PointF(), new Bitmap(pathes[18][0]).Size), GameSprite = new Sprite(pathes[18]) };
                //mapmakerform.choicegameobject = new Bot("Bot") { new GameObject("Background") { Position = new RectangleF(new PointF(), new Bitmap(pathes[15][0]).Size), GameSprite = new Sprite(pathes[15])  }

                ISprite isprite = mapmakerform.choicegameobject;
                isprite.LoadSprite();
            }
            else
            {

            }
        }

        private void LoadPathesOfPictures(List<string[]> ps)
        {
            pathes.Add(new string[] { "Content\\OtherObjects\\Spawn_point.png" });  
            pathes.Add(new string[] { "Content\\Ground\\1\\0\\3.png" }); 
            pathes.Add(new string[] { "Content\\OtherObjects\\Exit_new.png" });  
            pathes.Add(new string[] { "Content\\DamageObjects\\Koluchki\\3.png", null}); 
            pathes.Add(new string[] { "Content\\OtherObjects\\Checkpoint\\Check_off.png", "Content\\OtherObjects\\Checkpoint\\Check2.png" }); 
            pathes.Add(new string[] { "Content\\OtherObjects\\Switcher\\Switcher_Off.png", "Content\\OtherObjects\\Switcher\\Switcher_On.png" }); 
            pathes.Add(new string[] { "Content\\OtherObjects\\Switcher_Password\\Switcher_password_off.png", "Content\\OtherObjects\\Switcher_Password\\Switcher_password_on.png" });
            pathes.Add(new string[] { "Content\\OtherObjects\\Door\\Door_close.png", "Content\\OtherObjects\\Door\\Door_open.png" });
            pathes.Add(new string[] { "Content\\OtherObjects\\Chest\\Chest2.png", "Content\\OtherObjects\\Chest\\1.png" });

            pathes.Add(new string[] { "Content\\OtherObjects\\Life.png" });
            pathes.Add(new string[] { "Content\\OtherObjects\\Health.png" });
            pathes.Add(new string[] { "Content\\OtherObjects\\Money.png" });
            pathes.Add(new string[] { "Content\\OtherObjects\\ControlBall.png" });

            pathes.Add(new string[] { "Content\\OtherObjects\\Ladder\\1.png" });
            pathes.Add(new string[] { "Content\\OtherObjects\\Shop\\Shop_off.png", "Content\\OtherObjects\\Shop\\Shop_on.png" });
            pathes.Add(new string[] { "Content\\BackgroundLoad\\1. Летняя поляна.png" });
            pathes.Add(new string[] { "Content\\Bots\\MakerFirstBot.png" });
            pathes.Add(new string[] { "Content\\OtherObjects\\Trigger.png", null });
            pathes.Add(new string[] { "Content\\OtherObjects\\Teleport.png" });
        }
    }
}
