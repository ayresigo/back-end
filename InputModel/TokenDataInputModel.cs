using back_end.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.InputModel
{
    public class Signature
    {
        public string message { get; set; }
        public string signature { get; set; }
    }

    public class TokenDataInputModel
    {
        public int id { get; set; }
        public string address { get; set; }
        public Signature signature { get; set; }
        public int exp { get; set; }
    }
}
