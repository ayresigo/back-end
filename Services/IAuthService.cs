using back_end.InputModel;
using back_end.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services
{
    public interface IAuthService 
    {
        Task<TokenViewModel> generateToken(TokenDataInputModel data, string secret);
        Task<string> retrieveToken(TokenInputModel token);
        Task<bool> checkSignature(SignatureInputModel request);

    }
}
