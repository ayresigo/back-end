using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Exceptions
{
    public class AccountNotFound : Exception
    {
        public AccountNotFound()
            : base("Account not found!")
        { }
    }

}
