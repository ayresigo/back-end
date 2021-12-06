using back_end.Entities;
using back_end.InputModel;
using back_end.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services.Interfaces
{
    public interface ICharacterMockService
    {
        Task<CharacterViewModel> getCharacter(int id);
        Task<List<CharacterViewModel>> getCharacters(int id);
        Task<bool> addCharacter(CharacterInputModel character, string address);
        Task<Character> createCharacter();
        public Task editStatus(int id, int status, long duration, long start = 0);
        public Task editHealth(int id, int amount);
        public Task editStamina(int id, int amount);
        public Task<CharacterStatusViewModel> getStatus(int id);
        public Task<List<CharacterViewModel>> fetchCharacterStatus(int id);
    }
}
