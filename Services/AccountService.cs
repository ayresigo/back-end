using back_end.Entities;
using back_end.Exceptions;
using back_end.InputModel;
using back_end.Repositories;
using back_end.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountViewModel> addAccount(AccountInputModel account)
        {
            var _account = await _accountRepository.getAccount(account.Id);

            if (_account != null)
                throw new AccountAlreadyExists();

            var accountInsert = new Account
            {
                Id = account.Id,
                Address = account.Address,
                Username = account.Username,
                Ban = false
            };

            await _accountRepository.addAccount(accountInsert);

            return new AccountViewModel
            {
                Id = account.Id,
                Address = account.Address,
                Username = account.Username,
                Ban = account.Ban
            };
        }

        public async Task banAccount(string address, bool ban = true)
        {
            var account = _accountRepository.getAccount(address);

            if (account == null)
                throw new AccountNotFound();

            await _accountRepository.banAccount(address, ban);
        }

        public async Task banAccount(int id, bool ban = true)
        {
            var account = _accountRepository.getAccount(id);

            if (account == null)
                throw new AccountNotFound();

            await _accountRepository.banAccount(id, ban);
        }

        public async Task deleteAccount(int id)
        {
            var account = await _accountRepository.getAccount(id);

            if (account == null)
                throw new AccountNotFound();

            await _accountRepository.deleteAccount(id);
        }

        public async Task deleteAccount(string address)
        {
            var account = await _accountRepository.getAccount(address);

            if (account == null)
                throw new AccountNotFound();

            await _accountRepository.deleteAccount(address);
        }

        public async Task<AccountViewModel> editAccount(int id, AccountInputModel account)
        {
            var _account = await _accountRepository.getAccount(id);

            if (_account == null)
                return null;

            var accountInsert = new Account
            {
                Id = account.Id,
                Address = account.Address,
                Username = account.Username,
                Ban = false
            };

            if (accountInsert.Id != _account.Id)
            {
                var checkId = await _accountRepository.getAccount(accountInsert.Id);
                if (checkId != null)
                    throw new AccountAlreadyExists();
            }

            if (accountInsert.Address != _account.Address)
            {
                var checkAddress = await _accountRepository.getAccount(accountInsert.Address);
                if (checkAddress != null)
                    throw new AccountAlreadyExists();
            }

            await _accountRepository.addAccount(accountInsert);

            return new AccountViewModel
            {
                Id = account.Id,
                Address = account.Address,
                Username = account.Username,
                Ban = account.Ban
            };
        }

        public async Task<AccountViewModel> editAccount(string address, AccountInputModel account)
        {
            var _account = await _accountRepository.getAccount(address);

            if (_account == null)
                return null;

            var accountInsert = new Account
            {
                Id = account.Id,
                Address = account.Address,
                Username = account.Username,
                Ban = false
            };

            if (accountInsert.Id != _account.Id)
            {
                var checkId = await _accountRepository.getAccount(accountInsert.Id);
                if (checkId != null)
                    throw new AccountAlreadyExists();
            }

            if (accountInsert.Address != _account.Address)
            {
                var checkAddress = await _accountRepository.getAccount(accountInsert.Address);
                if (checkAddress != null)
                    throw new AccountAlreadyExists(); 
            }

            await _accountRepository.addAccount(accountInsert);

            return new AccountViewModel
            {
                Id = account.Id,
                Address = account.Address,
                Username = account.Username,
                Ban = account.Ban
            };
        }

        public async Task<List<AccountViewModel>> getAccount(int page, int qtd)
        {
            var accounts = await _accountRepository.getAccount(page, qtd);

            return accounts.Select(account => new AccountViewModel
            {
                Id = account.Id,
                Address = account.Address,
                Username = account.Username,
                Ban = account.Ban
            }).ToList();
        }

        public async Task<AccountViewModel> getAccount(int id)
        {
            var account = await _accountRepository.getAccount(id);

            if (account == null)
                return null;

            return new AccountViewModel
            {
                Id = account.Id,
                Address = account.Address,
                Username = account.Username,
                Ban = account.Ban
            };
        }

        public async Task<AccountViewModel> getAccount(string address)
        {
            var account = await _accountRepository.getAccount(address);

            if (account == null)
                return null;

            return new AccountViewModel
            {
                Id = account.Id,
                Address = account.Address,
                Username = account.Username,
                Ban = account.Ban
            };
        }
        
        public void Dispose()
        {
            _accountRepository?.Dispose();
        }
    }
}
