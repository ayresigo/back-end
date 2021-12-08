using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Models.ViewModels
{
    public class TokenDataViewModel
    {
        public string address { get; set; }
        public string signature { get; set; }
        public int message { get; set; }
        public int iat { get; set; }
        public int exp { get; set; }
    }

}
