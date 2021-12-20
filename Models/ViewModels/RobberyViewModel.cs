using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Models.ViewModels
{
    public class RobberyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Reward { get; set; }
        public int Duration { get; set; }
        public int Stamina { get; set; }
        public int Power { get; set; }
        public int MinParticipants { get; set; }
        public int MaxParticipants { get; set; }
        public int AmbushRisk { get; set; }
        public int PrisonRisk { get; set; }
        public int DeathRisk { get; set; }
        public string Background { get; set; }
    }
}
