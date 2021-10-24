using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Exceptions
{
    public class AccountAlreadyExists : Exception
    {
        public AccountAlreadyExists()
            : base("Account Already Registered!")
        { }
    }
}


