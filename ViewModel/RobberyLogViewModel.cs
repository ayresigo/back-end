using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.ViewModel
{
    public class RobberyLogViewModel
    {
        //public int robberyId { get; set; }
        public int senderId { get; set; }
        public int characterId { get; set; }
        public int robberyId { get; set; }
        public int participants { get; set; }
        public int startMoney { get; set; }
        public int startStamina { get; set; }
        public int startHealth { get; set; }
        public int startRespect { get; set; }
        public long startDate { get; set; }
        public long endDate { get; set; }
        public int endHealth { get; set; }
        public int endMoney { get; set; }
        public int robberyStatus { get; set; }
        public int serverStatus { get; set; }
    }
}
