using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Models.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public ItemDBViewModel Item { get; set; }
        public string Owner { get; set; }
        public int EquippedBy { get; set; }
        public int Durability { get; set; }
    }
}
