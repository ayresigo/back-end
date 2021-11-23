using back_end.Repositories;
using back_end.ViewModel;
using back_end.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _accountRepo;

        public AccountService(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<bool> createAccount(string address)
        {
            await _accountRepo.createAccount(address);
            return true;
        }

        public async Task<bool> editUsername(string address, string username)
        {
            if (await _accountRepo.editUsername(address, username)) return true; else return false;
        }

        public async Task<AccountViewModel> getAccount(string address)
        {
            var account = await _accountRepo.getAccount(address);
            if (account == null) return null;

            return new AccountViewModel
            {
                id = account.Id,
                username = account.Username,
                address = account.Address,
                avatar = account.Avatar,
                money = account.Money,
                respect = account.Respect,
                totalPower = account.TotalPower,
                status = account.Status
            };
        }
    }
}
