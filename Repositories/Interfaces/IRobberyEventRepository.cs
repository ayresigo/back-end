using cryminals.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Repositories.Interfaces
{
    public interface IRobberyEventRepository : IDisposable
    {
        public Task startRobbery(CharacterViewModel character, RobberyViewModel robbery);
    }
}
