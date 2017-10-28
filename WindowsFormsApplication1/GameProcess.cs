using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class GameProcess : Form
    {
        public GameProcess(GameMenu gamemenu, Profile profile, string namelocation, Level level)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            Application.Idle += delegate 
            {
                pictureBox1.Invalidate();
            };

            this.gamemenu = gamemenu;
            this.profile = profile;
            this.level = level;
            this.namelocation = namelocation;

            controlball = new GameObject("ControlBall") { GameSprite = new Sprite(new string[] { "Content\\OtherObjects\\ControlBall.png" }) };

            LoadSprites(level);
            LoadParametersForm(level);

            backgrounds = new Bitmap[5];

            pichealth = new Bitmap(new Bitmap("Content\\ShowStatHealthAndScoreOfHero\\055.png"),new Size(80, 80));
            glass1 = new Bitmap(new Bitmap("Content\\ShowStatHealthAndScoreOfHero\\2.png"), new Size(200, 35));
            glass2 = new Bitmap(new Bitmap("Content\\ShowStatHealthAndScoreOfHero\\glass.png"), new Size(150, 25));
            score = new Bitmap(new Bitmap("Content\\ShowStatHealthAndScoreOfHero\\star-gold.png"), new Size(32, 32));
            //background = new Bitmap("Sprites\\123.png");

            backgrounds[0] = new Bitmap("Content\\BackgroundLoad\\1. Летняя поляна.png");
            backgrounds[1] = new Bitmap("Content\\BackgroundLoad\\2. Каньон.jpg");
            backgrounds[2] = new Bitmap("Content\\BackgroundLoad\\3. Зимняя поляна.jpg");
            backgrounds[3] = new Bitmap("Content\\BackgroundLoad\\4. Ночной лес.jpg");
            backgrounds[4] = new Bitmap("Content\\BackgroundLoad\\5. Загадочная земля.png");

            backgronddeath = new Bitmap("Content\\DeathHero\\HeroIsAlive.png");

            profile.Hero.ControlledBot = null;
        }

        private GameMenu gamemenu;
        private Profile profile;
        private string namelocation;
        private Level level;

        private GameObject controlball;
        private float currentspeedcontrolball = 20f;

        //Bitmap background;
        Bitmap[] backgrounds;
        Bitmap backgronddeath;
        Bitmap pichealth;
        Bitmap glass1;
        Bitmap glass2;
        Bitmap score;

        private int timetogame = 200;  //Время показа названия локации
        private List<GameObject> deletegameobject = new List<GameObject>();  //Объекты, которые необходимо удлаить
        public bool pause = false;  //Пауза игры
        private bool ppause = false;
        private DateTime lastupdate;  //Последнее время, у которого прошло изменение

        private List<GameObject> staticobjects = new List<GameObject>();
        private List<GameObject> drawingobjects = new List<GameObject>();

        private Keys UpKey;
        private bool right = true;
        private bool left = true;
        private float divtime = 50f;
        private bool _showCharacteristic = false;

        private void UpdateObjects(float dt)
        {
            left = true;
            right = true;

            for (int i = 0; i < level.GameObjects.Count; i++)
            {

                if (level.GameObjects[i].Name == "Hero")
                {
                    Character c = level.GameObjects[i] as Character;

                    if (c.ControlledBot != null ? !c.ControlledBot.Controled : true)
                        c.Anim.ChangeFrame(Keyboard.IsKeyDown(Keys.D) ? Direction.Right : Keyboard.IsKeyDown(Keys.A) ? Direction.Left : /*Keyboard.IsKeyDown(Keys.S) && */!c.PhysBody.Gravity ? Direction.Down : Direction.Idle, c.PhysBody.MaxSpeed, c.PhysBody.Gravity);

                    c.PhysBody.OnGround = false;
                    c.PhysBody.Gravity = true;

                    for (int j = 0; j < level.GameObjects.Count; j++)
                    {
                        if (c.Position.IntersectsWith(level.GameObjects[j].Position) && level.GameObjects[j].Name != "Hero")
                        {
                            if (level.GameObjects[j].Name == "Platform" || (level.GameObjects[j].Name == "Door" && ((level.GameObjects[j] is Door) ? !(level.GameObjects[j] as Door).Opened : false)))
                            {

                                if (((level.GameObjects[j].Position.Right + 1 > c.Position.Location.X) && (level.GameObjects[j].Position.Right - 8/* - 5 */<= c.Position.Location.X)) && (((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + c.Position.Height - 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + c.Position.Height - 4)) || ((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + 4)) || ((level.GameObjects[j].Position.Location.Y > c.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height <= c.Position.Location.Y + c.Position.Height - 4))))
                                {
                                    c.PhysBody.Velocity = new PointF(0, c.PhysBody.Velocity.Y);
                                    c.Position = new RectangleF(new PointF(level.GameObjects[j].Position.Right, c.Position.Y), c.Position.Size);
                                    left = false;
                                    continue;
                                }

                                if (((level.GameObjects[j].Position.Location.X - 2 < c.Position.Right + 1) && (level.GameObjects[j].Position.Location.X + 8 >= c.Position.Right)) && (((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + c.Position.Height - 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + c.Position.Height - 4)) || ((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + 4)) || ((level.GameObjects[j].Position.Location.Y >= c.Position.Location.Y + 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height <= c.Position.Location.Y + c.Position.Height - 4))))
                                {
                                    c.PhysBody.Velocity = new PointF(0, c.PhysBody.Velocity.Y);
                                    c.Position = new RectangleF(new PointF(level.GameObjects[j].Position.Left - c.Position.Width - 1, c.Position.Y), c.Position.Size);
                                    right = false;
                                    continue;
                                }

                                if ((c.Position.Bottom > level.GameObjects[j].Position.Top) && ((((c.Position.X > level.GameObjects[j].Position.X + 3) && (c.Position.X < level.GameObjects[j].Position.Right - 3)) || ((c.Position.Right > level.GameObjects[j].Position.X + 3) && (c.Position.Right < level.GameObjects[j].Position.Right - 3))) || ((c.Position.X >= level.GameObjects[j].Position.X + 3) && (c.Position.Right <= level.GameObjects[j].Position.Right - 3)) || ((c.Position.X <= level.GameObjects[j].Position.X + 3) && (c.Position.Right >= level.GameObjects[j].Position.Right - 3))))
                                {
                                    c.PhysBody.OnGroundCollision(level.GameObjects[j].Position.Top - c.Position.Height);
                                    c.PhysBody.OnGround = true;
                                }

                                if ((c.Position.Bottom >= level.GameObjects[j].Position.Bottom))
                                {
                                    c.Position = new RectangleF(new PointF(c.Position.X, level.GameObjects[j].Position.Bottom), c.Position.Size);
                                    c.PhysBody.Velocity = new PointF(c.PhysBody.Velocity.X, 4);
                                }

                                continue;
                            }

                            if (level.GameObjects[j].Name == "DamageObject")
                            {
                                DamageObject dob = level.GameObjects[j] as DamageObject;

                                if (dob.Cripple)
                                {
                                    profile.Hero.CurrentHealth -= (dob.Damage - (int)Math.Round(dob.Damage * profile.Hero.ResistanceToDamage));
                                }

                                if (dob.Destructible)
                                    deletegameobject.Add(level.GameObjects[j]);

                                if ((c.Position.Top < level.GameObjects[j].Position.Top))
                                {
                                    if (dob.Cripple)
                                        c.PhysBody.Jump(110f);
                                    c.PhysBody.OnGroundCollision(level.GameObjects[j].Position.Top - c.Position.Height);
                                    c.PhysBody.OnGround = true;
                                    continue;
                                }

                                if ((c.Position.Bottom > level.GameObjects[j].Position.Bottom))
                                {
                                    c.PhysBody.Velocity = new PointF(c.PhysBody.Velocity.X, 4);
                                    continue;
                                }

                                if (level.GameObjects[j].Position.Right > c.Position.Left)//(((level.GameObjects[j].Position.Location.X + level.GameObjects[j].Position.Width >= c.Position.Location.X) && (level.GameObjects[j].Position.Location.X + level.GameObjects[j].Position.Width - 5 <= c.Position.Location.X)) && (((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + c.Position.Height) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + c.Position.Height)) || ((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y)) || ((level.GameObjects[j].Position.Location.Y >= c.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height <= c.Position.Location.Y + c.Position.Height))))
                                {
                                    c.Position = new RectangleF(new PointF(level.GameObjects[j].Position.Right + 40, c.Position.Y), c.Position.Size);
                                    continue;
                                }

                                if (level.GameObjects[j].Position.Right < c.Position.Left)//(((level.GameObjects[j].Position.Location.X <= c.Position.Location.X + c.Position.Width) && (level.GameObjects[j].Position.Location.X + 5 >= c.Position.Location.X + c.Position.Width)) && (((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + c.Position.Height) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + c.Position.Height)) || ((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y)) || ((level.GameObjects[j].Position.Location.Y >= c.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height <= c.Position.Location.Y + c.Position.Height))))
                                {
                                    c.Position = new RectangleF(new PointF(level.GameObjects[j].Position.Left - c.Position.Width - 40, c.Position.Y), c.Position.Size);
                                    continue;
                                }
                            }

                            if (level.GameObjects[j].Name == "Bot")
                            {
                                Bot b = level.GameObjects[j] as Bot;

                                profile.Hero.CurrentHealth -= (b.Damage - (int)Math.Round(b.Damage * profile.Hero.ResistanceToDamage));

                                if ((c.Position.Top < level.GameObjects[j].Position.Top))
                                {
                                    c.PhysBody.Jump(90f);
                                    c.PhysBody.OnGroundCollision(level.GameObjects[j].Position.Top - c.Position.Height);
                                    c.PhysBody.OnGround = true;
                                    continue;
                                }

                                if ((c.Position.Bottom > level.GameObjects[j].Position.Bottom))
                                {
                                    c.PhysBody.Jump(90f);
                                    continue;
                                }

                                if (level.GameObjects[j].Position.Right > c.Position.Left)//(((level.GameObjects[j].Position.Location.X + level.GameObjects[j].Position.Width >= c.Position.Location.X) && (level.GameObjects[j].Position.Location.X + level.GameObjects[j].Position.Width - 5 <= c.Position.Location.X)) && (((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + c.Position.Height) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + c.Position.Height)) || ((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y)) || ((level.GameObjects[j].Position.Location.Y >= c.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height <= c.Position.Location.Y + c.Position.Height))))
                                {
                                    c.PhysBody.Jump(90f);
                                    continue;
                                }

                                if (level.GameObjects[j].Position.Right < c.Position.Left)//(((level.GameObjects[j].Position.Location.X <= c.Position.Location.X + c.Position.Width) && (level.GameObjects[j].Position.Location.X + 5 >= c.Position.Location.X + c.Position.Width)) && (((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + c.Position.Height) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + c.Position.Height)) || ((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y)) || ((level.GameObjects[j].Position.Location.Y >= c.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height <= c.Position.Location.Y + c.Position.Height))))
                                {
                                    c.PhysBody.Jump(90f);
                                    continue;
                                }
                            }

                            if (level.GameObjects[j].Name == "CheckPoint")
                            {
                                PointCheck pc = level.GameObjects[j] as PointCheck;
                                if (!pc.CheckSave)
                                    pc.SavePoint(profile, c, level);


                                continue;
                            }

                            if (level.GameObjects[j].Name == "ExitDoor")
                            {
                                pause = true;
                                Thread.Sleep(1000);
                                this.Dispose();
                                this.Close();
                                gamemenu.LevelComplete(c);
                                continue;
                            }

                            if (level.GameObjects[j].Name == "Ladder")
                            {
                                c.PhysBody.Gravity = false;


                                continue;
                            }

                            if (level.GameObjects[j].Name == "Switcher" && (UpKey == Keys.E))
                            {
                                Switcher s = level.GameObjects[j] as Switcher;
                                s.Activated = !s.Activated;
                                continue;
                            }

                            if (level.GameObjects[j] is Item)
                            {
                                Item item = level.GameObjects[j] as Item;
                                item.AddToCharacter(c, deletegameobject);

                                continue;
                            }

                            if (level.GameObjects[j].Name == "Chest" && (UpKey == Keys.E))
                            {
                                Chest chest = level.GameObjects[j] as Chest;
                                drawingobjects.AddRange(chest.Items);
                                chest.Open(level.GameObjects);


                                continue;
                            }

                            if (level.GameObjects[j].Name == "SwitcherPassword" && (UpKey == Keys.E))
                            {
                                SwitcherPassword switcherpassword = level.GameObjects[j] as SwitcherPassword;
                                if (!switcherpassword.Activated)
                                    pause = true;
                                UpKey = 0;
                                switcherpassword.TryOn(this);


                                continue;
                            }

                            if (level.GameObjects[j].Name == "Trigger")
                            {
                                Trigger t = level.GameObjects[j] as Trigger;
                                if (!t.Activated)
                                    t.Activated = true;


                                continue;
                            }

                            if (level.GameObjects[j].Name == "Shop" && (UpKey == Keys.E))
                            {
                                UpKey = 0;
                                Shop shop = level.GameObjects[j] as Shop;

                                //pause = true;

                                shop.ToShop(c, this);
                                lastupdate = DateTime.MinValue;

                                continue;
                            }


                            if (level.GameObjects[j].Name == "Teleport" && (UpKey == Keys.E))
                            {
                                Teleport t = level.GameObjects[j] as Teleport;
                                UpKey = 0;

                                if (t.Teleport2 != null)
                                    t.ToTeleport2(c);

                                continue;
                            }
                        }
                    }

                    if (!c.PhysBody.OnGround)
                        c.PhysBody.UpdatePositionY(dt);
                }

                if (level.GameObjects[i].Name == "Bot")
                {
                    Bot c = level.GameObjects[i] as Bot;

                    if (!c.Controled)
                       c.Anim.ChangeFrame(c.Right ? Direction.Right : Direction.Left, c.PhysBody.MaxSpeed, c.PhysBody.Gravity);
                    else
                        c.Anim.ChangeFrame(Keyboard.IsKeyDown(Keys.D) ? Direction.Right : Keyboard.IsKeyDown(Keys.A) ? Direction.Left : /*Keyboard.IsKeyDown(Keys.S) && */!c.PhysBody.Gravity ? Direction.Down : Direction.Idle, c.PhysBody.MaxSpeed, c.PhysBody.Gravity);

                    if (c.Controled)
                    {
                        if (Keyboard.IsKeyDown(Keys.W) && CheckGroung(c)/*&& profile.SaveGame.Hero.PhysBody.OnGround*/ )
                        {
                            profile.Hero.ControlledBot.PhysBody.Jump();
                            profile.Hero.ControlledBot.PhysBody.OnGround = false;
                        }

                        if (Keyboard.IsKeyDown(Keys.A) && left)
                        {
                            profile.Hero.ControlledBot.PhysBody.Left();
                            right = true;
                        }

                        if (Keyboard.IsKeyDown(Keys.D) && right)
                        {
                            profile.Hero.ControlledBot.PhysBody.Right();
                            left = true;
                        }
                    }
                    else
                    {
                        if (c.Right)
                            c.PhysBody.Right();
                        else
                            c.PhysBody.Left();
                    }

                    c.PhysBody.UpdatePositionX(dt);

                    c.PhysBody.OnGround = false;
                    c.PhysBody.Gravity = true;

                    for (int j = 0; j < level.GameObjects.Count; j++)
                    {
                        if (c.Position.IntersectsWith(level.GameObjects[j].Position) && level.GameObjects[j] != level.GameObjects[i])
                        {
                            if (level.GameObjects[j].Name == "Platform" || (level.GameObjects[j].Name == "Door" && ((level.GameObjects[j] is Door) ? !(level.GameObjects[j] as Door).Opened : false)))
                            {
                                if (((level.GameObjects[j].Position.Right + 1 /* <--- clear*/> c.Position.Location.X) && (level.GameObjects[j].Position.Right - 8/* - 5 */<= c.Position.Location.X)) && (((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + c.Position.Height - 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + c.Position.Height - 4)) || ((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + 4)) || ((level.GameObjects[j].Position.Location.Y > c.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height <= c.Position.Location.Y + c.Position.Height - 4))))
                                {
                                    c.Position = new RectangleF(new PointF(level.GameObjects[j].Position.Right, c.Position.Y), c.Position.Size);
                                    c.Right = true;
                                    if (!c.Controled)
                                       left = false;
                                    continue;
                                }

                                if (((level.GameObjects[j].Position.Location.X - 2/* */ < c.Position.Right + 1) && (level.GameObjects[j].Position.Location.X + 8 >= c.Position.Right)) && (((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + c.Position.Height - 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + c.Position.Height - 4)) || ((level.GameObjects[j].Position.Location.Y < c.Position.Location.Y + 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > c.Position.Location.Y + 4)) || ((level.GameObjects[j].Position.Location.Y >= c.Position.Location.Y + 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height <= c.Position.Location.Y + c.Position.Height - 4))))
                                {
                                    c.Position = new RectangleF(new PointF(level.GameObjects[j].Position.Left - c.Position.Width - 1, c.Position.Y), c.Position.Size);
                                    c.Right = false;
                                    if (!c.Controled)
                                        right = false;
                                    continue;
                                }

                                if ((c.Position.Bottom > level.GameObjects[j].Position.Top) && ((((c.Position.X > level.GameObjects[j].Position.X + 3) && (c.Position.X < level.GameObjects[j].Position.Right - 3)) || ((c.Position.Right > level.GameObjects[j].Position.X + 3) && (c.Position.Right < level.GameObjects[j].Position.Right - 3))) || ((c.Position.X >= level.GameObjects[j].Position.X + 3) && (c.Position.Right <= level.GameObjects[j].Position.Right - 3)) || ((c.Position.X <= level.GameObjects[j].Position.X + 3) && (c.Position.Right >= level.GameObjects[j].Position.Right - 3))))
                                {
                                    c.PhysBody.OnGroundCollision(level.GameObjects[j].Position.Top - c.Position.Height);
                                    c.PhysBody.OnGround = true;

                                    if (level.GameObjects[j].Position.X > c.Position.X)
                                        c.Right = true;
                                    if (level.GameObjects[j].Position.Right < c.Position.Right)
                                        c.Right = false;
                                }

                                if ((c.Position.Bottom >= level.GameObjects[j].Position.Bottom))
                                {
                                    c.Position = new RectangleF(new PointF(c.Position.X, level.GameObjects[j].Position.Bottom), c.Position.Size);
                                    c.PhysBody.Velocity = new PointF(c.PhysBody.Velocity.X, 4);
                                }

                                continue;
                            }

                            if (level.GameObjects[j].Name == "ControlBall")
                            {
                                if (profile.Hero.LevelControlBot >= c.LevelControl)
                                    profile.Hero.ControlledBot = (Bot)level.GameObjects[i];

                                deletegameobject.Add(level.GameObjects[j]);
                            }

                            if (c.Controled && level.GameObjects[j].Name == "Switcher" && (UpKey == Keys.E))
                            {
                                Switcher s = level.GameObjects[j] as Switcher;
                                s.Activated = !s.Activated;
                            }

                            if (c.Controled && level.GameObjects[j].Name == "SwitcherPassword" && (UpKey == Keys.E))
                            {
                                pause = true;
                                SwitcherPassword switcherpassword = level.GameObjects[j] as SwitcherPassword;
                                UpKey = 0;
                                switcherpassword.TryOn(this);
                            }
                        }
                    }

                    if (!c.PhysBody.OnGround)
                        c.PhysBody.UpdatePositionY(dt);
                }

                if (level.GameObjects[i].Name == "ControlBall")
                {
                    bool removed = false;

                    for (int j = 0; j < level.GameObjects.Count; j++)
                    {
                        if (level.GameObjects[i].Position.IntersectsWith(level.GameObjects[j].Position) && level.GameObjects[j] != level.GameObjects[i])
                        {
                            if (level.GameObjects[j].Name == "Platform" || level.GameObjects[j].Name == "Ladder" || level.GameObjects[j].Name == "Door")
                            {
                                deletegameobject.Add(level.GameObjects[i]);
                                removed = true;
                                break;
                            }
                        }
                    }

                    if (!removed)
                        level.GameObjects[i].Position = new RectangleF(new PointF(level.GameObjects[i].Position.X + dt * currentspeedcontrolball, level.GameObjects[i].Position.Y), level.GameObjects[i].Position.Size); 
                }

                if (level.GameObjects[i] is Item)
                {
                    Item item = level.GameObjects[i] as Item;
                    item.PhysBody.OnGround = false;

                    for (int j = 0; j < level.GameObjects.Count; j++)
                    {
                        if (item.Position.IntersectsWith(level.GameObjects[j].Position) && level.GameObjects[j] != item)
                        {
                            if (level.GameObjects[j].Name == "Platform" || (level.GameObjects[j].Name == "Door" && ((level.GameObjects[j] is Door) ? !(level.GameObjects[j] as Door).Opened : false)))
                            {
                                if (((item.Position.Bottom >= level.GameObjects[j].Position.Y) /*&& (item.Position.Bottom < level.GameObjects[j].Position.Bottom)) && ((((item.Position.X > level.GameObjects[j].Position.X) && (item.Position.X < level.GameObjects[j].Position.Right)) || ((item.Position.Right > level.GameObjects[j].Position.X) && (item.Position.Right < level.GameObjects[j].Position.Right))) || ((item.Position.X >= level.GameObjects[j].Position.X) && (item.Position.Right <= level.GameObjects[j].Position.Right)) || ((item.Position.X <= level.GameObjects[j].Position.X) && (item.Position.Right >= level.GameObjects[j].Position.Right)))*/))
                                {
                                    item.PhysBody.OnGroundCollision(level.GameObjects[j].Position.Top - item.Position.Height);
                                    item.PhysBody.OnGround = true;
                                }

                                if (((level.GameObjects[j].Position.Location.X + level.GameObjects[j].Position.Width > item.Position.Location.X) && (level.GameObjects[j].Position.Location.X + level.GameObjects[j].Position.Width - 5 <= item.Position.Location.X)) && (((level.GameObjects[j].Position.Location.Y < item.Position.Location.Y + item.Position.Height - 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > item.Position.Location.Y + item.Position.Height - 4)) || ((level.GameObjects[j].Position.Location.Y < item.Position.Location.Y + 4) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > item.Position.Location.Y + 4)) || ((level.GameObjects[j].Position.Location.Y > item.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height <= item.Position.Location.Y + item.Position.Height - 4))))
                                {
                                    item.Position = new RectangleF(new PointF(level.GameObjects[j].Position.Right, item.Position.Y), item.Position.Size);
                                }

                                if (((level.GameObjects[j].Position.Location.X < item.Position.Location.X + item.Position.Width + 1) && (level.GameObjects[j].Position.Location.X + 5 >= item.Position.Location.X + item.Position.Width)) && (((level.GameObjects[j].Position.Location.Y < item.Position.Location.Y + item.Position.Height) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > item.Position.Location.Y + item.Position.Height)) || ((level.GameObjects[j].Position.Location.Y < item.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height > item.Position.Location.Y)) || ((level.GameObjects[j].Position.Location.Y >= item.Position.Location.Y) && (level.GameObjects[j].Position.Location.Y + level.GameObjects[j].Position.Height <= item.Position.Location.Y + item.Position.Height))))
                                {
                                    item.Position = new RectangleF(new PointF(level.GameObjects[j].Position.Left - item.Position.Width - 1, item.Position.Y), item.Position.Size);                                    //  continue;
                                }

                                if ((item.Position.Bottom >= level.GameObjects[j].Position.Bottom))
                                {
                                    item.Position = new RectangleF(new PointF(item.Position.X, level.GameObjects[j].Position.Bottom), item.Position.Size);
                                    item.PhysBody.Velocity = new PointF(item.PhysBody.Velocity.X, 4);
                                }

                                continue;
                            }
                        }
                    }

                    if (!item.PhysBody.OnGround)
                        item.PhysBody.UpdatePositionY(dt);
                }
            }

            while (deletegameobject.Count != 0)
            {
                level.GameObjects.Remove(deletegameobject[0]);
                drawingobjects.Remove(deletegameobject[0]);
                deletegameobject.RemoveAt(0);
            }
        }

        private new void Update()
        {
            var now = DateTime.Now;
            if (pause)
            {
                lastupdate = DateTime.MinValue; //DateTime.MinValue;
                ppause = true;
                return;
            }

            //var now = DateTime.Now;
            var deltaTime = 0f;

            if (ppause)
            {
                deltaTime = (float)0.1000f;
                ppause = false;
            }
            else
                deltaTime = (float)(now - lastupdate).TotalMilliseconds / divtime;

            
            label1.Text = deltaTime.ToString();
            //
            if (lastupdate != DateTime.MinValue)
            {
                UpdateInput();
                CheckDeath(profile.Hero, gamemenu);
                profile.Hero.PhysBody.UpdatePositionX(/*0.05000f */deltaTime > 0.500000f? 0.20000f: deltaTime /*0.10000f*/);
                UpdateObjects(/*0.05000f */deltaTime > 0.500000f ? 0.20000f : deltaTime/*0.10000f*/);
                UpKey = 0;
            }
            lastupdate = now;
        }

        private void UpdateInput()
        {
            if (pause || (profile.Hero.ControlledBot != null ? profile.Hero.ControlledBot.Controled : false))
                return;

            profile.Hero.PhysBody.Velocity = new PointF(0, !profile.Hero.PhysBody.Gravity? 0: profile.Hero.PhysBody.Velocity.Y);

            if (Keyboard.IsKeyDown(Keys.W) && CheckGroung()/*&& profile.SaveGame.Hero.PhysBody.OnGround*/ )
            {
                profile.Hero.PhysBody.Jump();
                profile.Hero.PhysBody.OnGround = false;
            }

            if (Keyboard.IsKeyDown(Keys.W) &&  !profile.Hero.PhysBody.Gravity)
            {
                profile.Hero.PhysBody.Up();
            }

            if (Keyboard.IsKeyDown(Keys.S) && !profile.Hero.PhysBody.Gravity)
            {
                profile.Hero.PhysBody.Down();
            }

            if (Keyboard.IsKeyDown(Keys.A) && left)
            {
                profile.Hero.PhysBody.Left();

                if (currentspeedcontrolball > 0)
                    currentspeedcontrolball *= -1;
            }

            if (Keyboard.IsKeyDown(Keys.D) && right)
            {
                profile.Hero.PhysBody.Right();
                if (currentspeedcontrolball < 0)
                    currentspeedcontrolball *= -1;
            }

            if (Keyboard.IsKeyDown(Keys.Space)) divtime = 150f;
            else
                divtime = 50f;

            if (Keyboard.IsKeyDown(Keys.T) && (level.GameObjects.IndexOf(controlball) == -1))
            {
                controlball.Position = new RectangleF(profile.Hero.Position.Location, controlball.Position.Size);
                level.GameObjects.Add(controlball);
                drawingobjects.Add(controlball);
            }
        }

        private bool CheckGroung()
        {
            for (int i = 0; i < level.GameObjects.Count; i++)
            {
                if (profile.Hero.Position.IntersectsWith(level.GameObjects[i].Position))
                {
                    if (level.GameObjects[i].Name == "Platform")
                    {
                        if ((profile.Hero.Position.Top < level.GameObjects[i].Position.Top))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool CheckGroung(GameObject g)
        {
            for (int i = 0; i < level.GameObjects.Count; i++)
            {
                if (g.Position.IntersectsWith(level.GameObjects[i].Position))
                {
                    if (level.GameObjects[i].Name == "Platform")
                    {
                        if ((g.Position.Top < level.GameObjects[i].Position.Top))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void DrawBegin(Graphics g)  //Показ названия уровня
        {
            g.DrawImage(profile.CurrentLocAndLev.CurrentLocation == 0? backgrounds[0] : profile.CurrentLocAndLev.CurrentLocation == 1 ? backgrounds[1] : profile.CurrentLocAndLev.CurrentLocation == 2 ? backgrounds[2] : profile.CurrentLocAndLev.CurrentLocation == 3 ? backgrounds[3] : profile.CurrentLocAndLev.CurrentLocation == 4 ? backgrounds[4] : WindowsFormsApplication1.Properties.Resources._1__Летняя_поляна, new RectangleF(new PointF(0, 0), new Size(1386,788)));
            //g.FillRectangle(Brushes.White, new RectangleF(new Point(0, DesktopBounds.Height / 2 - 150), new SizeF(DesktopBounds.Width, 150)));
            g.FillRectangle(Brushes.White, new RectangleF(new Point(0, 788 / 2 - 150), new SizeF(1386, 150)));
            //g.DrawString(namelocation, new Font("Microsoft Sans Serif", 50), new SolidBrush(Color.Green), new PointF(DesktopBounds.Width / 2 - 210, DesktopBounds.Height / 2 - 150));
            g.DrawString(namelocation, new Font("Microsoft Sans Serif", 50), new SolidBrush(Color.Green), new PointF(1386 / 2 - 210, 788 / 2 - 150));
            //g.DrawString("Уровень " + (profile.CurrentLocAndLev.CurrentLevel + 1).ToString(), new Font("Microsoft Sans Serif", 50), new SolidBrush(Color.LightSkyBlue), new PointF(DesktopBounds.Width / 2 - 170, DesktopBounds.Height / 2 - 80));
            g.DrawString("Уровень " + (profile.CurrentLocAndLev.CurrentLevel + 1).ToString(), new Font("Microsoft Sans Serif", 50), new SolidBrush(Color.LightSkyBlue), new PointF(1386 / 2 - 170, 788 / 2 - 80));
            //g.FillRectangle(Brushes.White, new RectangleF(new Point(0, DesktopBounds.Height / 2 + DesktopBounds.Height / 3), new SizeF(DesktopBounds.Width, 85)));
            g.FillRectangle(Brushes.White, new RectangleF(new Point(0, 788 / 2 + 788 / 3), new SizeF(1386, 85)));
            //g.DrawString("Загрузка...", new Font("Microsoft Sans Serif", 50), new SolidBrush(Color.Black), new PointF(DesktopBounds.Width / 2 - 170, DesktopBounds.Height / 2 + DesktopBounds.Height / 3));
            g.DrawString("Загрузка...", new Font("Microsoft Sans Serif", 50), new SolidBrush(Color.Black), new PointF(1386 / 2 - 170, 788 / 2 + 788 / 3));
        }

        private void DrawHealth(Graphics g)
        {
            //  g.DrawString(profile.Hero.Coins.ToString(), new Font("Microsoft Sans Serif", 30), Brushes.Yellow, 0,40);
            g.FillRectangle(Brushes.Red, new RectangleF(new PointF(25, 45), new Size((150 * profile.Hero.CurrentHealth) / profile.Hero.MaxHealth, 25)));
            g.DrawImage(glass2, new Point(25, 45));
            g.DrawImage(glass1,new Point(10, 40));
            g.DrawImage(pichealth,new Point(0,0));
            g.DrawImage(score, new Point(80, 10));

            g.DrawString(profile.Hero.Life.ToString(), new Font("Microsoft Sans Serif", 30), Brushes.White, 6, 17);
            g.DrawString("x " + profile.Hero.Coins.ToString(), new Font("Microsoft Sans Serif", 20), Brushes.White, 117, 11);

            //g.DrawRectangle(new Pen(Color.Black, 0.5f), new Rectangle(new Point(30, 20), new Size(150, 25)));
        }

        private void DrawObject(Graphics g, List<GameObject> GObjects, List<GameObject> BackGObjects)  //Рисует объекты на форме
        {
            foreach (GameObject go in drawingobjects)
                if (go.GameSprite.Image != null)
                {
                    if (go.Name == "Bot")
                    {
                        Bot b = go as Bot;

                        g.DrawImage(go.GameSprite.Image, go.Position);

                        if (_showCharacteristic)
                        {
                            g.DrawRectangle(Pens.Black, b.Position.X + 9, b.Position.Y - 22, 12, 18);
                            g.DrawString(b.LevelControl.ToString(), new Font("Book Antiqua", 10f), Brushes.Black, new PointF(b.Position.X + 10, b.Position.Y - 20));
                        }
                    }
                    else
                        g.DrawImage(go.GameSprite.Image, go.Position);
                }
            try
            {
                g.DrawImage(profile.Hero.GameSprite.Image, profile.Hero.Position);
            }
            catch { }

            if (_showCharacteristic)
            {
                g.DrawRectangle(Pens.Black, profile.Hero.Position.X + 9, profile.Hero.Position.Y - 22, 12, 18);
                g.DrawString(profile.Hero.LevelControlBot.ToString(), new Font("Book Antiqua", 10f), Brushes.Black, new PointF(profile.Hero.Position.X + 10, profile.Hero.Position.Y - 20));
            }
        }

        private void LoadSprites(Level lev)  //Загружает спрайты 
        {
            ISprite isprite;

            foreach (GameObject go in lev.GameObjects)
            {
                isprite = go;
                isprite.LoadSprite();
            }

            foreach (GameObject go in lev.Background)
            {
                isprite = go;
                isprite.LoadSprite();
            }

            isprite = profile.Hero;
            isprite.LoadSprite();

            isprite = controlball;
            controlball.UserSizeImage = new SizeF(16, 16);
            isprite.LoadSprite();
            

            lev.GameObjects.Insert(0,profile.Hero);
        }

        private void LoadParametersForm(Level l)
        {
            pictureBox1.Size = l.FieldSize;
            pictureBox1.Location = new Point(this.DesktopBounds.Width / 2 - pictureBox1.Width / 2, this.DesktopBounds.Height / 2 - pictureBox1.Height / 2);

            foreach (GameObject go in l.Background)
                staticobjects.Add(go);

            GameObject tempgo = null;

            List<GameObject> tlist = new List<GameObject>();

            foreach (GameObject go in l.GameObjects)
            {
                if (go.Name == "Platform" || go.Name == "Ladder")
                    staticobjects.Add(go);
                else
                {
                    if (go.Name != "Hero" && go.Name != "Trigger")
                        drawingobjects.Add(go);
                    else if (go.Name == "Bot")
                        tlist.Add(go);
                    else if (go.Name != "Trigger")
                        tempgo = go;
                }
            }

            drawingobjects.Add(tempgo);

            foreach (GameObject go in tlist)
                drawingobjects.Add(go);

            

            Bitmap bground = new Bitmap(1386, 788);

            Graphics g = Graphics.FromImage(bground);

            g.Clear(Color.LightSkyBlue);

            foreach (GameObject go in staticobjects)
                g.DrawImage(go.GameSprite.Image, go.Position);

            pictureBox1.Image = bground;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (timetogame >= 0)
            {
                timetogame -= 1;
                if (!(profile.Hero.CurrentHealth <= 0))
                   DrawBegin(e.Graphics);
                else
                    e.Graphics.DrawImage(backgronddeath, new RectangleF(new PointF(0, 0), backgronddeath.Size));
                return;
            }

            Update();
            DrawObject(e.Graphics, level.GameObjects, level.Background);
            DrawHealth(e.Graphics);

            if (profile.Hero.CurrentHealth <= 0 && (profile.Hero.Life - 1 != 0))
                timetogame = 200;
        }

        private void GameProcess_KeyUp(object sender, KeyEventArgs e)
        {
            UpKey = e.KeyCode;

            if (e.KeyCode == Keys.Q)
                _showCharacteristic = !_showCharacteristic;

            if (e.KeyCode == Keys.P)
                pause = !pause;

            if (e.KeyCode == Keys.Escape)
            {
                pause = true;
                new GamePause(this, gamemenu).ShowDialog();
                pause = false;
            }

            if (e.KeyCode == Keys.C)
                if (profile.Hero.ControlledBot != null)
                    if (profile.Hero.ControlledBot.LevelControl <= profile.Hero.LevelControlBot)
                        profile.Hero.ControlledBot.Controled = !profile.Hero.ControlledBot.Controled;
        }

        private void CheckDeath(Character ch, GameMenu gmenu)
        {
            if (ch.CurrentHealth <= 0)
            {
                if (ch.Life - 1 <= 0)
                {
                    profile.SaveGame.CheckPoint = null;
                    this.Close();
                    gmenu.GameOver();
                }
                else
                {
                    gmenu.LoadLevel(profile.CurrentLocAndLev.CurrentLocation, profile.CurrentLocAndLev.CurrentLevel, true);
                    this.Close();
                }

                this.Dispose();
            }
        }
    }
}