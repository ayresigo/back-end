using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Exceptions
{
    public class TokenGenerationException : Exception
    {
        public TokenGenerationException() : base ("Token generation failed.")
        {

        }
    }
}
