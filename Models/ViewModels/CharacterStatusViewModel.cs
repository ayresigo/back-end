using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Models.ViewModels
{
    public class CharacterStatusViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public string BgColor { get; set; }
        public int statusDuration { get; set; }
        public long statusChanged { get; set; }
    }
}
