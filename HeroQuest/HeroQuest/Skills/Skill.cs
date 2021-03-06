﻿/* This is the parent class for all skills in the game.  Includes spells. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeroQuest.Skills
{
    abstract class Skill
    {
        protected string mName;     //skill name
        protected int mPrice;       //cost of skill
        protected abstract void Activate();  //defines what a skill does
    }
}
