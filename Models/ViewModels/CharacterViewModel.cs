using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Models.ViewModels
{
    public class CharacterViewModel
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public long Seed { get; set; }
        public string Rarity { get; set; }
        public int Power { get; set; }
        public int Health { get; set; }
        public int CurrentHealth { get; set; }
        public int Stamina { get; set; }
        public int CurrentStamina { get; set; }
        public string Job { get; set; }
        public string Affiliation { get; set; }
        public CharacterStatusViewModel Status { get; set; }
        public long CreationDate { get; set; }
    }
}
