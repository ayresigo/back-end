using back_end.Services.Interfaces;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace back_end.Services.Classes
{
    public class CheckInputs : ICheckInputs
    {
        public bool checkAddress(string address)
        {
            var addrValidator = new AddressUtil();
            Regex rx = new Regex("^0x[a-fA-F0-9]{40}$");
            if (rx.Match(address).Success && !addrValidator.IsAnEmptyAddress(address)
                && addrValidator.IsValidEthereumAddressHexFormat(address))
                return true;
            else
                return false;
        }

        public bool checkBase64Input(string input)
        {
            Regex rx = new Regex("^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4})$"); //base64
            if (rx.Match(input).Success)
                return true;
            else
                return false;
        }

        public bool checkInt(int number, int min = 0, int max = int.MaxValue)
        {
            if (number >= min && number <= max)
                return true;
            else return false;
        }
    }
}
