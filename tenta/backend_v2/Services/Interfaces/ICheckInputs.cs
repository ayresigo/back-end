using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Services.Interfaces
{
    public interface ICheckInputs
    {
        public bool checkBase64(string input);
        public bool checkAddress(string address);
        public bool checkToken(string token);
        public bool checkInt(int value, int min = 1, int max = 2147483647);
        public bool checkHexHash(string hexHash);
    }
}
