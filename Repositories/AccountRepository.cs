using back_end.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private static Dictionary<int, Account> accounts = new Dictionary<int, Account>()
        {
            {1, new Account{Id = 1, Address = "0x8Da798034D863DaE4C16BDA71B2E03Fc8B3259A3", Username = "Ayresigo"} },
            {2, new Account{Id = 2, Address = "0xCCD43Eb41fD89f0f862be29eAFc8FB9228919d86", Username = "Manur"} }
        };

        public Task addAccount(Account account)
        {
            accounts.Add(account.Id, account);
            return Task.CompletedTask;
        }

        //teste
        public Task banAccount(string address, bool ban = true)
        {
            List<Account> _accounts = accounts.Values.Where(account => account.Address.Equals(address)).ToList();
            accounts[_accounts[0].Id].Ban = ban;
            return Task.CompletedTask;
        }

        public Task banAccount(int id, bool ban = true)
        {
            accounts[id].Ban = ban;
            return Task.CompletedTask;
        }

        public Task deleteAccount(int id)
        {
            accounts.Remove(id);
            return Task.CompletedTask;
        }

        //teste
        public Task deleteAccount(string address)
        {
            List<Account> _accounts = accounts.Values.Where(account => account.Address.Equals(address)).ToList();
            accounts.Remove(_accounts[0].Id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // nada
        }

        public Task editAccount(int id, Account account)
        {
            accounts[account.Id] = account;
            return Task.CompletedTask;
        }

        //Teste
        public Task editAccount(string address, Account account)
        {
            List<Account> _accounts = accounts.Values.Where(account => account.Address.Equals(address)).ToList();
            accounts[_accounts[0].Id] = account;
            return Task.CompletedTask;
        }

        public Task<List<Account>> getAccount(int page, int qtd)
        {
            return Task.FromResult(accounts.Values.Skip((page - 1) * qtd).Take(qtd).ToList());
        }

        public Task<Account> getAccount(int id)
        {
            if (!accounts.ContainsKey(id))
                return null;

            return Task.FromResult(accounts[id]);
        }

        //Teste
        public Task<Account> getAccount(string address)
        {
            List<Account> _accounts = accounts.Values.Where(account => account.Address.Equals(address)).ToList();
            return Task.FromResult(_accounts[0]);
        }
    }
}
