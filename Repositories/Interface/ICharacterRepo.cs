using back_end.InputModel;
using back_end.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositories.Interface
{
    public interface ICharacterRepo : IDisposable
    {
        public Task<bool> addCharacter(CharacterInputModel character, string address);
        public Task<CharacterViewModel> getCharacter(int id);
        public Task<List<CharacterViewModel>> getCharacters(int id);
    }
}
