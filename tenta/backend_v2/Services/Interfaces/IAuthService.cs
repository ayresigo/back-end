using cryminals.Models.InputModels;
using cryminals.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Services.Interfaces
{
    public  interface IAuthService
    {
        string generateToken(SignatureInputModel data);
        TokenDataViewModel retrieveTokenData(string token);
        bool validateSignature(SignatureInputModel signature);
    }
}
