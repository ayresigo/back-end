using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.ViewModel
{
    public class Signature
    {
        public string message { get; set; }
        public string signature { get; set; }
    }

    public class TokenDataViewModel
    {
        public int id { get; set; }
        public string address { get; set; }
        public Signature signature { get; set; }
        public int iat { get; set; }
        public int exp { get; set; }
    }
}
