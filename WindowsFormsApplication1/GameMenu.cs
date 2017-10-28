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

namespace WindowsFormsApplication1
{
    public partial class GameMenu : Form
    {
        public GameMenu(ChoiceProfile cp)
        {
            InitializeComponent();
            choiceprofile = cp;
            this.Shown += delegate (object sender, EventArgs e) { CheckSave(choiceprofile.SelectedProfile); };
            this.GotFocus += delegate (object sender, EventArgs e) { CheckSave(choiceprofile.SelectedProfile); };
            this.VisibleChanged += delegate (object sender, EventArgs e) { CheckSave(choiceprofile.SelectedProfile); };
        }

        private ChoiceProfile choiceprofile;

        #region ButtonsOfForm
        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            LoadLevel(choiceprofile.SelectedProfile.CurrentLocAndLev.CurrentLocation, choiceprofile.SelectedProfile.CurrentLocAndLev.CurrentLevel, false);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            NewGame();
            LoadLevel(choiceprofile.SelectedProfile.CurrentLocAndLev.CurrentLocation, choiceprofile.SelectedProfile.CurrentLocAndLev.CurrentLevel, false);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            SaveGame(choiceprofile.SelectedProfile);
            choiceprofile.Show();
            this.Close();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            SaveGame(choiceprofile.SelectedProfile);
            choiceprofile.Close();
        }
        #endregion

        #region TempGet
        private Character GetTempHero(Character c)
        {
            Character ch;

            using (FileStream fs = new FileStream("tempHeroStat.dat", FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, c);
            }

            using (FileStream fs = new FileStream("tempHeroStat.dat", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                ch = (Character)bf.Deserialize(fs);
            }

            File.Delete("tempHeroStat.dat");
            return ch;
        }
        private Level GetTempLevel(Level l)
        {
            Level lev;

            using (FileStream fs = new FileStream("tempLevel.dat", FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, l);
            }

            using (FileStream fs = new FileStream("tempLevel.dat", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                lev = (Level)bf.Deserialize(fs);
            }

            File.Delete("temptLevel.dat");
            return lev;
        }
        #endregion

        //private List<Location> GetLevels()
        //{
        //    if (!File.Exists("Locations"))
        //        return new List<Location>();

        //    using (FileStream fs = new FileStream("Locations", FileMode.Open))
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();

        //        List<Location> a = (List<Location>)bf.Deserialize(fs);

        //        return a;
        //    }
        //}

        private List<Location> GetLevels()
        {
            if (!File.Exists("Locations"))
                return new List<Location>();

            using (FileStream fs = new FileStream("Locations", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();

                return (List<Location>)bf.Deserialize(fs);
            }
        }

        private void SaveGame(Profile p)  //Сохранение
        {
            using (FileStream fs = new FileStream("Profiles//" + p.Nickname + ".dat", FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(fs, p);
            }
        }

        private void CheckSave(Profile p)
        {
            if (p.SaveGame.CheckPoint == null && p.CurrentLocAndLev.CurrentLocation == 0 && p.CurrentLocAndLev.CurrentLevel == 0)
            {
                pictureBox1.Enabled = false;
                pictureBox1.Visible = false;
            }
            else
            {
                pictureBox1.Enabled = true;
                pictureBox1.Visible = true;
            }
        }

        private void NewGame()
        {
            choiceprofile.SelectedProfile.CurrentLocAndLev = new CurrentLocationAndLevel();
            choiceprofile.SelectedProfile.Hero = new Character();
            choiceprofile.SelectedProfile.SaveGame = new Save();
        }

        private void GamePassed()  //Игра Пройдена
        {
            choiceprofile.SelectedProfile.CurrentLocAndLev = new CurrentLocationAndLevel();
            choiceprofile.SelectedProfile.SaveGame.CheckPoint = null;
            SaveGame(choiceprofile.SelectedProfile);
            new GameCompleted(this, choiceprofile.SelectedProfile).ShowDialog();
        }

        public void GameOver()  //Игра окончена (проиграл)
        {
            NewGame();

            CheckSave(choiceprofile.SelectedProfile);
            new GameOver(this).ShowDialog();
        }

        private void NextLevel(Profile p) //переходим на следующий уроень 
        {
            List<Location> CopyLocations = GetLevels();

            if (p.CurrentLocAndLev.CurrentLevel + 1 <= CopyLocations[p.CurrentLocAndLev.CurrentLocation].Levels.Count - 1)
                ++p.CurrentLocAndLev.CurrentLevel;
            else
            {
                ++p.CurrentLocAndLev.CurrentLocation;
                p.CurrentLocAndLev.CurrentLevel = 0;
            }

            CopyLocations.Clear();
        }

        public void LoadLevel(int location, int level, bool dead)
        {
            List<Location> CopyLocations = GetLevels();

            if (location > CopyLocations.Count - 1) 
            {
                GamePassed();
                return;
            }

            Level startlevel;

            if (dead)
            {
                --choiceprofile.SelectedProfile.SaveGame.Hero.Life;
                choiceprofile.SelectedProfile.SaveGame.Hero.CurrentHealth = choiceprofile.SelectedProfile.SaveGame.Hero.MaxHealth;
            }

            if (choiceprofile.SelectedProfile.SaveGame.CheckPoint != null)
            {
                startlevel = GetTempLevel(choiceprofile.SelectedProfile.SaveGame.CheckPoint);
                choiceprofile.SelectedProfile.Hero = GetTempHero(choiceprofile.SelectedProfile.SaveGame.Hero);
            }
            else
            {
                startlevel = CopyLocations[location].Levels[level];
                choiceprofile.SelectedProfile.Hero = GetTempHero(choiceprofile.SelectedProfile.SaveGame.Hero);
                choiceprofile.SelectedProfile.Hero.Position = new RectangleF(startlevel.SpawnHero.Position.Location, choiceprofile.SelectedProfile.Hero.Position.Size);
            }

            new GameProcess(this, choiceprofile.SelectedProfile, CopyLocations[location].Name, startlevel).Show();
            CopyLocations.Clear();
        }

        public void LevelComplete(Character c)  //Уровень пройден
        {
            Statistic tempst = new Statistic();

            choiceprofile.SelectedProfile.StatisticGame.Add(c.TempStatistic);
            tempst.Add(c.TempStatistic);


            c.TempStatistic = new Statistic();
            choiceprofile.SelectedProfile.SaveGame.Hero = GetTempHero(c);

            choiceprofile.SelectedProfile.SaveGame.CheckPoint = null;

            NextLevel(choiceprofile.SelectedProfile);

            SaveGame(choiceprofile.SelectedProfile);

            new GameLevelComplete(this, tempst, choiceprofile.SelectedProfile).Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new MapMaker.MapMakerForm().Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            new GameRating(choiceprofile.SelectedProfile.Nickname).ShowDialog();
        }
    }
}
