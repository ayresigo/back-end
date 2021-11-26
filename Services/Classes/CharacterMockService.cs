using back_end.Entities;
using back_end.InputModel;
using back_end.Repositories.Interface;
using back_end.Services.Interfaces;
using back_end.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services
{
    public class CharacterMockService : ICharacterMockService
    {
        private readonly ICharacterRepo _characterRepo;

        public CharacterMockService(ICharacterRepo characterRepo)
        {
            _characterRepo = characterRepo;
        }

        private Random rng = new Random();
        public async Task<bool> addCharacter(CharacterInputModel character, string address)
        {
            await _characterRepo.addCharacter(character, address);
            return true;
        }

        public async Task<Character> createCharacter()
        {
            Character character = new Character();

            character.name = "wip";

            //gender selection
            float op = rng.Next(1, 101);
            if (op <= 37.5)
            {
                character.gender = 1;
            }
            else if (op > 37.5 && op <= 75)
            {
                character.gender = 2;
            }
            else
            {
                character.gender = 3;
            }

            //rarity selection
            op = rng.Next(1, 101);
            if (op <= 60)
            {
                character.rarity = 1;
            }
            else if (op > 60 && op <= 90)
            {
                character.rarity = 2;
            }
            else if (op > 90 && op <= 99)
            {
                character.rarity = 3;
            }
            else
            {
                character.rarity = 4;
            }

            // status gen
            switch (character.rarity)
            {
                case 1:
                    character.power = rng.Next(200, 400);
                    character.health = rng.Next(100, 300);
                    character.stamina = rng.Next(75, 125);
                    break;
                case 2:
                    character.power = rng.Next(400, 600);
                    character.health = rng.Next(250, 500);
                    character.stamina = rng.Next(125, 175);
                    break;
                case 3:
                    character.power = rng.Next(600, 950);
                    character.health = rng.Next(500, 750);
                    character.stamina = rng.Next(175, 250);
                    break;
                case 4:
                    character.power = rng.Next(1000, 2000);
                    character.health = rng.Next(800, 1200);
                    character.stamina = rng.Next(250, 400);
                    break;
                default: break;
            }

            character.avatar = "none";
            character.job = "none";
            character.alignment = "none";
            character.moneyRatio = 0;

            return character;
        }

        public async Task<CharacterViewModel> getCharacter(int id)
        {
            var character = await _characterRepo.getCharacter(id);
            return new CharacterViewModel
            {
                id = character.id,
                owner = character.owner,
                name = character.name,
                gender = character.gender,
                avatar = character.avatar,
                rarity = character.rarity,
                power = character.power,
                moneyRatio = character.moneyRatio,
                health = character.health,
                stamina = character.stamina,
                job = character.job,
                alignment = character.alignment,
                status = character.status,
                statusTime = character.statusTime,
                statusChanged = character.statusChanged
            };
        }

        public async Task<List<CharacterViewModel>> getCharacters(int id)
        {
            var characters = await _characterRepo.getCharacters(id);
            return characters;
        }

        public async Task editStatus(int id, int status = 1, long duration = -1, long start = 0)
        {
            await _characterRepo.editStatus(id, status, duration, start);
        }
    }
}
