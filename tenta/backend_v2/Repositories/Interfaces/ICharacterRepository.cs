using cryminals.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Repositories.Interfaces
{
    interface ICharacterRepository : IDisposable
    {
        Task<CharacterViewModel> getCharacter(int id);
    }
}
