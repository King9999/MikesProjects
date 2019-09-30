/* This is the base class for all effects in the game. This includes both positive and negative effects. 
 The class takes a parameter which is used to call the desired effect. */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeroQuest.Heroes;
using HeroQuest.Monsters;
using HeroQuest.Items;
using HeroQuest.Skills;




namespace HeroQuest.Effects
{
    class Effect
    {
        Hero hero;
        Monster monster;
        Item item;
        Skill skill;

        //constants for effects. This is public so it can be used elsewhere.
       public enum Effects
       {
           Heal, Poison
       };

       Effects effect;

       public Effect(Effects effectID)
       {
           effect = effectID;
       }

       public string GetEffectOnHero(Effects id, Hero hero)
       {
           string effectText;
           switch (id)
           {
               case Effects.Heal:
                   hero.ModifyBodyPoints(1);
                   effectText = "Hero restored BP!";
                   break;
               case Effects.Poison:
                   hero.ModifyBodyPoints(-1);
                   effectText = "Hero is poisoned!";
                   break;
               default:
                   effectText = "";
                   break;
           }

           return effectText;
       }

    }
}
