using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services.Interfaces
{
    public interface ICheckInputs
    {
        public bool checkInt(int number, int min = 0, int max = 2147483647);
        public bool checkAddress(string address);
        public bool checkBase64Input(string input);
    }
}
