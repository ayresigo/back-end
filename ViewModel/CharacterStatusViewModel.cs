using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.ViewModel
{
    public class CharacterStatusViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string iconColor { get; set; }
        public string bgColor { get; set; }        
    }
}
