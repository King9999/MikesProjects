using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Weapon : Item
    {
        protected short WpnAtk;                 //Attack Power. Determines physical damage. Minimum value is 0
        protected float WpnAcc;                 //Accuracy. Hit chance is a value between 0 and 1.
        protected short WpnMag;                 //Magic Power. Determines magical damage. Min value is 0. Generally applies to staff weapons, but other weapons can boost Mag.
        protected byte WpnElementAtkValue;
        protected byte WpnAilmentAtkValue;
        protected short WpnAtkMod;              //a modifier for adjusting attack power.
        protected short WpnMagMod;

        ////Constants////
        protected const short MAX_ATTACK = 9999;
        protected const float MAX_ACCURACY = 1f;    //This can also be used for elements and status effect hit chance
        protected const short MAX_MAGICATK = 9999;
        


        /* The following lists of dictionaries are used to contain the prefixes and the strength values of each element/ailment */
        protected List<Dictionary<string, byte>> wpnElementNames = new List<Dictionary<string, byte>>();
        protected List<Dictionary<string, byte>> wpnAilmentNames = new List<Dictionary<string, byte>>();

        public enum WpnAilment:byte        //must be public so that getters/setters will work
        {
            None,
            Poison,
            Stun,               //A weapon can come with a status effect. Weapons can only have one status effect enabled.
            Freeze,             //The success rate of landing an effect is determined by the suffix in the item name.
            Death,
            Sleep
        }

        public enum WpnElement:byte
        {
            None,
            Fire,
            Water,
            Earth,              //A weapon can have an element. A staff is most likely to have an element than any other weapon type.
            Wind,               //A weapon can have up to two elements, and the name of item will reflect that. The only invalid combinations
            Light,              //are: Fire + Water, Earth + Wind, and Light + Dark.       
            Dark
        }

        protected WpnElement WpnActiveElement;        
        protected WpnAilment WpnActiveAilment;
        

        public Weapon()
        {
            ItemLevel = 1;
            WpnAtk = 1;
            WpnAcc = 0;
            WpnMag = 0;
            WpnActiveElement = 0;       
            WpnActiveAilment = 0;
            ItemName = "";
            ItemRank = 0;
            ItemType = "Weapon";
            WpnElementAtkValue = 0;
            WpnAilmentAtkValue = 0;
            WpnActiveElement = WpnElement.None;
            WpnActiveAilment = WpnAilment.None;
            WpnAtkMod = 1;
            WpnMagMod = 1;

            /* Set up names. Prefixes are always status effects, while suffixes are always an element. The second value in the dictionary is the 
             * max percentage the weapon can have in a given tier. */
            for (int i = 0; i < Enum.GetNames(typeof(WpnAilment)).Length; i++)
                wpnAilmentNames.Add(new Dictionary<string, byte>());

            for (int i = 0; i < Enum.GetNames(typeof(WpnElement)).Length; i++)
                wpnElementNames.Add(new Dictionary<string, byte>());

            /****Ailments. These are prefixes, which means weapon names can begin with these names. *****/

            //Poison tiers
            wpnAilmentNames[(int)WpnAilment.Poison].Add("Bacterial", 10);
            wpnAilmentNames[(int)WpnAilment.Poison].Add("Contaminated", 20);
            wpnAilmentNames[(int)WpnAilment.Poison].Add("Tainted", 30);
            wpnAilmentNames[(int)WpnAilment.Poison].Add("Toxic", 40);
            wpnAilmentNames[(int)WpnAilment.Poison].Add("Infected", 50);
            wpnAilmentNames[(int)WpnAilment.Poison].Add("Viral", 60);
            wpnAilmentNames[(int)WpnAilment.Poison].Add("Venomous", 70);
            wpnAilmentNames[(int)WpnAilment.Poison].Add("Virulent", 80);
            wpnAilmentNames[(int)WpnAilment.Poison].Add("Noxious", 90);
            wpnAilmentNames[(int)WpnAilment.Poison].Add("Biohazardous", 100);

           
            //Console.WriteLine(wpnAilmentNames[(int)WpnAilment.Poison].Keys);

            //Stun tiers
            wpnAilmentNames[(int)WpnAilment.Stun].Add("Tingling", 10);
            wpnAilmentNames[(int)WpnAilment.Stun].Add("Numbing", 20);
            wpnAilmentNames[(int)WpnAilment.Stun].Add("Stiffening", 30);
            wpnAilmentNames[(int)WpnAilment.Stun].Add("Immobilizing", 40);
            wpnAilmentNames[(int)WpnAilment.Stun].Add("Disabling", 50);
            wpnAilmentNames[(int)WpnAilment.Stun].Add("Debilitating", 60);
            wpnAilmentNames[(int)WpnAilment.Stun].Add("Paralyzing", 70);
            wpnAilmentNames[(int)WpnAilment.Stun].Add("Disarming", 80);
            wpnAilmentNames[(int)WpnAilment.Stun].Add("Arresting", 90);
            wpnAilmentNames[(int)WpnAilment.Stun].Add("Enfeebling", 100);

            //Freezing tiers
            wpnAilmentNames[(int)WpnAilment.Freeze].Add("Chilling", 10);
            wpnAilmentNames[(int)WpnAilment.Freeze].Add("Frosty", 20);
            wpnAilmentNames[(int)WpnAilment.Freeze].Add("Icy", 30);
            wpnAilmentNames[(int)WpnAilment.Freeze].Add("Biting", 40);
            wpnAilmentNames[(int)WpnAilment.Freeze].Add("Polar", 50);
            wpnAilmentNames[(int)WpnAilment.Freeze].Add("Glacial", 60);    //a weapon with this effect should be more likely to have water element
            wpnAilmentNames[(int)WpnAilment.Freeze].Add("Wintry", 70);
            wpnAilmentNames[(int)WpnAilment.Freeze].Add("Frigid", 80);
            wpnAilmentNames[(int)WpnAilment.Freeze].Add("Shivering", 90);
            wpnAilmentNames[(int)WpnAilment.Freeze].Add("Icicle", 100);

            //Death tiers
            wpnAilmentNames[(int)WpnAilment.Death].Add("Dim", 10);
            wpnAilmentNames[(int)WpnAilment.Death].Add("Shadow", 20);
            wpnAilmentNames[(int)WpnAilment.Death].Add("Sinister", 30);
            wpnAilmentNames[(int)WpnAilment.Death].Add("Blackened", 40);
            wpnAilmentNames[(int)WpnAilment.Death].Add("Death", 50);
            wpnAilmentNames[(int)WpnAilment.Death].Add("Disastrous", 60);    //a weapon with this effect should be more likely to have dark element
            wpnAilmentNames[(int)WpnAilment.Death].Add("Calamitous", 70);
            wpnAilmentNames[(int)WpnAilment.Death].Add("Obliterating", 80);
            wpnAilmentNames[(int)WpnAilment.Death].Add("Nihil", 90);
            wpnAilmentNames[(int)WpnAilment.Death].Add("Void", 100);

            //Sleep tiers
            wpnAilmentNames[(int)WpnAilment.Sleep].Add("Drowsy", 10);
            wpnAilmentNames[(int)WpnAilment.Sleep].Add("Torpid", 20);
            wpnAilmentNames[(int)WpnAilment.Sleep].Add("Slumberous", 30);
            wpnAilmentNames[(int)WpnAilment.Sleep].Add("Soporific", 40);
            wpnAilmentNames[(int)WpnAilment.Sleep].Add("Sluggish", 50);
            wpnAilmentNames[(int)WpnAilment.Sleep].Add("Somnolent", 60);    
            wpnAilmentNames[(int)WpnAilment.Sleep].Add("Hypnotic", 70);
            wpnAilmentNames[(int)WpnAilment.Sleep].Add("Lethargic", 80);
            wpnAilmentNames[(int)WpnAilment.Sleep].Add("Dreamy", 90);
            wpnAilmentNames[(int)WpnAilment.Sleep].Add("Nightmarish", 100);

            /*****Elements. These are suffixes, which means they are added at the end of weapon names****/

            //Fire tiers
            wpnElementNames[(int)WpnElement.Fire].Add("of Heat", 10);
            wpnElementNames[(int)WpnElement.Fire].Add("of Charring", 20);
            wpnElementNames[(int)WpnElement.Fire].Add("of Embers", 30);
            wpnElementNames[(int)WpnElement.Fire].Add("of Searing", 40);
            wpnElementNames[(int)WpnElement.Fire].Add("of the Bonfire", 50);
            wpnElementNames[(int)WpnElement.Fire].Add("of Incandescence", 60);
            wpnElementNames[(int)WpnElement.Fire].Add("of Brimstone", 70);
            wpnElementNames[(int)WpnElement.Fire].Add("of Flames", 80);
            wpnElementNames[(int)WpnElement.Fire].Add("of Combustion", 90);
            wpnElementNames[(int)WpnElement.Fire].Add("of the Inferno", 100);

            //Water tiers
            wpnElementNames[(int)WpnElement.Water].Add("of Droplets", 10);
            wpnElementNames[(int)WpnElement.Water].Add("of the Puddle", 20);
            wpnElementNames[(int)WpnElement.Water].Add("of the Pond", 30);
            wpnElementNames[(int)WpnElement.Water].Add("of the Lake", 40);
            wpnElementNames[(int)WpnElement.Water].Add("of the Ocean", 50);
            wpnElementNames[(int)WpnElement.Water].Add("of the Tsunami", 60);
            wpnElementNames[(int)WpnElement.Water].Add("of the Whirlpool", 70);
            wpnElementNames[(int)WpnElement.Water].Add("of the Flood", 80);
            wpnElementNames[(int)WpnElement.Water].Add("of the Deluge", 90);
            wpnElementNames[(int)WpnElement.Water].Add("of the Torrent", 100);

            //Earth tiers
            wpnElementNames[(int)WpnElement.Earth].Add("of the Pebble", 10);
            wpnElementNames[(int)WpnElement.Earth].Add("of Gravel", 20);
            wpnElementNames[(int)WpnElement.Earth].Add("of Rubble", 30);
            wpnElementNames[(int)WpnElement.Earth].Add("of Sands", 40);
            wpnElementNames[(int)WpnElement.Earth].Add("of Bedrock", 50);
            wpnElementNames[(int)WpnElement.Earth].Add("of the Boulder", 60);
            wpnElementNames[(int)WpnElement.Earth].Add("of the Crag", 70);
            wpnElementNames[(int)WpnElement.Earth].Add("of Seimicity", 80);
            wpnElementNames[(int)WpnElement.Earth].Add("of Tremors", 90);
            wpnElementNames[(int)WpnElement.Earth].Add("of the Quake", 100);

            //Wind tiers
            wpnElementNames[(int)WpnElement.Wind].Add("of Air", 10);
            wpnElementNames[(int)WpnElement.Wind].Add("of the Breeze", 20);
            wpnElementNames[(int)WpnElement.Wind].Add("of Gust", 30);
            wpnElementNames[(int)WpnElement.Wind].Add("of Gales", 40);
            wpnElementNames[(int)WpnElement.Wind].Add("of the Mistral", 50);
            wpnElementNames[(int)WpnElement.Wind].Add("of the Twister", 60);
            wpnElementNames[(int)WpnElement.Wind].Add("of the Squall", 70);
            wpnElementNames[(int)WpnElement.Wind].Add("of the Maelstrom", 80);
            wpnElementNames[(int)WpnElement.Wind].Add("of the Tempest", 90);
            wpnElementNames[(int)WpnElement.Wind].Add("of the Cyclone", 100);

            //Light tiers
            wpnElementNames[(int)WpnElement.Light].Add("of Dawn", 10);
            wpnElementNames[(int)WpnElement.Light].Add("of Luminance", 20);
            wpnElementNames[(int)WpnElement.Light].Add("of Daybreak", 30);
            wpnElementNames[(int)WpnElement.Light].Add("of Brilliance", 40);
            wpnElementNames[(int)WpnElement.Light].Add("of Splendor", 50);
            wpnElementNames[(int)WpnElement.Light].Add("of Resplendence", 60);
            wpnElementNames[(int)WpnElement.Light].Add("of Starlight", 70);
            wpnElementNames[(int)WpnElement.Light].Add("of Radiance", 80);
            wpnElementNames[(int)WpnElement.Light].Add("of Scintillation", 90);
            wpnElementNames[(int)WpnElement.Light].Add("of the Sun", 100);

            //Dark tiers
            wpnElementNames[(int)WpnElement.Dark].Add("of Dusk", 10);
            wpnElementNames[(int)WpnElement.Dark].Add("of Gloom", 20);
            wpnElementNames[(int)WpnElement.Dark].Add("of Shade", 30);
            wpnElementNames[(int)WpnElement.Dark].Add("of Eventide", 40);
            wpnElementNames[(int)WpnElement.Dark].Add("of Midnight", 50);
            wpnElementNames[(int)WpnElement.Dark].Add("of the Umbra", 60);
            wpnElementNames[(int)WpnElement.Dark].Add("of Spirits", 70);
            wpnElementNames[(int)WpnElement.Dark].Add("of Sin", 80);
            wpnElementNames[(int)WpnElement.Dark].Add("of Twilight", 90);
            wpnElementNames[(int)WpnElement.Dark].Add("of the End", 100);


        }

        #region Getters/Setters

   

        public short GetAttackPower()
        {
            return WpnAtk;
        }

        public void SetAttackPower(short atk)
        {
            WpnAtk = atk;
            if (WpnAtk < 1)
                WpnAtk = 1;
            if (WpnAtk > MAX_ATTACK)
                WpnAtk = MAX_ATTACK;
        }

        public short GetMagicPower()
        {
            return WpnMag;
        }

        public void SetMagicPower(short mag)
        {
            WpnMag = mag;
            if (WpnMag < 0)
                WpnMag = 0;
            if (WpnMag > MAX_MAGICATK)
                WpnMag = MAX_MAGICATK;
        }

        public float GetAccuracy()
        {
            return WpnAcc;
        }

        public void SetAccuracy(float acc)
        {
            WpnAcc = acc;
            if (WpnAcc > MAX_ACCURACY)
                WpnAcc = MAX_ACCURACY;
        }

        public WpnElement GetElement()
        {
            return WpnActiveElement;
        }

        public void SetElement(WpnElement element, byte value)
        {
            WpnActiveElement = element;
            WpnElementAtkValue = value;
            if (WpnElementAtkValue > 100)
                WpnElementAtkValue = 100;
        }

        public WpnAilment GetAilment()
        {
            return WpnActiveAilment;
        }

        public void SetAilment(WpnAilment ailment, byte value)
        {
            WpnActiveAilment = ailment;
            WpnAilmentAtkValue = value;
            if (WpnAilmentAtkValue > 100)
                WpnAilmentAtkValue = 100;
        }

        public byte GetElementAtkValue()
        {
            return WpnElementAtkValue;
        }

       

        public byte GetAilmentAtkValue()
        {
            return WpnAilmentAtkValue;
        }

        public short GetAtkMod()
        {
            return WpnAtkMod;
        }

        public void SetAtkMod(short val)
        {
            WpnAtkMod = val;
        }

        public short GetMagMod()
        {
            return WpnMagMod;
        }

        public void SetMagMod(short val)
        {
            WpnMagMod = val;
        }

        public int GetCount(List<Dictionary<string, byte>> list)
        {
            return list.Count;
        }

        public List<Dictionary<string, byte>> GetWeaponElementNames()
        {
            return wpnElementNames;
        }

        public List<Dictionary<string, byte>> GetWeaponAilmentNames()
        {
            return wpnAilmentNames;
        }

        public short GetMaxAttackPower()
        {
            return MAX_ATTACK;
        }

        public short GetMaxMagicPower()
        {
            return MAX_MAGICATK;
        }

        public float GetMaxAccuracy()
        {
            return MAX_ACCURACY;
        }

        #endregion

    }
}
