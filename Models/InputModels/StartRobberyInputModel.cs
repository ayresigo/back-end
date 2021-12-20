using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Models.InputModels
{
    public class StartRobberyInputModel
    {
        public string Token { get; set; }
        public int[] Participants { get; set; }
        public int RobberyId { get; set; }
    }
}