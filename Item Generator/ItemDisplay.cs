/* This form displays all information of a generated item. The stats of the item can be modified to suit the user's needs, and can also be saved as an XML for use
 * in a game. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Item_Generator
{
    public partial class ItemDisplay : Form
    {
        public ItemDisplay()
        {
            InitializeComponent();
        }

        private void Label_ItemName_Click(object sender, EventArgs e)
        {
            
        }

        private void Tooltip_HealthPoints_Popup(object sender, PopupEventArgs e)
        {
            
        }

        private void TextBox_HealthPoints_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            
        }

#region Getters/Setters
        public string ItemName
        {
            get { return Label_ItemName.Text; }
            set { Label_ItemName.Text = value; }
        }

        public string ItemType
        {
            get { return Label_ItemType.Text; }
            set { Label_ItemType.Text = value; }
        }

        public string ItemSubType
        {
            get { return Label_ItemSubType.Text; }
            set { Label_ItemSubType.Text = value; }
        }

        public string ItemLevel
        {
            get { return Label_ItemLevel.Text; }
            set { Label_ItemLevel.Text = value; }
        }

        public string ItemRank
        {
            get { return Label_ItemRank.Text; }
            set { Label_ItemRank.Text = value; }
        }

        public string ItemAilment
        {
            get { return Label_AilmentType.Text; }
            set { Label_AilmentType.Text = value; }
        }

        public string ItemElement
        {
            get { return Label_ElementType.Text; }
            set { Label_ElementType.Text = value; }
        }

        public string AttackPower
        {
            get { return TextBox_AttackPower.Text; }
            set { TextBox_AttackPower.Text = value; }
        }

        public string DefensePower
        {
            get { return TextBox_DefensePower.Text; }
            set { TextBox_DefensePower.Text = value; }
        }

        public string MagicPower
        {
            get { return TextBox_MagicPower.Text; }
            set { TextBox_MagicPower.Text = value; }
        }

        public string Speed
        {
            get { return TextBox_Speed.Text; }
            set { TextBox_Speed.Text = value; }
        }

        public string Accuracy
        {
            get { return TextBox_Accuracy.Text; }
            set { TextBox_Accuracy.Text = value; }
        }

        public string Evasion
        {
            get { return TextBox_Evasion.Text; }
            set { TextBox_Evasion.Text = value; }
        }

        public string MagicResist
        {
            get { return TextBox_MagicResist.Text; }
            set { TextBox_MagicResist.Text = value; }
        }

        public string HealthBonus
        {
            get { return TextBox_HealthPoints.Text; }
            set { TextBox_HealthPoints.Text = value; }
        }

        public string MagicBonus
        {
            get { return TextBox_MagicPoints.Text; }
            set { TextBox_MagicPoints.Text = value; }
        }


        ////////Attack Elements////////////

        public string FireAtkValue
        {
            get { return TextBox_FireAtkValue.Text; }
            set { TextBox_FireAtkValue.Text = value; }
        }

        public string WaterAtkValue
        {
            get { return TextBox_WaterAtkValue.Text; }
            set { TextBox_WaterAtkValue.Text = value; }
        }

        public string EarthAtkValue
        {
            get { return TextBox_EarthAtkValue.Text; }
            set { TextBox_EarthAtkValue.Text = value; }
        }

        public string WindAtkValue
        {
            get { return TextBox_WindAtkValue.Text; }
            set { TextBox_WindAtkValue.Text = value; }
        }

        public string LightAtkValue
        {
            get { return TextBox_LightAtkValue.Text; }
            set { TextBox_LightAtkValue.Text = value; }
        }

        public string DarkAtkValue
        {
            get { return TextBox_DarkAtkValue.Text; }
            set { TextBox_DarkAtkValue.Text = value; }
        }

        ////////Attack Ailments////////////

        public string PoisonAtkValue
        {
            get { return TextBox_PoisonAtkValue.Text; }
            set { TextBox_PoisonAtkValue.Text = value; }
        }

        public string StunAtkValue
        {
            get { return TextBox_StunAtkValue.Text; }
            set { TextBox_StunAtkValue.Text = value; }
        }

        public string SleepAtkValue
        {
            get { return TextBox_SleepAtkValue.Text; }
            set { TextBox_SleepAtkValue.Text = value; }
        }

        public string DeathAtkValue
        {
            get { return TextBox_DeathAtkValue.Text; }
            set { TextBox_DeathAtkValue.Text = value; }
        }

        public string FreezeAtkValue
        {
            get { return TextBox_FreezeAtkValue.Text; }
            set { TextBox_FreezeAtkValue.Text = value; }
        }

        ////////Defense Elements////////////

        public string FireDefValue
        {
            get { return TextBox_FireDefValue.Text; }
            set { TextBox_FireDefValue.Text = value; }
        }

        public string WaterDefValue
        {
            get { return TextBox_WaterDefValue.Text; }
            set { TextBox_WaterDefValue.Text = value; }
        }

        public string EarthDefValue
        {
            get { return TextBox_EarthDefValue.Text; }
            set { TextBox_EarthDefValue.Text = value; }
        }

        public string WindDefValue
        {
            get { return TextBox_WindDefValue.Text; }
            set { TextBox_WindDefValue.Text = value; }
        }

        public string LightDefValue
        {
            get { return TextBox_LightDefValue.Text; }
            set { TextBox_LightDefValue.Text = value; }
        }

        public string DarkDefValue
        {
            get { return TextBox_DarkDefValue.Text; }
            set { TextBox_DarkDefValue.Text = value; }
        }

        ////////Defense Ailments////////////

        public string PoisonDefValue
        {
            get { return TextBox_PoisonDefValue.Text; }
            set { TextBox_PoisonDefValue.Text = value; }
        }

        public string StunDefValue
        {
            get { return TextBox_StunDefValue.Text; }
            set { TextBox_StunDefValue.Text = value; }
        }

        public string SleepDefValue
        {
            get { return TextBox_SleepDefValue.Text; }
            set { TextBox_SleepDefValue.Text = value; }
        }

        public string DeathDefValue
        {
            get { return TextBox_DeathDefValue.Text; }
            set { TextBox_DeathDefValue.Text = value; }
        }

        public string FreezeDefValue
        {
            get { return TextBox_FreezeDefValue.Text; }
            set { TextBox_FreezeDefValue.Text = value; }
        }

        #endregion

        //Write the contents to an XML file.
        private void Button_SaveAsXML_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);            
            defaultPath += "\\My Generated Items\\";
            System.IO.Directory.CreateDirectory(defaultPath);

            //create folders
            string weaponsPath = defaultPath + "\\Weapons\\";
            string armorPath = defaultPath + "\\Armor\\";
            string accPath = defaultPath + "\\Accessories\\";

            //weapons subfolders
            string swordPath = weaponsPath + "\\Swords\\";
            string axePath = weaponsPath + "\\Axes\\";
            string bowPath = weaponsPath + "\\Bows\\";
            string staffPath = weaponsPath + "\\Staves";

            //armor subfolders
            string suitPath = armorPath + "\\Suits\\";
            string vestPath = armorPath + "\\Vests\\";
            string robePath = armorPath + "\\Robes\\";

            //accessory subfolders
            string ringPath = accPath + "\\Rings\\";
            string bootPath = accPath + "\\Boots\\";
            string neckPath = accPath + "\\Necklaces\\";

            System.IO.Directory.CreateDirectory(swordPath);
            System.IO.Directory.CreateDirectory(axePath);
            System.IO.Directory.CreateDirectory(bowPath);
            System.IO.Directory.CreateDirectory(staffPath);
            System.IO.Directory.CreateDirectory(suitPath);
            System.IO.Directory.CreateDirectory(vestPath);
            System.IO.Directory.CreateDirectory(robePath);
            System.IO.Directory.CreateDirectory(ringPath);
            System.IO.Directory.CreateDirectory(bootPath);
            System.IO.Directory.CreateDirectory(neckPath);

            //create an initial filepath by checking what the item subtype is.
            switch (Label_ItemSubType.Text.ToLower())
            {
                case "sword":
                    fileDialog.InitialDirectory = swordPath;
                    break;

                case "axe":
                    fileDialog.InitialDirectory = axePath;
                    break;

                case "bow":
                    fileDialog.InitialDirectory = bowPath;
                    break;

                case "staff":
                    fileDialog.InitialDirectory = staffPath;
                    break;

                case "suit":
                    fileDialog.InitialDirectory = suitPath;
                    break;

                case "vest":
                    fileDialog.InitialDirectory = vestPath;
                    break;

                case "robe":
                    fileDialog.InitialDirectory = robePath;
                    break;

                case "ring":
                    fileDialog.InitialDirectory = ringPath;
                    break;

                case "boots":
                    fileDialog.InitialDirectory = bootPath;
                    break;

                case "necklace":
                    fileDialog.InitialDirectory = neckPath;
                    break;

                default:
                    break;

            }

            fileDialog.FileName = Label_ItemName.Text;
            fileDialog.DefaultExt = "xml";
            fileDialog.AddExtension = true;                 //Ensures that the file extension is added even if removed
            fileDialog.OverwritePrompt = true;
            fileDialog.Filter = "eXtensible Markup Language (.xml)|*.xml|Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            

            //fileDialog.Dis

            //fileDialog.InitialDirectory = swordPath;
            //fileDialog.ShowDialog();

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

               Console.WriteLine(fileDialog.InitialDirectory);

                //write to file
                //string fileName = fileDialog.InitialDirectory + fileDialog.FileName;
                XmlTextWriter writer = new XmlTextWriter(fileDialog.FileName, Encoding.UTF8);
                writer.WriteStartDocument();
                writer.Formatting = Formatting.Indented;


                //string itemType = Regex.Replace(Label_ItemType.Text, " ", "");  //this removes any spaces that are added in case we get an enhanced item.
                writer.WriteStartElement(Label_ItemType.Text);  //This is a tag
                writer.WriteStartElement(Label_ItemSubType.Text);

                //item name
                writer.WriteStartElement("Name");
                writer.WriteString(Label_ItemName.Text);
                writer.WriteEndElement();
                

                //ailment and element
                //string ailmentLabel = Regex.Replace(Label_Ailment.Text, ":", "");
                writer.WriteStartElement("Ailment");
                writer.WriteString(Label_AilmentType.Text);
                writer.WriteEndElement();

                //string elementLabel = Regex.Replace(Label_Element.Text, ":", "");
                writer.WriteStartElement("Element");
                writer.WriteString(Label_ElementType.Text);
                writer.WriteEndElement();


                //item level
                string level = Regex.Replace(Label_ItemLevel.Text, "Level: ", "");
                writer.WriteStartElement("Level");
                writer.WriteString(level);
                writer.WriteEndElement();

                //item rank
                string rank = Regex.Replace(Label_ItemRank.Text, "Rank: ", "");
                writer.WriteStartElement("Rank");
                writer.WriteString(rank);
                writer.WriteEndElement();


                //attack power
                /*(writer.WriteStartElement("Attack");
                writer.WriteString(TextBox_AttackPower.Text);
                writer.WriteEndElement();

                //accuracy
                writer.WriteStartElement("Accuracy");
                writer.WriteString(TextBox_Accuracy.Text);
                writer.WriteEndElement();*/

                //start writing new info based on item type
                switch(Label_ItemType.Text.ToLower())
                {
                    case "weapon":
                        WriteWeaponData(writer);
                        break;

                    case "armor":
                        WriteArmorData(writer);
                        break;

                    case "accessory":
                        {
                            WriteWeaponData(writer);
                            WriteArmorData(writer);
                            WriteAccessoryData(writer);
                        }
                        break;
                    default:
                        break;
                }
                       

                writer.WriteEndElement();                       //close subtype tag
                writer.WriteEndElement();                       //close type tag

                writer.Close();
            }

        }

        private void WriteWeaponData(XmlTextWriter w)
        {
            w.WriteComment("Attack data");
            //attack power
            w.WriteStartElement("AttackPwr");
            w.WriteString(TextBox_AttackPower.Text);
            w.WriteEndElement();

            //accuracy
            w.WriteStartElement("Accuracy");
            w.WriteString(TextBox_Accuracy.Text);
            w.WriteEndElement();

            //magic power
            w.WriteStartElement("MagicPwr");
            w.WriteString(TextBox_MagicPower.Text);
            w.WriteEndElement();


            //element atk attributes
            w.WriteComment("Elements");
            w.WriteStartElement("ElementAtk");

            w.WriteStartElement("Fire");
            w.WriteString(TextBox_FireAtkValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Water");
            w.WriteString(TextBox_WaterAtkValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Wind");
            w.WriteString(TextBox_WindAtkValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Earth");
            w.WriteString(TextBox_EarthAtkValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Light");
            w.WriteString(TextBox_LightAtkValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Dark");
            w.WriteString(TextBox_DarkAtkValue.Text);
            w.WriteEndElement();

            w.WriteEndElement();    //close ElementAtk tag

            //ailment atk attributes
            w.WriteComment("Ailments");
            w.WriteStartElement("AilmentAtk");

            w.WriteStartElement("Poison");
            w.WriteString(TextBox_PoisonAtkValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Stun");
            w.WriteString(TextBox_StunAtkValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Freeze");
            w.WriteString(TextBox_FreezeAtkValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Death");
            w.WriteString(TextBox_DeathAtkValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Sleep");
            w.WriteString(TextBox_SleepAtkValue.Text);
            w.WriteEndElement();

            w.WriteEndElement();    //close AilmentAtk tag
        }

        private void WriteArmorData(XmlTextWriter w)
        {
            w.WriteComment("Defense data");
            //defense power
            w.WriteStartElement("DefensePwr");
            w.WriteString(TextBox_DefensePower.Text);
            w.WriteEndElement();

            //evasion
            w.WriteStartElement("Evasion");
            w.WriteString(TextBox_Evasion.Text);
            w.WriteEndElement();

            //magic resist
            w.WriteStartElement("MagicRes");
            w.WriteString(TextBox_MagicResist.Text);
            w.WriteEndElement();


            //element def attributes
            w.WriteComment("Elements");
            w.WriteStartElement("ElementDef");

            w.WriteStartElement("Fire");
            w.WriteString(TextBox_FireDefValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Water");
            w.WriteString(TextBox_WaterDefValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Wind");
            w.WriteString(TextBox_WindDefValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Earth");
            w.WriteString(TextBox_EarthDefValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Light");
            w.WriteString(TextBox_LightDefValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Dark");
            w.WriteString(TextBox_DarkDefValue.Text);
            w.WriteEndElement();

            w.WriteEndElement();    //close ElementDef tag

            //ailment def attributes
            w.WriteComment("Ailments");
            w.WriteStartElement("AilmentDef");

            w.WriteStartElement("Poison");
            w.WriteString(TextBox_PoisonDefValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Stun");
            w.WriteString(TextBox_StunDefValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Freeze");
            w.WriteString(TextBox_FreezeDefValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Death");
            w.WriteString(TextBox_DeathDefValue.Text);
            w.WriteEndElement();

            w.WriteStartElement("Sleep");
            w.WriteString(TextBox_SleepDefValue.Text);
            w.WriteEndElement();

            w.WriteEndElement();    //close AilmentDef tag
        }

        private void WriteAccessoryData(XmlTextWriter w)
        {
            w.WriteComment("Bonus data");
            w.WriteStartElement("Bonuses");

            w.WriteStartElement("HealthPoints");
            w.WriteString(TextBox_HealthPoints.Text);
            w.WriteEndElement();

            w.WriteStartElement("MagicPoints");
            w.WriteString(TextBox_MagicPoints.Text);
            w.WriteEndElement();

            w.WriteStartElement("Speed");
            w.WriteString(TextBox_Speed.Text);
            w.WriteEndElement();

            w.WriteEndElement();    //end bonuses tag
        }
    }
}
