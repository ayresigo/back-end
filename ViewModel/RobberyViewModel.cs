using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.ViewModel
{
    public class RobberyViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public int difficulty { get; set; }
        public int time { get; set; }
        public int power { get; set; }
        public int stamina { get; set; }
        public int reward { get; set; }
        public int minPart { get; set; }
        public int maxPart { get; set; }
        public int status { get; set; }
    }
}
