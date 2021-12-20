using cryminals.Models.InputModels;
using cryminals.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Repositories.Interfaces
{
    public interface IRobberyRepository : IDisposable
    {
        public Task<List<RobberyViewModel>> getRobberies(int status);
        public Task<RobberyViewModel> getRobbery(int id);
        public Task<string> startRobbery(StartRobberyInputModel data);
        public Task<List<RobberyEventViewModel>> getRobberyEvent(int characterId, int claimed, int status);
        public Task endRobberyEvent(int robberyEventId, int status);
    }
}