using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Models.InputModels
{
    public class SignatureInputModel
    {
        public string Address { get; set; }
        public int Message { get; set; }
        public string Signature { get; set; }
    }
}
