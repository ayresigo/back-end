using cryminals.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Repositories.Interfaces
{
    public interface ICharacterRepository : IDisposable
    {
        public Task<CharacterViewModel> getCharacter(int id);
        public Task<List<CharacterViewModel>> fetchCharacters(string token);
        public Task<List<CharacterViewModel>> getCharacters(string address);
    }
}
