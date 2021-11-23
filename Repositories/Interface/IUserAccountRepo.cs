using back_end.Entities;
using back_end.InputModel;
using back_end.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services
{
    public interface IUserAccountRepo : IDisposable
    {
        Task<UserAccount> getAccount(string address);
    }
}
