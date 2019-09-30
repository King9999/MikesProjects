/* This class is used to parse XML files containing item information.  It works by searching for a file name, and within that file
 * the values contained in each element are copied and assigned to variables. */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace HeroQuest.Items
{
    class ItemReader
    {
        XElement item;      //file type
        ContentManager content;
        string fileName;    //the name of the file.
        string name;        //item's name
        string desc;        //item description. Also lists any effects
        ushort price;       //item's price
        ushort attack;      //attack dice if applicable
        ushort id;          //item effect ID number

        public ItemReader(/*ContentManager content,*/ string fileName)
        {
            //this.content = content;
            //fileName = file;
            item = XElement.Load(fileName + ".xml");
        }

        public string ItemName()
        {
           // string itemName;
            //var item = XElement.Load(fileName + ".xml");
            string itemName = (string)item.Element("name");
            return itemName;
        }
    }
}
