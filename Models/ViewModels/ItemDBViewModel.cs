using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Models.ViewModels
{
    public class ItemDBViewModel
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public string Description { get; set; }
        public string Rarity { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
    }
}
