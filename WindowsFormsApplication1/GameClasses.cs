using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Media;

namespace WindowsFormsApplication1
{
    public enum ItemShop
    {
        Life,
        Health,
        Control,
        OpportunityControl,
        Protection,
        UpHealth
    }

    public enum Direction
    {
        Idle,
        Left,
        Right,
        Down
    }

    public interface IState
    {
        void StateOn(bool activ, bool inversion);
    }

    public interface ISprite
    {
        void LoadSprite();
    }

        
    [Serializable]
    public class Sprite
    {
        [NonSerialized]
        private Bitmap _image;

        public Bitmap Image
        {
            get { return _image; }
            set { _image = value; }
        }
        public string[] PathImage { get; set; }

        public Sprite() { }

        public Sprite(string[] pathimage)
        {
            PathImage = pathimage;
        }
    }
    
    #region Profile
    [Serializable]
    public class Profile: IComparable  //Профиль
    {
        public string Nickname { get; set; }  //никнем игрока
        public Statistic StatisticGame { get; set; }  //Статистика игры
        public CurrentLocationAndLevel CurrentLocAndLev { get; set; }  //Текущая локация и уровень
        public Save SaveGame { get; set; }  //Сохранение игры (Персонаж и сам уровень)
        public Character Hero { get; set; }  //Текущее состояние персонажа
        public Profile(string Nickname)
        {
            this.Nickname = Nickname;
            StatisticGame = new Statistic();
            CurrentLocAndLev = new CurrentLocationAndLevel();

            /////////////////////////////!!!!!!!!!!!!!!!!!!!!!!!!!!!!///////////////////
            CurrentLocAndLev.CurrentLocation = 0;
            CurrentLocAndLev.CurrentLevel = 0;
            /////////////////////////////!!!!!!!!!!!!!!!!!!!!!!!!!!!!///////////////////

            SaveGame = new Save();
            Hero = new Character();
        }

        int IComparable.CompareTo(object obj)
        {
            Profile p = obj as Profile;

            if (this.StatisticGame.CountCoins > p.StatisticGame.CountCoins)
                return -1;
            else if (this.StatisticGame.CountCoins < p.StatisticGame.CountCoins)
                return 1;
            else
                return 0;
        }
    }

    [Serializable]
    public class Statistic
    {
        public int CountCoins { get; set; }  //Количество всего собранных монет
        public int SpentCoins { get; set; }  //Количество всего потраченных
        public int TimePassingSeconds { get; set; } //Время, затраченное на прохождение

        public void Add(Statistic s)  //Обновляем статистику
        {
            CountCoins += s.CountCoins;
            SpentCoins += s.SpentCoins;
            TimePassingSeconds += s.TimePassingSeconds;
        }
    }

    [Serializable]
    public class CurrentLocationAndLevel
    {
        public int CurrentLocation { get; set; }
        public int CurrentLevel { get; set; }
    }

    [Serializable]
    public class Save
    {
        public Character Hero { get; set; }  //состояние персонажа во время игры
        public Level CheckPoint { get; set; }

        public Save()
        {
            Hero = new Character();
        }
    }

    [Serializable]
    public class Level
    {
        public GameObject SpawnHero { get; set; }
        public List<GameObject> GameObjects { get; set; }
        public List<GameObject> Background { get; set; }
        public Size FieldSize { get; set; }

        public Level()
        {
            GameObjects = new List<GameObject>();
            Background = new List<GameObject>();
        }
    }

    [Serializable]
    public class Location
    {
        public string Name { get; set; }
        public List<Level> Levels { get; set; }

        public Location(string Name)
        {
            this.Name = Name;
            Levels = new List<Level>();
        }
    }
    #endregion

    #region GameObjects
    [Serializable]
    public class GameObject: ISprite
    {
        public string Name { get; set; }
        public RectangleF Position { get; set; }
        public Sprite GameSprite { get; set; }

        public SizeF UserSizeImage { get; set; }

        public GameObject(string Name) { this.Name = Name; }

        public GameObject() { }

        void ISprite.LoadSprite()
        {
            GameSprite.Image = new Bitmap(GameSprite.PathImage[0]);
            Position = new RectangleF(Position.Location,(UserSizeImage == Size.Empty? GameSprite.Image.Size: UserSizeImage) /*GameSprite.Image.Size*/);
        }
    }

    [Serializable]
    public class Character : GameObject, ISprite
    {
        private int _currenthealth;

        public int Life { get; set; }  //Жизни
        public int CurrentHealth //Текущие здоровье
        {
            get { return _currenthealth; }
            set
            {
                _currenthealth = value;
                if (_currenthealth > MaxHealth)
                    _currenthealth = MaxHealth;
                //if (value + _currenthealth > MaxHealth)
                //    _currenthealth = MaxHealth;
                //else
                //{
                //    _currenthealth = value;
                //}
            }
        } 

        public int MaxHealth { get; set; }  //Максимальное здоровье
        public int Coins { get; set; }
        public Statistic TempStatistic { get; set; }  //Временная статистика
        public int LevelControlBot { get; set; } //Уровень контроля бота
        public bool ControlBot { get; set; }  //Возможность контролировать бота
        public double ResistanceToDamage { get; set; }  //Сопротивление к урону
        public Bot ControlledBot { get; set; } //Управляемый бот

        public RigidBody PhysBody { get; set; }  //Прикрепляем физику
        public Animation Anim { get; set; }

        public Character(): base("Hero")
        {
            this.Life = 3;
            this.MaxHealth = 300;
            this.CurrentHealth = 300;
            TempStatistic = new Statistic();
            LevelControlBot = 0;
            ControlBot = false;
            ResistanceToDamage = 0.2;
            //ControlledBot = null;
            PhysBody = new RigidBody(this, 0.4f, 1f, 15.0f, 110f/*150f*/);
            Anim = new Animation(this);
            Anim.PathTileset = "Content\\Hero\\Hero_01.png";
            this.GameSprite = new Sprite();
            this.GameSprite.PathImage = new string[] { "Content\\Bots\\MakerFirstBot.png" };
        }

        void ISprite.LoadSprite()
        {
            Anim.Tileset = new Bitmap(Anim.PathTileset);
            this.GameSprite.Image = new Bitmap(this.GameSprite.PathImage[0]);
            Position = new RectangleF(Position.Location, (UserSizeImage == Size.Empty ? GameSprite.Image.Size : UserSizeImage)/*this.GameSprite.Image.Size*/);
        }
    }
    
    [Serializable]
    public class DamageObject : GameObject, IState, ISprite
    {
        public bool Destructible { get; set; } //Разрушимость
        //public bool Rebound { get; set; }  //Отскок объекта
        public bool Cripple { get; set; } //Наносить урон?
        public int Damage { get; set; }  //Урон
        
        public DamageObject(string Name, bool Destructible, /*bool Rebound, */bool Cripple, int Damage) : base(Name)
        {
            this.Damage = Damage;
            //this.Rebound = Rebound;
            this.Cripple = Cripple;
            this.Destructible = Destructible;
        }
        
        void IState.StateOn(bool activ, bool inversion)
        {
            if (inversion)
                Cripple = !Cripple;
            else
                Cripple = activ;

            ISprite isprite = this;
            isprite.LoadSprite();
        }

        void ISprite.LoadSprite()
        {
            if (Cripple)
                this.GameSprite.Image = this.GameSprite.PathImage[1] == null ? new Bitmap(this.GameSprite.PathImage[0]) : new Bitmap(this.GameSprite.PathImage[1]); //new Bitmap(this.GameSprite.PathImage[1]);
            else
                this.GameSprite.Image = new Bitmap(this.GameSprite.PathImage[0]);

            Position = new RectangleF(Position.Location, (UserSizeImage == Size.Empty ? GameSprite.Image.Size : UserSizeImage)/*this.GameSprite.Image.Size*/);
        }
    }

    [Serializable]
    public class PointCheck : GameObject, ISprite
    {
        public bool CheckSave { get; set; }

        public void SavePoint(Profile p, Character ch, Level level)
        {
            CheckSave = true;

            level.GameObjects.Remove(ch);

            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream("TempSave.dat", FileMode.Create))
                bf.Serialize(fs, new Save() { Hero = ch, CheckPoint = level });
            using (FileStream fs = new FileStream("TempSave.dat", FileMode.Open))
                p.SaveGame = (Save)bf.Deserialize(fs);
            File.Delete("TempSave.dat");

            ISprite isprite = this;
            isprite.LoadSprite();

            level.GameObjects.Add(ch);
        }

        void ISprite.LoadSprite()
        {
            if (CheckSave)
                this.GameSprite.Image = this.GameSprite.PathImage[1] == null ? new Bitmap(this.GameSprite.PathImage[0]) : new Bitmap(this.GameSprite.PathImage[1]); //new Bitmap(this.GameSprite.PathImage[1]);
            else
                this.GameSprite.Image = new Bitmap(this.GameSprite.PathImage[0]);

            Position = new RectangleF(Position.Location, (UserSizeImage == Size.Empty ? GameSprite.Image.Size : UserSizeImage)/*this.GameSprite.Image.Size*/);
        }

        public PointCheck(string Name) : base(Name) { }
    }

    [Serializable]
    public class Item: GameObject, ISprite
    {
        public int Life { get; set; }
        public int Health { get; set; }
        public int Coin { get; set; }
        public int Control { get; set; }

        public void AddToCharacter(Character c, List<GameObject> dellist)
        {
            if (this.Name == "Life")
                c.Life += Life;

            if (this.Name == "Health")
                c.CurrentHealth += Health;

            if (this.Name == "Coin")
            {
                c.Coins += Coin;
                c.TempStatistic.CountCoins += Coin;
            }

            if (this.Name == "Control")
                c.LevelControlBot += Control;

            dellist.Add(this);
        }

        public RigidBody PhysBody { get; set; }
        public Item(string Name): base(Name)
        {
            PhysBody = new RigidBody(this, 0.4f, 1f, 15.0f, 150f);

            if (this.Name == "Life")
                this.Life = 1;
            if (this.Name == "Health")
                this.Health = 10;
            if (this.Name == "Coin")
                this.Coin = 2;
            if (this.Name == "Control")
                this.Control = 1;
        }

        void ISprite.LoadSprite()
        {
            this.GameSprite.Image = new Bitmap(this.GameSprite.PathImage[0]);

            Position = new RectangleF(Position.Location, (UserSizeImage == Size.Empty ? GameSprite.Image.Size : UserSizeImage)/*this.GameSprite.Image.Size*/);
        }
    }

    [Serializable]
    public class Chest: GameObject, IState, ISprite
    {
        public bool Opened { get; set; }
        public bool Locked { get; set; }

        public List<Item> Items { get; set; }

        public void Open(List<GameObject> listForAddItems)
        {
            if (Locked)
                return;

            if (!Opened)
            {
                LoadSprites(Items);
                listForAddItems.AddRange(Items);
                Items.Clear();
                Opened = true;

                ISprite isprite = this;
                isprite.LoadSprite();
            }
        }

        private void LoadSprites(List<Item> items)
        {
            ISprite isprite;
            foreach (Item item in items)
            {
                (isprite = item).LoadSprite();
                item.Position = new RectangleF(this.Position.X + this.Position.Width / 2 - item.Position.Width /2, this.Position.Y + this.Position.Height / 2 - item.Position.Height/2 - 10, item.Position.Width, item.Position.Height);
                item.PhysBody.Jump();
            }
        }

        public Chest(string Name): base(Name)
        {
            Items = new List<Item>();
        }

        void ISprite.LoadSprite()
        {
            if (Opened)
                this.GameSprite.Image = this.GameSprite.PathImage[1] == null ? new Bitmap(this.GameSprite.PathImage[0]) : new Bitmap(this.GameSprite.PathImage[1]); //new Bitmap(this.GameSprite.PathImage[1]);
            else
                this.GameSprite.Image = new Bitmap(this.GameSprite.PathImage[0]);

            Position = new RectangleF(Position.Location, (UserSizeImage == Size.Empty ? GameSprite.Image.Size : UserSizeImage)/*this.GameSprite.Image.Size*/);
        }

        void IState.StateOn(bool activ, bool inversion)
        {
            if (inversion)
                Locked = !Locked;
            else
                Locked = activ;
        }
    }

    [Serializable]
    public class Switcher: GameObject, ISprite
    {
        private bool _activated = false;

        public bool Activated
        {
            get { return _activated; }
            set { _activated = value; Activate(); }
        }

        public bool Inversion { get; set; }

        public List<GameObject> CanActivateObject { get; set; } 

        public void Activate()
        {
            IState istate;

            foreach(GameObject gobject in CanActivateObject)
            {
                istate = (IState)gobject;
                istate.StateOn(Activated, Inversion);
            }

            ISprite isprite = this;
            isprite.LoadSprite();
        }

        void ISprite.LoadSprite()
        {
            if (Activated)
                this.GameSprite.Image = this.GameSprite.PathImage[1] == null ? new Bitmap(this.GameSprite.PathImage[0]) : new Bitmap(this.GameSprite.PathImage[1]); //new Bitmap(this.GameSprite.PathImage[1]);
            else
                this.GameSprite.Image = new Bitmap(this.GameSprite.PathImage[0]);

            Position = new RectangleF(Position.Location, (UserSizeImage == Size.Empty ? GameSprite.Image.Size : UserSizeImage)/*this.GameSprite.Image.Size*/);
        }

        public Switcher(string Name): base(Name)
        {
            CanActivateObject = new List<GameObject>();
            Inversion = false;
            //Activated = false;
        }
    }

    [Serializable]
    public class SwitcherPassword: Switcher
    {
        public string Password { get; set; }

        public void TryOn(GameProcess gp)
        {
            if (!Activated)
            {
                new InputPasswordForm(this,gp).ShowDialog();
            }
        }

        public bool CheckPassword(string writtenpassword)
        {
            if (writtenpassword == Password)
            {
                Activated = true;
                return true;
            }
            else
                return false;
        }

        public SwitcherPassword(string Name): base(Name)
        {
            Password = "0000";
        }
    }

    [Serializable]
    public class Door : GameObject, IState, ISprite
    {
        public bool Opened { get; set; }
        public bool OpenHorizontal { get; set; }
        public bool DirectionOpenDoorRight { get; set; }


        void IState.StateOn(bool activ, bool inversion)
        {
            if (inversion)
                Opened = !Opened;
            else
                Opened = activ;

            ISprite isprite = this;
            isprite.LoadSprite();
        }

        void ISprite.LoadSprite()
        {
            if (Opened)
            {
                if (OpenHorizontal)
                {
                    if (DirectionOpenDoorRight)
                    {
                        this.GameSprite.Image = this.GameSprite.PathImage[1] == null ? new Bitmap(this.GameSprite.PathImage[0]) : new Bitmap(this.GameSprite.PathImage[1]); //new Bitmap(this.GameSprite.PathImage[1]);
                    }
                    else
                    {
                        this.GameSprite.Image = this.GameSprite.PathImage[1] == null ? new Bitmap(this.GameSprite.PathImage[0]) : new Bitmap(this.GameSprite.PathImage[1]); //new Bitmap(this.GameSprite.PathImage[1]);
                        this.GameSprite.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        Position = new RectangleF(new PointF(Position.X - (this.GameSprite.PathImage[1] != null ? new Bitmap(this.GameSprite.PathImage[1]).Width - new Bitmap(this.GameSprite.PathImage[0]).Width : 0), Position.Y), Position.Size);
                    }
                }
                else
                {
                    if (DirectionOpenDoorRight)
                    {
                        this.GameSprite.Image = this.GameSprite.PathImage[1] == null ? new Bitmap(this.GameSprite.PathImage[0]) : new Bitmap(this.GameSprite.PathImage[1]); //new Bitmap(this.GameSprite.PathImage[1]);
                    }
                    else
                    {
                        this.GameSprite.Image = this.GameSprite.PathImage[1] == null ? new Bitmap(this.GameSprite.PathImage[0]) : new Bitmap(this.GameSprite.PathImage[1]); //new Bitmap(this.GameSprite.PathImage[1]);
                        this.GameSprite.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        Position = new RectangleF(new PointF(Position.X, Position.Y - (this.GameSprite.PathImage[1] != null ? new Bitmap(this.GameSprite.PathImage[1]).Height - new Bitmap(this.GameSprite.PathImage[0]).Height : 0)), Position.Size);
                    }
                }
            }
            else
            {
                if (OpenHorizontal)
                {
                    if (!DirectionOpenDoorRight)
                    {
                        Position = new RectangleF(new PointF(Position.X + (this.GameSprite.PathImage[1] != null ? new Bitmap(this.GameSprite.PathImage[1]).Width - new Bitmap(this.GameSprite.PathImage[0]).Width : 0), Position.Y), Position.Size);
                    }
                }
                else
                {
                    if (!DirectionOpenDoorRight)
                    {
                        Position = new RectangleF(new PointF(Position.X, Position.Y + (this.GameSprite.PathImage[1] != null ? new Bitmap(this.GameSprite.PathImage[1]).Height - new Bitmap(this.GameSprite.PathImage[0]).Height : 0)), Position.Size);
                    }
                }

                this.GameSprite.Image = new Bitmap(this.GameSprite.PathImage[0]);
            }
                //this.GameSprite.Image = new Bitmap(this.GameSprite.PathImage[0]);

            Position = new RectangleF(Position.Location, (UserSizeImage == Size.Empty ? GameSprite.Image.Size : UserSizeImage)/*this.GameSprite.Image.Size*/);
        }

        public Door(string Name): base(Name)
        {
            Opened = false;
            OpenHorizontal = true;
            DirectionOpenDoorRight = true;
        }
    }

    [Serializable]
    public class Shop: GameObject, IState, ISprite
    {
        public bool Activated { get; set; }
        public List<ShopItem> ShopItems { get; set; }

        public void ToShop(Character ch, GameProcess gp)
        {
            if (Activated)
            {

                gp.pause = true;
                ISprite isprite;

                foreach (ShopItem si in ShopItems)
                    if (si.GameImage.Image == null)
                        (isprite = si).LoadSprite();

                new GameShop(this, ch, gp).ShowDialog();
            }
        }


        public bool Buy(Character character, ShopItem shopitem)  //Покупка этого предмета
        {
            if (character.Coins < shopitem.Price)
                return false;

            character.Coins -= shopitem.Price;
            character.TempStatistic.SpentCoins += shopitem.Price;
            shopitem.Price += shopitem.Plusprice;

            switch (shopitem.NameItem)
            {
                case ItemShop.Life:
                    character.Life += (int)shopitem.Value;

                    break;
                case ItemShop.Health:
                    character.CurrentHealth += (int)shopitem.Value;

                    break;
                case ItemShop.Control:
                    character.LevelControlBot += (int)shopitem.Value;

                    break;
                case ItemShop.OpportunityControl:
                    character.ControlBot = true;

                    break;
                case ItemShop.Protection:
                    character.ResistanceToDamage += shopitem.Value;

                    break;
                case ItemShop.UpHealth:
                    character.MaxHealth += (int)shopitem.Value;

                    break;
            }

            return true;
        }


        public Shop(string Name): base(Name)
        {
            Activated = true;
            ShopItems = new List<ShopItem>();
        }

        void IState.StateOn(bool activ, bool inversion)
        {
            if (inversion)
                Activated = !Activated;
            else
                Activated = activ;

            ISprite isprite = this;
            isprite.LoadSprite();
        }

        void ISprite.LoadSprite()
        {
            if (Activated)
                this.GameSprite.Image = this.GameSprite.PathImage[1] == null ? new Bitmap(this.GameSprite.PathImage[0]) : new Bitmap(this.GameSprite.PathImage[1]); //new Bitmap(this.GameSprite.PathImage[1]);
            else
                this.GameSprite.Image = new Bitmap(this.GameSprite.PathImage[0]);

            Position = new RectangleF(Position.Location, (UserSizeImage == Size.Empty ? GameSprite.Image.Size : UserSizeImage)/*this.GameSprite.Image.Size*/);
        }
    }

    [Serializable]
    public class ShopItem: ISprite
    {
        public Sprite GameImage { get; set; }//убрать
        public ItemShop NameItem { get; set; }
        public double Value { get; set; }  //прибавление
        public int Price { get; set; }  //Цена
        public int Plusprice { get; set; }  //На сколько увеличиться цена

        public string Description { get; set; }  //Описание

        public ShopItem(ItemShop NameItem)
        {
            this.NameItem = NameItem;
            
            switch(NameItem)
            {
                case ItemShop.Life:

                    Value = 1;
                    Price = 100;
                    Plusprice = 50;
                    Description = "Увеличивает жизнь";
                    GameImage = new Sprite(new string[] {/* "Sprites\\Shop\\ShopLife.png" */ null});
                    break;
                case ItemShop.Health:

                    Value = 100;
                    Price = 30;
                    Plusprice = 70;
                    Description = "Пополняет здоровье";
                    GameImage = new Sprite(new string[] { /*"Sprites\\Shop\\ShopHealth.png" */null });
                    break;
                case ItemShop.Control:

                    Value = 1;
                    Price = 1000;
                    Plusprice = 1500;
                    Description = "Увеличивает уроень контроля";
                    GameImage = new Sprite(new string[] { /*"Sprites\\Shop\\ShopControl.png" */null });
                    break;
                case ItemShop.OpportunityControl:

                    Value = 0;
                    Price = 10000;
                    Plusprice = 0;
                    Description = "Возможность держать контроль над ботами";
                    GameImage = new Sprite(new string[] { /*"Sprites\\Shop\\ShopOControl.png"*/ null });
                    break;
                case ItemShop.Protection:

                    Value = 0.05;
                    Price = 15000;
                    Plusprice = 500;
                    Description = "Увеличивает сопротивление к урону";
                    GameImage = new Sprite(new string[] { /*"Sprites\\Shop\\ShopProtection.png" */null });
                    break;
                case ItemShop.UpHealth:

                    Value = 100;
                    Price = 10000;
                    Plusprice = 200;
                    Description = "Увеличивает MAX здоровья";
                    GameImage = new Sprite(new string[] { /*"Sprites\\Shop\\ShopUPMaxHealth.png" */null });
                    break;
            }
        }

        //public bool Buy(Character character)  //Покупка этого предмета
        //{
        //    if (character.Coins < Price)
        //        return false;

        //    character.Coins -= Price;
        //    character.TempStatistic.SpentCoins += Price;
        //    Price += Plusprice;

        //    switch (NameItem)
        //    {
        //        case ItemShop.Life:
        //            character.Life += (int)Value;

        //            break;
        //        case ItemShop.Health:
        //            character.CurrentHealth += (int)Value;

        //            break;
        //        case ItemShop.Control:
        //            character.LevelControlBot += (int)Value;

        //            break;
        //        case ItemShop.OpportunityControl:
        //            character.ControlBot = true;

        //            break;
        //        case ItemShop.Protection:
        //            character.ResistanceToDamage += Value;

        //            break;
        //        case ItemShop.UpHealth:
        //            character.MaxHealth += (int)Value;

        //            break;
        //    }

        //    return true;
        //}

        void ISprite.LoadSprite()
        {
            //this.GameImage.Image = new Bitmap(this.GameImage.PathImage[0]);
        }
    }

    [Serializable]
    public class Bot: GameObject, ISprite
    {
        public int Damage { get; set; }
        public RigidBody PhysBody { get; set; }  //Прикрепляем физику
        public Animation Anim { get; set; }
        public int LevelControl { get; set; }
        public bool Controled { get; set; }
        public bool Right { get; set; }

        public Bot(string Name): base(Name)
        {
            Damage = 10;
            LevelControl = 1;

            PhysBody = new RigidBody(this, 0.4f, 1f, 15.0f, 110f/*150f*/);
            Anim = new Animation(this);
            Anim.PathTileset = "Content\\Bots\\Bots_01.png";

            this.GameSprite = new Sprite();
            this.GameSprite.PathImage = new string[] { "Sprites\\Hero.png" };
        }

        void ISprite.LoadSprite()
        {
            Controled = false;
            Anim.Tileset = new Bitmap(Anim.PathTileset);
            this.GameSprite.Image = new Bitmap(this.GameSprite.PathImage[0]);
            //Position = new RectangleF(Position.Location, (UserSizeImage == Size.Empty ? GameSprite.Image.Size : UserSizeImage)/*this.GameSprite.Image.Size*/);
        }
    }

    [Serializable]
    public class Trigger: Switcher
    {
        public void GetIn()
        {
            Activated = true;
        }

        public Trigger(string Name) : base(Name)
        {

        }
    }

    [Serializable]
    public class Teleport: GameObject
    {
        public Teleport Teleport2;

        public void ToTeleport2(Character character)
        {
            if (Teleport2 == null)
                return;

            character.Position = new RectangleF(Teleport2.Position.X + (Teleport2.Position.Width / 2) - 18, Teleport2.Position.Y + (Teleport2.Position.Height / 2) - 18, character.Position.Width, character.Position.Height);
        }

        public Teleport(string Name): base(Name) { }
    }
    #endregion

    #region GameComponents
    [Serializable]
    public class RigidBody
    {
        public static Random rand = new Random();

        public GameObject GObject { get; set; }
        //скорость
        public PointF Velocity { get; set; }
        //масса
        public float Mass { get; set; }
        //упругость
        public float Spring { get; set; }
        //гравитация
        public bool Gravity { get; set; }
        //приложенная сила
        public PointF Force { get; set; }
        //Мы на поверхности?
        public bool OnGround { get; set; }
        //Максимальная скорость ходьбы
        public float MaxSpeed { get; set; }
        //Высота прыжка
        public float MaxJump { get; set; }

        public bool RighDir { get; set; }

        public void Right()
        {
            Velocity = new PointF(MaxSpeed, Velocity.Y);
        }
        public void Right(float speed)
        {
            Velocity = new PointF(speed, Velocity.Y);
        }

        public void Left()
        {
            Velocity = new PointF(-MaxSpeed, Velocity.Y);
        }
        public void Left(float speed)
        {
            Velocity = new PointF(-speed, Velocity.Y);
        }

        public void Jump()
        {
            Velocity = new PointF(Velocity.X, MaxJump);
        }
        public void Jump(float speed)
        {
            Velocity = new PointF(Velocity.X, speed);
        }

        public void Up()
        {
            Velocity = new PointF(Velocity.X, -MaxSpeed / 2);
        }

        public void Down()
        {
            Velocity = new PointF(Velocity.X, MaxSpeed / 2);
        }

        public RigidBody() { }
        public RigidBody(GameObject GObject, float Spring, float Mass, float MaxSpeed, float MaxJump)
        {
            this.GObject = GObject;
            this.Spring = Spring;   //Spring = 0.5f;
            this.Mass = Mass;   //Mass = 1;
            if (GObject is Item)
            {
                Gravity = false;

                for (int i = 0; i < 10; i++)
                   if (RigidBody.rand.Next(0, 2) == 0)
                    {
                        RighDir = true;
                    }
                    else
                    {
                        RighDir = false;
                    }


                this.MaxJump = RigidBody.rand.Next(100, 200);
                this.MaxSpeed = RigidBody.rand.Next(3, 10);

                //this.MaxJump = RigidBody.rand.Next(100, 151);
                //this.MaxSpeed = RigidBody.rand.Next(6, 16);
            }
            else
            {
                this.MaxJump = MaxJump;
                this.MaxSpeed = MaxSpeed;
                Gravity = true;
            }
           
            //this.MaxJump = MaxJump;
            //this.MaxSpeed = MaxSpeed;

            //Gravity = true;
            OnGround = false;
        }

        /// <summary>
        /// Обновление
        /// </summary>
        /// 
        public void UpdatePositionX(float dt)
        {
            GObject.Position = new RectangleF(new PointF(GObject.Position.X + this.Velocity.X * dt, GObject.Position.Y), GObject.Position.Size);
            Velocity = new PointF(0.0f, Velocity.Y);
        }

        public void UpdatePositionY(float dt)
        {
            if (GObject is Item && OnGround)
            {
                //GObject.Position = new RectangleF(new PointF(GObject.Position.X, groundY - 0.0001f), GObject.Position.Size);
                return;
            }
            else if (GObject is Item && Gravity && !OnGround)
                if (RighDir)
                {
                    Right();
                }
                else
                {
                    Left();
                }


            //сила
            var force = Force;
            Force = Point.Empty;
            //гравитация
            if (Gravity)
                force = new PointF(force.X, force.Y + 9.8f * Mass);
            //ускорение
            var ax = force.X / Mass;
            var ay = force.Y;/// Mass;
            //скорость
            Velocity = new PointF(Velocity.X /*+ ax * dt*/, Velocity.Y + (!Gravity? 0: ay * dt));
            //координаты
            GObject.Position = new RectangleF(new PointF(GObject.Position.X + ((Gravity && GObject is Item)?  + Velocity.X * dt: 0), GObject.Position.Y + Velocity.Y * dt), GObject.Position.Size);
        }

        /// <summary>
        /// Вызвать при столкновении с землей
        /// </summary>
        public void OnGroundCollision(float groundY)
        {
            if (GObject is Item && OnGround && !Gravity)
            {
               // GObject.Position = new RectangleF(new PointF(GObject.Position.X, groundY - 0.0001f), GObject.Position.Size);
                return;
            }
        
            if (Velocity.Y < 1f/*0.21000*/ /*-1.10000*//* -float.Epsilon*/) return;

            GObject.Position = new RectangleF(new PointF(GObject.Position.X, groundY - 0.0001f), GObject.Position.Size);

            if (GObject is Character)
            {
                Character c = GObject as Character;
                if (Velocity.Y >= 80 && (Velocity.Y != 110))
                {
                    c.CurrentHealth -= (int)Math.Round(((Velocity.Y * 50) / 80));
                }
            }

            Velocity = new PointF(Velocity.X, -Velocity.Y * Spring);
        }
    }

    [Serializable]
    public class Animation
    {
        private float frame = 0;

        public Direction DirectionCharacter { get; set; }
        public GameObject GObject { get; set; }
        public Bitmap Tileset { get; set; }
        public string PathTileset { get; set; }
        public float SpriteSpeed { get; set; }

        public Animation(GameObject go)
        {
            GObject = go;
            SpriteSpeed = 0.005f;
        }

        public void ChangeFrame(Direction d, float speed, bool gravity)
        {
            GObject.GameSprite.Image = Tileset.Clone(new Rectangle(32 * (int)frame * (!gravity? 0:1), 32 * (int)d, 32, 32), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            frame += speed * SpriteSpeed;
            if (frame >= 3)
                frame = 0;
        }
    }
    #endregion
}