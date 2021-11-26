using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.InputModel
{
    public class Characters
    {
        public int characterId { get; set; }
    }
    public class StartRobberyInputModel
    {
        public int robberyId { get; set; }
        public string token { get; set; }
        public Characters[] participants { get; set; }
    }
}
