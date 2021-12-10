using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Models.ViewModels
{
    public class AccountViewModel
    {
        public string Address { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public int Money { get; set; }
        public int Respect { get; set; }
        public int Status { get; set; }
        public long CreationDate { get; set; }
    }
}
