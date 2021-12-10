using cryminals.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Repositories.Interfaces
{
    public interface IAccountRepository : IDisposable
    {
        Task<AccountViewModel> getAccount(string address);
        Task<AccountViewModel> fetchAccount(string token);
        Task<List<CharacterViewModel>> getCharacters(string address);
        Task<List<CharacterViewModel>> fetchCharacters(string token);
    }
}
