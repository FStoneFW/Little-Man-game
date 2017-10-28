using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1.MapMaker.PropertiesObject
{
    public partial class ShopPropForm : Form
    {
        public ShopPropForm(Shop shop)
        {
            InitializeComponent();
            this.shop = shop;
            LoadShop(shop);
            LoadShopItems(shop.ShopItems);
        }

        private Shop shop;
        private string[] temppath = new string[2];

        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (pb.Name == "pictureBox1")
                {
                    temppath[0] = openFileDialog1.FileName;
                    pb.Image = new Bitmap(openFileDialog1.FileName);
                }
                else if (pb.Name == "pictureBox2")
                {
                    temppath[1] = openFileDialog1.FileName;
                    pb.Image = new Bitmap(openFileDialog1.FileName);
                }
            }
        }

        private void LoadShop(Shop s)
        {
            checkBox1.Checked = s.Activated;

            if (s.GameSprite.PathImage[0] != null)
                pictureBox1.Image = new Bitmap(s.GameSprite.PathImage[0]);

            if (s.GameSprite.PathImage[1] != null)
                pictureBox2.Image = new Bitmap(s.GameSprite.PathImage[1]);

            temppath[0] = s.GameSprite.PathImage[0];
            temppath[1] = s.GameSprite.PathImage[1];
        }

        private void Save(Shop s)
        {
            s.Activated = checkBox1.Checked;

            s.UserSizeImage = s.GameSprite.PathImage[0] != temppath[0] ? new SizeF(0, 0) : s.UserSizeImage;

            s.GameSprite.PathImage[0] = temppath[0];
            s.GameSprite.PathImage[1] = temppath[1];

            ISprite isprite = s;
            isprite.LoadSprite();
        }

        private void LoadShopItems(List<ShopItem> sitems)
        {
            listBox1.Items.Clear();

            foreach (ShopItem si in sitems)
                listBox1.Items.Add(LabelItem(si));
        }

        private string LabelItem(ShopItem shopitem)
        {
            switch (shopitem.NameItem)
            {
                case ItemShop.Life:

                    return string.Format("{0}: Количество {1}| Цена {2}| Прибавленная цена {3}| Описание: {4}", "Жизнь", shopitem.Value, shopitem.Price, shopitem.Plusprice, shopitem.Description);
                case ItemShop.Health:

                    return string.Format("{0}: Количество {1}| Цена {2}| Прибавленная цена {3}| Описание: {4}", "Здоровье", shopitem.Value, shopitem.Price, shopitem.Plusprice, shopitem.Description);
                case ItemShop.Control:

                    return string.Format("{0}: Количество {1}| Цена {2}| Прибавленная цена {3}| Описание: {4}", "Управление", shopitem.Value, shopitem.Price, shopitem.Plusprice, shopitem.Description);
                case ItemShop.OpportunityControl:

                    return string.Format("{0}: Количество {1}| Цена {2}| Прибавленная цена {3}| Описание: {4}", "Возм. управляения", shopitem.Value, shopitem.Price, shopitem.Plusprice, shopitem.Description);
                case ItemShop.Protection:

                    return string.Format("{0}: Количество {1}| Цена {2}| Прибавленная цена {3}| Описание: {4}", "Защита", shopitem.Value, shopitem.Price, shopitem.Plusprice, shopitem.Description);
                case ItemShop.UpHealth:

                    return string.Format("{0}: Количество {1}| Цена {2}| Прибавленная цена {3}| Описание: {4}", "Увелич. MAX Health", shopitem.Value, shopitem.Price, shopitem.Plusprice, shopitem.Description);
                default:
                    return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save(shop);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                shop.ShopItems.RemoveAt(listBox1.SelectedIndex);
                LoadShopItems(shop.ShopItems);
            }
        }
        private bool CheckExist(ItemShop itemsh)
        {
            foreach (ShopItem si in shop.ShopItems)
                if (si.NameItem == itemsh)
                    return true;
            return false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (CheckExist(ItemShop.Life))
                return;

            shop.ShopItems.Add(new ShopItem(ItemShop.Life));
            LoadShopItems(shop.ShopItems);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (CheckExist(ItemShop.Health))
                return;

            shop.ShopItems.Add(new ShopItem(ItemShop.Health));
            LoadShopItems(shop.ShopItems);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (CheckExist(ItemShop.Control))
                return;

            shop.ShopItems.Add(new ShopItem(ItemShop.Control));
            LoadShopItems(shop.ShopItems);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (CheckExist(ItemShop.OpportunityControl))
                return;

            shop.ShopItems.Add(new ShopItem(ItemShop.OpportunityControl));
            LoadShopItems(shop.ShopItems);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (CheckExist(ItemShop.Protection))
                return;

            shop.ShopItems.Add(new ShopItem(ItemShop.Protection));
            LoadShopItems(shop.ShopItems);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (CheckExist(ItemShop.UpHealth))
                return;

            shop.ShopItems.Add(new ShopItem(ItemShop.UpHealth));
            LoadShopItems(shop.ShopItems);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                new ShopItemPropForm(shop.ShopItems[listBox1.SelectedIndex]).ShowDialog();
                LoadShopItems(shop.ShopItems);
            }
        }
    }
}
