using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Exceptions
{
    public class SignatureValidationException : Exception
    {
        public SignatureValidationException() : base ("Signature Validation Failed.")
        {

        }
    }
}
