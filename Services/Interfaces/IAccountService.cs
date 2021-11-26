using back_end.InputModel;
using back_end.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services
{
    public interface IAccountService
    {
        public Task<AccountViewModel> getAccount(string address);
        public Task<bool> editUsername(string address, string username);
        public Task<bool> createAccount(string address);

    }
}
