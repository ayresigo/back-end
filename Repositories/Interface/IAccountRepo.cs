using back_end.Entities;
using back_end.InputModel;
using back_end.ViewModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositories
{
    public interface IAccountRepo : IDisposable
    {
        public Task<Account> getAccount(string address);
        public Task<bool> editUsername(string address, string username);
        public Task<bool> createAccount(string address);
        
    }
}
