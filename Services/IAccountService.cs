using back_end.InputModel;
using back_end.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services
{
    public interface IAccountService : IDisposable
    {
        Task<List<AccountViewModel>> getAccount(int page, int qtd);
        Task<AccountViewModel> getAccount(int id);
        Task<AccountViewModel> getAccount(string address);
        Task<AccountViewModel> addAccount(AccountInputModel account);
        Task<AccountViewModel> editAccount(int id, AccountInputModel account);
        Task<AccountViewModel> editAccount(string address, AccountInputModel account);
        Task banAccount(string address, bool ban = true);
        Task banAccount(int id, bool ban = true);
        Task deleteAccount(int id);
        Task deleteAccount(string address);
    }
}
