using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.ViewModel
{
    public class AccountViewModel
    {
        public int id { get; set; }
        public string address { get; set; }
        public string username { get; set; }
        public string avatar { get; set; }
        public int money { get; set; }
        public int respect { get; set; }
        public int totalPower { get; set; }
        public int status { get; set; }
    }
}
