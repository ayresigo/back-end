﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.ViewModel
{
    public class CharacterViewModel
    {
        public int id { get; set; }
        public int owner { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string avatar { get; set; }
        public string rarity { get; set; }
        public int power { get; set; }
        public int moneyRatio { get; set; }
        public int health { get; set; }
        public int currentHealth { get; set; }
        public int stamina { get; set; }
        public int currentStamina { get; set; }
        public string job { get; set; }
        public string alignment { get; set; }
        public CharacterStatusViewModel status { get; set; }
        public long statusTime { get; set; }
        public long statusChanged { get; set; }
    }
}