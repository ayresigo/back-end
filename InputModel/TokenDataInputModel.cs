using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.ViewModel
{
    public class TokenDataViewModel
    {
        public int accountId { get; set; }
        public string address { get; set; }
        public string message { get; set; }
        public string signature { get; set; }
    }
}
