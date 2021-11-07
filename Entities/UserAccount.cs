using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entities
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public int Money { get; set; }
        public int Respect { get; set; }
        public int TotalPower { get; set; }
        public int Status { get; set; }
    }
}
