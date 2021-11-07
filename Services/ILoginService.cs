using back_end.InputModel;
using back_end.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services
{
    public interface ILoginService
    {
        Task<UserAccountViewModel> getAccount(string address);
        Task<bool> checkSignature(MessageInputModel request);
    }
}
