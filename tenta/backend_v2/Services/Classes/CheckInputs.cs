using cryminals.Exceptions;
using cryminals.Services.Interfaces;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cryminals.Services.Classes
{
    public class CheckInputs : ICheckInputs
    {
        public bool checkToken(string token)
        {
            Regex rx = new Regex(@"^eyJ[a-zA-Z0-9-_=]+\.eyJ[a-zA-Z0-9-_=]+\.?[A-Za-z0-9-_.+/=]*$");
            if (rx.Match(token).Success && !String.IsNullOrEmpty(token))
                return true;
            else throw new InvalidInputException("token");
        }

        public bool checkAddress(string address)
        {
            var addrValidator = new AddressUtil();
            Regex rx = new Regex("^0x[a-fA-F0-9]{40}$");
            if (rx.Match(address).Success && !addrValidator.IsAnEmptyAddress(address)
                && addrValidator.IsValidEthereumAddressHexFormat(address))
                return true;
            else throw new InvalidInputException("address");
        }

        public bool checkBase64(string input)
        {
            Regex rx = new Regex("^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4})$");
            if (rx.Match(input).Success && !String.IsNullOrEmpty(input))
                return true;
            else throw new InvalidInputException("base64");
        }

        public bool checkHexHash(string hexHash)
        {
            Regex rx = new Regex("^0x[a-fA-F0-9]*$");
            if (rx.Match(hexHash).Success && !String.IsNullOrEmpty(hexHash)) return true;
            else throw new InvalidInputException("hex hash");
        }

        public bool checkInt(int value, int min = 1, int max = 2147483647)
        {
            if (value >= min && value <= max)
                return true;
            else if (value < min)
                throw new InvalidInputException("value < min");
            else if (value > max)
                throw new InvalidInputException("value > max");
            else throw new InvalidInputException("int");
        }
    }
}
