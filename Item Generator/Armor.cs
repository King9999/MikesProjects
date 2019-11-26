using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Armor : Item
    {
        protected short ArmDef;                 //Defense Power. Reduces physical damage. Minimum value is 0
        protected short ArmRes;                 //Magic Resistance. Reduces magic damage.
        protected float ArmEvade;             //Chance to dodge physical attacks.
        protected byte ArmElementDefValue;
        protected byte ArmAilmentDefValue;
        protected short ArmDefMod;              //a modifier for adjusting defense power.
        protected short ArmResMod;

        ////Constants////
        protected const short MAX_DEFENSE = 9999;
        protected const float MAX_EVADE = 1f;    //This can also be used for elements and status effect dodge chance
        protected const short MAX_MAGICRES = 9999;



        /* The following lists of dictionaries are used to contain the prefixes and the strength values of each element/ailment */
        protected List<Dictionary<string, byte>> armElementNames = new List<Dictionary<string, byte>>();
        protected List<Dictionary<string, byte>> armAilmentNames = new List<Dictionary<string, byte>>();

        public enum ArmAilment : byte        //must be public so that getters/setters will work
        {
            None,
            Poison,
            Stun,               
            Freeze,                  
            Death,
            Sleep
        }

        public enum ArmElement : byte
        {
            None,
            Fire,
            Water,
            Earth,              
            Wind,               
            Light,                                                                  
            Dark
        }

        protected ArmElement ArmActiveElement;
        protected ArmAilment ArmActiveAilment;


        public Armor()
        {
            ItemLevel = 1;
            ArmDef = 1;
            ArmEvade = 0;
            ArmRes = 0;
            ArmActiveElement = 0;
            ArmActiveAilment = 0;
            ItemName = "";
            ItemRank = 0;
            ItemType = "Armor";
            ArmElementDefValue = 0;
            ArmAilmentDefValue = 0;
            ArmActiveElement = ArmElement.None;
            ArmActiveAilment = ArmAilment.None;
            ArmDefMod = 1;
            ArmResMod = 1;

            /* Set up names. Prefixes are always status effects, while suffixes are always an element. The second value in the dictionary is the 
             * max percentage the weapon can have in a given tier. */
            for (int i = 0; i < Enum.GetNames(typeof(ArmAilment)).Length; i++)
                armAilmentNames.Add(new Dictionary<string, byte>());

            for (int i = 0; i < Enum.GetNames(typeof(ArmElement)).Length; i++)
                armElementNames.Add(new Dictionary<string, byte>());

            /****Ailments. These are prefixes, which means armor names can begin with these names. *****/

            //Poison tiers
            armAilmentNames[(int)ArmAilment.Poison].Add("Asp's", 10);
            armAilmentNames[(int)ArmAilment.Poison].Add("Cobra's", 20);
            armAilmentNames[(int)ArmAilment.Poison].Add("Anaconda's", 30);
            armAilmentNames[(int)ArmAilment.Poison].Add("Viper's", 40);
            armAilmentNames[(int)ArmAilment.Poison].Add("Boa's", 50);
            armAilmentNames[(int)ArmAilment.Poison].Add("Rattlesnake's", 60);
            armAilmentNames[(int)ArmAilment.Poison].Add("Bushmaster's", 70);
            armAilmentNames[(int)ArmAilment.Poison].Add("Copperhead's", 80);
            armAilmentNames[(int)ArmAilment.Poison].Add("Sidewinder's", 90);
            armAilmentNames[(int)ArmAilment.Poison].Add("Death Adder's", 100);


            //Console.WriteLine(armAilmentNames[(int)ArmAilment.Poison].Keys);

            //Stun tiers
            armAilmentNames[(int)ArmAilment.Stun].Add("Wasp's", 10);
            armAilmentNames[(int)ArmAilment.Stun].Add("Jellyfish's", 20);
            armAilmentNames[(int)ArmAilment.Stun].Add("Scorpion's", 30);
            armAilmentNames[(int)ArmAilment.Stun].Add("Basilisk's", 40);
            armAilmentNames[(int)ArmAilment.Stun].Add("Spore's", 50);
            armAilmentNames[(int)ArmAilment.Stun].Add("Parasite's", 60);
            armAilmentNames[(int)ArmAilment.Stun].Add("Frog's", 70);
            armAilmentNames[(int)ArmAilment.Stun].Add("Spider's", 80);
            armAilmentNames[(int)ArmAilment.Stun].Add("Ghoul's", 90);
            armAilmentNames[(int)ArmAilment.Stun].Add("Banshee's", 100);

            //Freezing tiers
            armAilmentNames[(int)ArmAilment.Freeze].Add("Chilling", 10);
            armAilmentNames[(int)ArmAilment.Freeze].Add("Frosty", 20);
            armAilmentNames[(int)ArmAilment.Freeze].Add("Icy", 30);
            armAilmentNames[(int)ArmAilment.Freeze].Add("Biting", 40);
            armAilmentNames[(int)ArmAilment.Freeze].Add("Polar", 50);
            armAilmentNames[(int)ArmAilment.Freeze].Add("Glacial", 60);    
            armAilmentNames[(int)ArmAilment.Freeze].Add("Wintry", 70);
            armAilmentNames[(int)ArmAilment.Freeze].Add("Frigid", 80);
            armAilmentNames[(int)ArmAilment.Freeze].Add("Shivering", 90);
            armAilmentNames[(int)ArmAilment.Freeze].Add("Icicle", 100);

            //Death tiers
            armAilmentNames[(int)ArmAilment.Death].Add("Dim", 10);
            armAilmentNames[(int)ArmAilment.Death].Add("Shadow", 20);
            armAilmentNames[(int)ArmAilment.Death].Add("Sinister", 30);
            armAilmentNames[(int)ArmAilment.Death].Add("Blackened", 40);
            armAilmentNames[(int)ArmAilment.Death].Add("Death", 50);
            armAilmentNames[(int)ArmAilment.Death].Add("Disastrous", 60);    
            armAilmentNames[(int)ArmAilment.Death].Add("Calamitous", 70);
            armAilmentNames[(int)ArmAilment.Death].Add("Obliterating", 80);
            armAilmentNames[(int)ArmAilment.Death].Add("Nihil", 90);
            armAilmentNames[(int)ArmAilment.Death].Add("Void", 100);

            //Sleep tiers
            armAilmentNames[(int)ArmAilment.Sleep].Add("Fairy's", 10);
            armAilmentNames[(int)ArmAilment.Sleep].Add("Cricket's", 20);
            armAilmentNames[(int)ArmAilment.Sleep].Add("Pesanta's", 30);
            armAilmentNames[(int)ArmAilment.Sleep].Add("Adarna's", 40);
            armAilmentNames[(int)ArmAilment.Sleep].Add("Bakhtak's", 50);
            armAilmentNames[(int)ArmAilment.Sleep].Add("Owl's", 60);
            armAilmentNames[(int)ArmAilment.Sleep].Add("Alp's", 70);
            armAilmentNames[(int)ArmAilment.Sleep].Add("Incubus's", 80);
            armAilmentNames[(int)ArmAilment.Sleep].Add("Mare's", 90);
            armAilmentNames[(int)ArmAilment.Sleep].Add("Succubus's", 100);

            /*****Elements. These are suffixes, which means they are added at the end of weapon names****/

            //Fire tiers
            armElementNames[(int)ArmElement.Fire].Add("of the Salamander", 10);
            armElementNames[(int)ArmElement.Fire].Add("of the Drake", 20);
            armElementNames[(int)ArmElement.Fire].Add("of the Kiln", 30);
            armElementNames[(int)ArmElement.Fire].Add("of Magma", 40);
            armElementNames[(int)ArmElement.Fire].Add("of Basalt", 50);
            armElementNames[(int)ArmElement.Fire].Add("of the Furnace", 60);
            armElementNames[(int)ArmElement.Fire].Add("of the Volcano", 70);
            armElementNames[(int)ArmElement.Fire].Add("of the Firebird", 80);
            armElementNames[(int)ArmElement.Fire].Add("of the Ifrit", 90);
            armElementNames[(int)ArmElement.Fire].Add("of the Seraphim", 100);

            //Water tiers
            armElementNames[(int)ArmElement.Water].Add("of the Goldfish", 10);
            armElementNames[(int)ArmElement.Water].Add("of the Otter", 20);
            armElementNames[(int)ArmElement.Water].Add("of the Penguin", 30);
            armElementNames[(int)ArmElement.Water].Add("of the Floe", 40);
            armElementNames[(int)ArmElement.Water].Add("of Waterfalls", 50);
            armElementNames[(int)ArmElement.Water].Add("of Snow", 60);
            armElementNames[(int)ArmElement.Water].Add("of the Polar Bear", 70);
            armElementNames[(int)ArmElement.Water].Add("of the Yeti", 80);
            armElementNames[(int)ArmElement.Water].Add("of Ice", 90);
            armElementNames[(int)ArmElement.Water].Add("of Storms", 100);

            //Earth tiers
            armElementNames[(int)ArmElement.Earth].Add("of Mud", 10);
            armElementNames[(int)ArmElement.Earth].Add("of Granite", 20);
            armElementNames[(int)ArmElement.Earth].Add("of Topaz", 30);
            armElementNames[(int)ArmElement.Earth].Add("of Garnet", 40);
            armElementNames[(int)ArmElement.Earth].Add("of the Ruby", 50);
            armElementNames[(int)ArmElement.Earth].Add("of Silver", 60);
            armElementNames[(int)ArmElement.Earth].Add("of Gold", 70);
            armElementNames[(int)ArmElement.Earth].Add("of Platinum", 80);
            armElementNames[(int)ArmElement.Earth].Add("of Diamond", 90);
            armElementNames[(int)ArmElement.Earth].Add("of Obsidian", 100);

            //Wind tiers
            armElementNames[(int)ArmElement.Wind].Add("of the Gull", 10);
            armElementNames[(int)ArmElement.Wind].Add("of the Windmill", 20);
            armElementNames[(int)ArmElement.Wind].Add("of the Blue Jay", 30);
            armElementNames[(int)ArmElement.Wind].Add("of the Cloud", 40);
            armElementNames[(int)ArmElement.Wind].Add("of the Sky", 50);
            armElementNames[(int)ArmElement.Wind].Add("of Calm", 60);
            armElementNames[(int)ArmElement.Wind].Add("of the Egret", 70);
            armElementNames[(int)ArmElement.Wind].Add("of the Peacock", 80);
            armElementNames[(int)ArmElement.Wind].Add("of the Eagle", 90);
            armElementNames[(int)ArmElement.Wind].Add("of the Albatross", 100);

            //Light tiers
            armElementNames[(int)ArmElement.Light].Add("of Dawn", 10);
            armElementNames[(int)ArmElement.Light].Add("of Luminance", 20);
            armElementNames[(int)ArmElement.Light].Add("of Daybreak", 30);
            armElementNames[(int)ArmElement.Light].Add("of Brilliance", 40);
            armElementNames[(int)ArmElement.Light].Add("of Splendor", 50);
            armElementNames[(int)ArmElement.Light].Add("of Resplendence", 60);
            armElementNames[(int)ArmElement.Light].Add("of Starlight", 70);
            armElementNames[(int)ArmElement.Light].Add("of Radiance", 80);
            armElementNames[(int)ArmElement.Light].Add("of Scintillation", 90);
            armElementNames[(int)ArmElement.Light].Add("of the Sun", 100);

            //Dark tiers
            armElementNames[(int)ArmElement.Dark].Add("of Dusk", 10);
            armElementNames[(int)ArmElement.Dark].Add("of Gloom", 20);
            armElementNames[(int)ArmElement.Dark].Add("of Shade", 30);
            armElementNames[(int)ArmElement.Dark].Add("of Eventide", 40);
            armElementNames[(int)ArmElement.Dark].Add("of Midnight", 50);
            armElementNames[(int)ArmElement.Dark].Add("of the Umbra", 60);
            armElementNames[(int)ArmElement.Dark].Add("of Spirits", 70);
            armElementNames[(int)ArmElement.Dark].Add("of Sin", 80);
            armElementNames[(int)ArmElement.Dark].Add("of Twilight", 90);
            armElementNames[(int)ArmElement.Dark].Add("of the End", 100);


        }

        #region Getters/Setters



        public short GetDefensePower()
        {
            return ArmDef;
        }

        public void SetDefensePower(short def)
        {
            ArmDef = def;
            if (ArmDef < 1)
                ArmDef = 1;
            if (ArmDef > MAX_DEFENSE)
                ArmDef = MAX_DEFENSE;
        }

        public short GetMagicResist()
        {
            return ArmRes;
        }

        public void SetMagicResist(short res)
        {
            ArmRes = res;
            if (ArmRes < 0)
                ArmRes = 0;
            if (ArmRes > MAX_MAGICRES)
                ArmRes = MAX_MAGICRES;
        }

        public float GetEvade()
        {
            return ArmEvade;
        }

        public void SetEvade(float evd)
        {
            ArmEvade = evd;
            if (ArmEvade > MAX_EVADE)
                ArmEvade = MAX_EVADE;
        }

        public ArmElement GetElement()
        {
            return ArmActiveElement;
        }

        public void SetElement(ArmElement element, byte value)
        {
            ArmActiveElement = element;
            ArmElementDefValue = value;
            if (ArmElementDefValue > 100)
                ArmElementDefValue = 100;
        }

        public ArmAilment GetAilment()
        {
            return ArmActiveAilment;
        }

        public void SetAilment(ArmAilment ailment, byte value)
        {
            ArmActiveAilment = ailment;
            ArmAilmentDefValue = value;
            if (ArmAilmentDefValue > 100)
                ArmAilmentDefValue = 100;
        }

        public byte GetElementDefValue()
        {
            return ArmElementDefValue;
        }



        public byte GetAilmentDefValue()
        {
            return ArmAilmentDefValue;
        }

        public short GetDefMod()
        {
            return ArmDefMod;
        }

        public void SetDefMod(short val)
        {
            ArmDefMod = val;
        }

        public short GetResMod()
        {
            return ArmResMod;
        }

        public void SetResMod(short val)
        {
            ArmResMod = val;
        }

        public int GetCount(List<Dictionary<string, byte>> list)
        {
            return list.Count;
        }

        public List<Dictionary<string, byte>> GetArmorElementNames()
        {
            return armElementNames;
        }

        public List<Dictionary<string, byte>> GetArmorAilmentNames()
        {
            return armAilmentNames;
        }

        public short GetMaxDefensePower()
        {
            return MAX_DEFENSE;
        }

        public short GetMaxMagicResist()
        {
            return MAX_MAGICRES;
        }

        public float GetMaxEvade()
        {
            return MAX_EVADE;
        }

        #endregion
    }
}
