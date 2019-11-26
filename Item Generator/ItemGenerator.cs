/* ITEM GENERATOR BY MIKE MURRAY - NOVEMBER 3, 2019 - Twitter: @MikeADMurray - king9999.itch.io
 * This program is a tool made for ProcJam 2019. What it will do is procedurally generate items for use in a RPG. The items created can be saved as a XML file.
 * I will try to be as generic as possible so that potential devs can just customize the XML files or even the code itself to suit their needs. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace Item_Generator
{

    public partial class ItemGenerator : Form
    {

        private const string VERSION_NUMBER = "1.0";

        Weapon weapon;
        Armor armor;
        Accessory accessory;

        public ItemGenerator()
        {
            InitializeComponent();
        }

        private void ButtonGenerate_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Hello World!");

            /* TODO: If the user doesn't select anything in the drop down boxes, throw an exception */
            if (ComboBox_ItemSubType.Text == "" || ComboBox_ItemType.Text == "")
                MessageBox.Show("Please select an item type and subtype!");
            else
            {
                //Run the process to generate an item, and then call the item display form to show the results.
                byte level = Byte.Parse(TextBox_ItemLevel.Text);

                //Check which item type was selected

                int selectedItem = ComboBox_ItemType.SelectedIndex;
                const int WEAPON = 0;
                const int ARMOR = 1;
                const int ACCESSORY = 2;

                ItemMaker maker = new ItemMaker();

                //int selectedItem = ComboBox_ItemType.SelectedIndex;
                switch (selectedItem)
                {
                    case WEAPON:
                        weapon = GenerateWeapon(weapon);
                        maker.GenerateItem(weapon, level);
                        break;

                    case ARMOR:
                        armor = GenerateArmor(armor);
                        maker.GenerateItem(armor, level);
                        break;

                    case ACCESSORY:
                        accessory = GenerateAccessory(accessory);
                        maker.GenerateItem(accessory, level);
                        break;

                    default:
                        break;
                }
                


                //Console.WriteLine("Item Level is " + TextBox_ItemLevel.Text);
                //ItemMaker maker = new ItemMaker();
                //maker.GenerateItem(item, level);
                //ItemDisplay itemDisplay = new ItemDisplay();
                //itemDisplay.Show();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ComboBox_ItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Every time the item type is selected, the contents of the subtype combo box is cleared and re-populated.
            ComboBox_ItemSubType.Items.Clear();

            //Now we populate the subtype again
            int selectedItem = ComboBox_ItemType.SelectedIndex;
            const int WEAPON = 0;
            const int ARMOR = 1;
            const int ACCESSORY = 2;

            switch (selectedItem)
            {
                case WEAPON:
                    {
                        ComboBox_ItemSubType.Items.Add("Sword");
                        ComboBox_ItemSubType.Items.Add("Axe");
                        ComboBox_ItemSubType.Items.Add("Bow");
                        ComboBox_ItemSubType.Items.Add("Staff");
                    }
                    break;

                case ARMOR:
                    {
                        ComboBox_ItemSubType.Items.Add("Suit");
                        ComboBox_ItemSubType.Items.Add("Vest");
                        ComboBox_ItemSubType.Items.Add("Robe");      
                    }
                    break;

                case ACCESSORY:
                    {
                        ComboBox_ItemSubType.Items.Add("Ring");
                        ComboBox_ItemSubType.Items.Add("Boots");
                        ComboBox_ItemSubType.Items.Add("Necklace");
                    }
                    break;

                /*case CONSUMABLE:
                    {
                        ComboBox_ItemSubType.Items.Add("Healing");
                        ComboBox_ItemSubType.Items.Add("Attack");
                        ComboBox_ItemSubType.Items.Add("Support");
                    }
                    break;*/

                default:
                    break;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ComboBox_ItemSubType_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* The contents of this box changes depending on what's selected for the item type in the first combo box. The contents are
             * generated at runtime. */

           
        }


        private void TextBox_ItemLevel_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            
        }

        private Weapon GenerateWeapon(Weapon item)
        {
            int selectedItem = ComboBox_ItemSubType.SelectedIndex;
            const int SWORD = 0;
            const int AXE = 1;
            const int BOW = 2;
            const int STAFF = 3;

            switch (selectedItem)
            {
                case SWORD:
                    item = new Sword();
                    break;

               case AXE:
                    item = new Axe();
                    break;

                case BOW:
                    item = new Bow();
                    break;

                case STAFF:
                    item = new Staff();
                    break;

                default:
                    break;
            }

            return item;
       
        }

        private Armor GenerateArmor(Armor item)
        {
            int selectedItem = ComboBox_ItemSubType.SelectedIndex;
            const int SUIT = 0;
            const int VEST = 1;
            const int ROBE = 2;

            switch (selectedItem)
            {
                case SUIT:
                    item = new Suit();
                    break;

                case VEST:
                    item = new Vest();
                    break;

                case ROBE:
                    item = new Robe();
                    break;


                default:
                    break;
            }

            return item;

        }

        private Accessory GenerateAccessory(Accessory item)
        {
            int selectedItem = ComboBox_ItemSubType.SelectedIndex;
            const int RING = 0;
            const int BOOTS = 1;
            const int NECKLACE = 2;

            switch (selectedItem)
            {
                case RING:
                    item = new Ring();
                    break;

                case BOOTS:
                    item = new Boots();
                    break;

                case NECKLACE:
                    item = new Necklace();
                    break;


                default:
                    break;
            }

            return item;

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //display a window showing my contact info.
            MessageBox.Show("Item Generator v" + VERSION_NUMBER + " by Mike Murray\n\n Made for Proc Jam 2019 \n\n Twitter: @MikeADMurray \n\n king9999.itch.io", "About!");
        }
    }
}
