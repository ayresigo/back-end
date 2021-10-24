using back_end.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositories
{
    public interface IAccountRepository : IDisposable
    {       
        Task<List<Account>> getAccount(int page, int qtd);
        Task<Account> getAccount(int id);
        Task<Account> getAccount(string address);
        Task addAccount(Account account);
        Task editAccount(int id, Account account);
        Task editAccount(string address, Account account);
        Task banAccount(string address, bool ban = true);
        Task banAccount(int id, bool ban = true);
        Task deleteAccount(int id);
        Task deleteAccount(string address);
    }
}
