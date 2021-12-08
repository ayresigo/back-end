﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Exceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string param) : base("Invalid Input(" + param + ").")
        {
        }
    }
}
