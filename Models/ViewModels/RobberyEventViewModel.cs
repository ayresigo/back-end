using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Models.ViewModels
{
    public class RobberyEventViewModel
    {
        public int Id { get; set; }
        public int RobberyId { get; set; }
        public int CharacterId { get; set; }
        public long Started { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; }
        public long Claimed { get; set; }
    }
}


