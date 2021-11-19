using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.InputModel
{
    public class SignatureInputModel
    {
        public string Address { get; set; }
        public string Message { get; set; }
        public string Signature { get; set; }
    }
}
