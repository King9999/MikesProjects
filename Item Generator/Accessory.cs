using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item_Generator
{
    class Accessory : Item
    {
        protected short AccAtk;                 //Attack Power. Determines physical damage. Minimum value is 0
        protected float AccAcc;                 //Accuracy. Hit chance is a value between 0 and 1.
        protected short AccMag;                 //Magic Power. Determines magical damage. Min value is 0. Generally applies to staff weapons, but other weapons can boost Mag.
        protected byte AccElementAtkValue;
        protected byte AccAilmentAtkValue;
        protected byte AccElementDefValue;
        protected byte AccAilmentDefValue;
        protected short AccDef;                 //Defense Power. Reduces physical damage. Minimum value is 0
        protected short AccRes;                 //Magic Resistance. Reduces magic damage.
        protected float AccEvade;

        protected List<Dictionary<string, byte>> accElementNames = new List<Dictionary<string, byte>>();
        protected List<Dictionary<string, byte>> accAilmentNames = new List<Dictionary<string, byte>>();
        protected List<Dictionary<string, byte>> accBonusNames = new List<Dictionary<string, byte>>();  //used to hold health, magic, and speed bonuses. These are prefixes.

        protected const short MAX_DEFENSE = 999;
        protected const float MAX_EVADE = 1f;   
        protected const short MAX_MAGICRES = 999;
        protected const short MAX_ATTACK = 999;
        protected const float MAX_ACCURACY = 1f;   
        protected const short MAX_MAGICATK = 999;

        public enum AccAilment : byte        //must be public so that getters/setters will work
        {
            None,
            Poison,
            Stun,
            Freeze,
            Death,
            Sleep
        }

        public enum AccElement : byte
        {
            None,
            Fire,
            Water,
            Earth,
            Wind,
            Light,
            Dark
        }

        public enum AccBonus : byte
        {
            None,
            Health,
            Magic,
            Speed
        }

        //protected AccElement AccActiveElementAtk;
        //protected AccElement AccActiveElementDef;
        //protected AccAilment AccActiveAilmentAtk;
        //protected AccAilment AccActiveAilmentDef;

        protected AccAilment AccActiveAilment;
        protected AccElement AccActiveElement;
        protected AccBonus AccActiveBonus;



        public Accessory()
        {
            AccAtk = 0;
            AccAcc = 0;
            AccMag = 0;
            AccDef = 0;
            AccRes = 0;
            AccEvade = 0;
            AccElementAtkValue = 0;
            AccAilmentAtkValue = 0;
            AccElementDefValue = 0;
            AccAilmentDefValue = 0;
            ItemHealthPointBonus = 0;
            ItemMagicPointBonus = 0;
            ItemSpeedBonus = 0;
            ItemType = "Accessory";

            /* Set up names. Prefixes for accessories are bonuses, followed by ailments, and then elements (suffixes). The value is the  
            * max percentage the weapon can have in a given tier. */
            for (int i = 0; i < Enum.GetNames(typeof(AccAilment)).Length; i++)
                accAilmentNames.Add(new Dictionary<string, byte>());

            for (int i = 0; i < Enum.GetNames(typeof(AccElement)).Length; i++)
                accElementNames.Add(new Dictionary<string, byte>());

            for (int i = 0; i < Enum.GetNames(typeof(AccBonus)).Length; i++)
                accBonusNames.Add(new Dictionary<string, byte>());

            /******Bonuses. These are the first prefix. *****/

            //health tiers. each value is a fixed amount that's added to a stat
            accBonusNames[(int)AccBonus.Health].Add("Stable", 20);
            accBonusNames[(int)AccBonus.Health].Add("Durable", 40);
            accBonusNames[(int)AccBonus.Health].Add("Enduring", 60);
            accBonusNames[(int)AccBonus.Health].Add("Tenacious", 80);
            accBonusNames[(int)AccBonus.Health].Add("Reliable", 100);
            accBonusNames[(int)AccBonus.Health].Add("Tough", 120);
            accBonusNames[(int)AccBonus.Health].Add("Reinforced", 140);
            accBonusNames[(int)AccBonus.Health].Add("Heavy", 160);
            accBonusNames[(int)AccBonus.Health].Add("Armored", 180);
            accBonusNames[(int)AccBonus.Health].Add("Impregnable", 200);

            //magic point tiers
            accBonusNames[(int)AccBonus.Magic].Add("Bright", 20);
            accBonusNames[(int)AccBonus.Magic].Add("Acute", 40);
            accBonusNames[(int)AccBonus.Magic].Add("Perceptive", 60);
            accBonusNames[(int)AccBonus.Magic].Add("Wise", 80);
            accBonusNames[(int)AccBonus.Magic].Add("Sage", 100);
            accBonusNames[(int)AccBonus.Magic].Add("Keen", 120);
            accBonusNames[(int)AccBonus.Magic].Add("Rational", 140);
            accBonusNames[(int)AccBonus.Magic].Add("Brilliant", 160);
            accBonusNames[(int)AccBonus.Magic].Add("Profound", 180);
            accBonusNames[(int)AccBonus.Magic].Add("Enlightened", 200);

            //speed tiers
            accBonusNames[(int)AccBonus.Speed].Add("Quick", 5);
            accBonusNames[(int)AccBonus.Speed].Add("Hasty", 10);
            accBonusNames[(int)AccBonus.Speed].Add("Swift", 15);
            accBonusNames[(int)AccBonus.Speed].Add("Agile", 20);
            accBonusNames[(int)AccBonus.Speed].Add("Nimble", 25);
            accBonusNames[(int)AccBonus.Speed].Add("Rapid", 30);
            accBonusNames[(int)AccBonus.Speed].Add("Brisk", 35);
            accBonusNames[(int)AccBonus.Speed].Add("Energetic", 40);
            accBonusNames[(int)AccBonus.Speed].Add("Expeditious", 45);
            accBonusNames[(int)AccBonus.Speed].Add("Spirited", 50);

            /****Ailments *****/

            //Poison tiers
            accAilmentNames[(int)AccAilment.Poison].Add("Asp's", 10);
            accAilmentNames[(int)AccAilment.Poison].Add("Cobra's", 20);
            accAilmentNames[(int)AccAilment.Poison].Add("Anaconda's", 30);
            accAilmentNames[(int)AccAilment.Poison].Add("Viper's", 40);
            accAilmentNames[(int)AccAilment.Poison].Add("Boa's", 50);
            accAilmentNames[(int)AccAilment.Poison].Add("Rattlesnake's", 60);
            accAilmentNames[(int)AccAilment.Poison].Add("Bushmaster's", 70);
            accAilmentNames[(int)AccAilment.Poison].Add("Copperhead's", 80);
            accAilmentNames[(int)AccAilment.Poison].Add("Sidewinder's", 90);
            accAilmentNames[(int)AccAilment.Poison].Add("Death Adder's", 100);


            //Console.WriteLine(accAilmentNames[(int)AccAilment.Poison].Keys);

            //Stun tiers
            accAilmentNames[(int)AccAilment.Stun].Add("Wasp's", 10);
            accAilmentNames[(int)AccAilment.Stun].Add("Jellyfish's", 20);
            accAilmentNames[(int)AccAilment.Stun].Add("Scorpion's", 30);
            accAilmentNames[(int)AccAilment.Stun].Add("Basilisk's", 40);
            accAilmentNames[(int)AccAilment.Stun].Add("Spore's", 50);
            accAilmentNames[(int)AccAilment.Stun].Add("Parasite's", 60);
            accAilmentNames[(int)AccAilment.Stun].Add("Frog's", 70);
            accAilmentNames[(int)AccAilment.Stun].Add("Spider's", 80);
            accAilmentNames[(int)AccAilment.Stun].Add("Ghoul's", 90);
            accAilmentNames[(int)AccAilment.Stun].Add("Banshee's", 100);

            //Freezing tiers
            accAilmentNames[(int)AccAilment.Freeze].Add("Chilling", 10);
            accAilmentNames[(int)AccAilment.Freeze].Add("Frosty", 20);
            accAilmentNames[(int)AccAilment.Freeze].Add("Icy", 30);
            accAilmentNames[(int)AccAilment.Freeze].Add("Biting", 40);
            accAilmentNames[(int)AccAilment.Freeze].Add("Polar", 50);
            accAilmentNames[(int)AccAilment.Freeze].Add("Glacial", 60);
            accAilmentNames[(int)AccAilment.Freeze].Add("Wintry", 70);
            accAilmentNames[(int)AccAilment.Freeze].Add("Frigid", 80);
            accAilmentNames[(int)AccAilment.Freeze].Add("Shivering", 90);
            accAilmentNames[(int)AccAilment.Freeze].Add("Icicle", 100);

            //Death tiers
            accAilmentNames[(int)AccAilment.Death].Add("Dim", 10);
            accAilmentNames[(int)AccAilment.Death].Add("Shadow", 20);
            accAilmentNames[(int)AccAilment.Death].Add("Sinister", 30);
            accAilmentNames[(int)AccAilment.Death].Add("Blackened", 40);
            accAilmentNames[(int)AccAilment.Death].Add("Death", 50);
            accAilmentNames[(int)AccAilment.Death].Add("Disastrous", 60);
            accAilmentNames[(int)AccAilment.Death].Add("Calamitous", 70);
            accAilmentNames[(int)AccAilment.Death].Add("Obliterating", 80);
            accAilmentNames[(int)AccAilment.Death].Add("Nihil", 90);
            accAilmentNames[(int)AccAilment.Death].Add("Void", 100);

            //Sleep tiers
            accAilmentNames[(int)AccAilment.Sleep].Add("Fairy's", 10);
            accAilmentNames[(int)AccAilment.Sleep].Add("Cricket's", 20);
            accAilmentNames[(int)AccAilment.Sleep].Add("Pesanta's", 30);
            accAilmentNames[(int)AccAilment.Sleep].Add("Adarna's", 40);
            accAilmentNames[(int)AccAilment.Sleep].Add("Bakhtak's", 50);
            accAilmentNames[(int)AccAilment.Sleep].Add("Owl's", 60);
            accAilmentNames[(int)AccAilment.Sleep].Add("Alp's", 70);
            accAilmentNames[(int)AccAilment.Sleep].Add("Incubus's", 80);
            accAilmentNames[(int)AccAilment.Sleep].Add("Mare's", 90);
            accAilmentNames[(int)AccAilment.Sleep].Add("Succubus's", 100);

            /*****Elements. These are suffixes, which means they are added at the end of accessory names****/

            //Fire tiers
            accElementNames[(int)AccElement.Fire].Add("of the Salamander", 10);
            accElementNames[(int)AccElement.Fire].Add("of the Drake", 20);
            accElementNames[(int)AccElement.Fire].Add("of the Kiln", 30);
            accElementNames[(int)AccElement.Fire].Add("of Magma", 40);
            accElementNames[(int)AccElement.Fire].Add("of Basalt", 50);
            accElementNames[(int)AccElement.Fire].Add("of the Furnace", 60);
            accElementNames[(int)AccElement.Fire].Add("of the Volcano", 70);
            accElementNames[(int)AccElement.Fire].Add("of the Firebird", 80);
            accElementNames[(int)AccElement.Fire].Add("of the Ifrit", 90);
            accElementNames[(int)AccElement.Fire].Add("of the Seraphim", 100);

            //Water tiers
            accElementNames[(int)AccElement.Water].Add("of the Goldfish", 10);
            accElementNames[(int)AccElement.Water].Add("of the Otter", 20);
            accElementNames[(int)AccElement.Water].Add("of the Penguin", 30);
            accElementNames[(int)AccElement.Water].Add("of the Floe", 40);
            accElementNames[(int)AccElement.Water].Add("of Waterfalls", 50);
            accElementNames[(int)AccElement.Water].Add("of Snow", 60);
            accElementNames[(int)AccElement.Water].Add("of the Polar Bear", 70);
            accElementNames[(int)AccElement.Water].Add("of the Yeti", 80);
            accElementNames[(int)AccElement.Water].Add("of Ice", 90);
            accElementNames[(int)AccElement.Water].Add("of Storms", 100);

            //Earth tiers
            accElementNames[(int)AccElement.Earth].Add("of Mud", 10);
            accElementNames[(int)AccElement.Earth].Add("of Granite", 20);
            accElementNames[(int)AccElement.Earth].Add("of Topaz", 30);
            accElementNames[(int)AccElement.Earth].Add("of Garnet", 40);
            accElementNames[(int)AccElement.Earth].Add("of the Ruby", 50);
            accElementNames[(int)AccElement.Earth].Add("of Silver", 60);
            accElementNames[(int)AccElement.Earth].Add("of Gold", 70);
            accElementNames[(int)AccElement.Earth].Add("of Platinum", 80);
            accElementNames[(int)AccElement.Earth].Add("of Diamond", 90);
            accElementNames[(int)AccElement.Earth].Add("of Obsidian", 100);

            //Wind tiers
            accElementNames[(int)AccElement.Wind].Add("of the Gull", 10);
            accElementNames[(int)AccElement.Wind].Add("of the Windmill", 20);
            accElementNames[(int)AccElement.Wind].Add("of the Blue Jay", 30);
            accElementNames[(int)AccElement.Wind].Add("of the Cloud", 40);
            accElementNames[(int)AccElement.Wind].Add("of the Sky", 50);
            accElementNames[(int)AccElement.Wind].Add("of Calm", 60);
            accElementNames[(int)AccElement.Wind].Add("of the Egret", 70);
            accElementNames[(int)AccElement.Wind].Add("of the Peacock", 80);
            accElementNames[(int)AccElement.Wind].Add("of the Eagle", 90);
            accElementNames[(int)AccElement.Wind].Add("of the Albatross", 100);

            //Light tiers
            accElementNames[(int)AccElement.Light].Add("of Dawn", 10);
            accElementNames[(int)AccElement.Light].Add("of Luminance", 20);
            accElementNames[(int)AccElement.Light].Add("of Daybreak", 30);
            accElementNames[(int)AccElement.Light].Add("of Brilliance", 40);
            accElementNames[(int)AccElement.Light].Add("of Splendor", 50);
            accElementNames[(int)AccElement.Light].Add("of Resplendence", 60);
            accElementNames[(int)AccElement.Light].Add("of Starlight", 70);
            accElementNames[(int)AccElement.Light].Add("of Radiance", 80);
            accElementNames[(int)AccElement.Light].Add("of Scintillation", 90);
            accElementNames[(int)AccElement.Light].Add("of the Sun", 100);

            //Dark tiers
            accElementNames[(int)AccElement.Dark].Add("of Dusk", 10);
            accElementNames[(int)AccElement.Dark].Add("of Gloom", 20);
            accElementNames[(int)AccElement.Dark].Add("of Shade", 30);
            accElementNames[(int)AccElement.Dark].Add("of Eventide", 40);
            accElementNames[(int)AccElement.Dark].Add("of Midnight", 50);
            accElementNames[(int)AccElement.Dark].Add("of the Umbra", 60);
            accElementNames[(int)AccElement.Dark].Add("of Spirits", 70);
            accElementNames[(int)AccElement.Dark].Add("of Sin", 80);
            accElementNames[(int)AccElement.Dark].Add("of Twilight", 90);
            accElementNames[(int)AccElement.Dark].Add("of the End", 100);
        }

        #region Getters/Setters

        public short GetAttackPower()
        {
            return AccAtk;
        }

        public void SetAttackPower(short atk)
        {
            AccAtk = atk;
            if (AccAtk < 1)
                AccAtk = 1;
            if (AccAtk > MAX_ATTACK)
                AccAtk = MAX_ATTACK;
        }

        public short GetMagicPower()
        {
            return AccMag;
        }

        public void SetMagicPower(short mag)
        {
            AccMag = mag;
            if (AccMag < 0)
                AccMag = 0;
            if (AccMag > MAX_MAGICATK)
                AccMag = MAX_MAGICATK;
        }

        public float GetAccuracy()
        {
            return AccAcc;
        }

        public void SetAccuracy(float acc)
        {
            AccAcc = acc;
            if (AccAcc > MAX_ACCURACY)
                AccAcc = MAX_ACCURACY;
        }

        public short GetDefensePower()
        {
            return AccDef;
        }

        public void SetDefensePower(short def)
        {
            AccDef = def;
            if (AccDef < 1)
                AccDef = 1;
            if (AccDef > MAX_DEFENSE)
                AccDef = MAX_DEFENSE;
        }

        public short GetMagicResist()
        {
            return AccRes;
        }

        public void SetMagicResist(short res)
        {
            AccRes = res;
            if (AccRes < 0)
                AccRes = 0;
            if (AccRes > MAX_MAGICRES)
                AccRes = MAX_MAGICRES;
        }

        public float GetEvade()
        {
            return AccEvade;
        }

        public void SetEvade(float evd)
        {
            AccEvade = evd;
            if (AccEvade > MAX_EVADE)
                AccEvade = MAX_EVADE;
        }

       /* public AccElement GetElementDef()
        {
            return AccActiveElementDef;
        }*/

        public void SetElementDef(AccElement element, byte value)
        {
            AccActiveElement = element;
            AccElementDefValue = value;
            if (AccElementDefValue > 100)
                AccElementDefValue = 100;
        }

        /*public AccAilment GetAilmentDef()
        {
            return AccActiveAilmentDef;
        }*/

        public void SetAilmentDef(AccAilment ailment, byte value)
        {
            AccActiveAilment = ailment;
            AccAilmentDefValue = value;
            if (AccAilmentDefValue > 100)
                AccAilmentDefValue = 100;
        }

        /*public AccElement GetElementAtk()
        {
            return AccActiveElementAtk;
        }*/

        public void SetElementAtk(AccElement element, byte value)
        {
            AccActiveElement = element;
            AccElementAtkValue = value;
            if (AccElementAtkValue > 100)
                AccElementAtkValue = 100;
        }

        public AccAilment GetAilment()
        {
            return AccActiveAilment;
        }

        public AccElement GetElement()
        {
            return AccActiveElement;
        }

        public void SetAilmentAtk(AccAilment ailment, byte value)
        {
            AccActiveAilment = ailment;
            AccAilmentAtkValue = value;
            if (AccAilmentAtkValue > 100)
                AccAilmentAtkValue = 100;
        }

        public AccBonus GetBonus()
        {
            return AccActiveBonus;
        }

        /*public void SetBonus(AccBonus bonus, byte value)
        {
            AccActiveBonus = bonus;
            AccBonusValue = value;
            if (AccAilmentDefValue > 100)
                AccAilmentDefValue = 100;

        }*/

        public byte GetElementDefValue()
        {
            return AccElementDefValue;
        }



        public byte GetAilmentDefValue()
        {
            return AccAilmentDefValue;
        }

        public byte GetAilmentAtkValue()
        {
            return AccAilmentAtkValue;
        }

        public byte GetElementAtkValue()
        {
            return AccElementAtkValue;
        }

        public int GetCount(List<Dictionary<string, byte>> list)
        {
            return list.Count;
        }

        public List<Dictionary<string, byte>> GetAccElementNames()
        {
            return accElementNames;
        }

        public List<Dictionary<string, byte>> GetAccAilmentNames()
        {
            return accAilmentNames;
        }

        public List<Dictionary<string, byte>> GetAccBonusNames()
        {
            return accBonusNames;
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
