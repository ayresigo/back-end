using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public bool Ban { get; set; }
    }
}
