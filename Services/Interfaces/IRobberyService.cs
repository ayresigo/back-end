using back_end.ViewModel;
using back_end.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services.Interfaces
{
    public interface IRobberyService
    {
        public Task<RobberyViewModel> getRobbery(int id);
        public Task<List<RobberyViewModel>> getRobberies(int status);
        public Task addLogs(RobberyLogViewModel log);
        public Task<List<RobberyLogViewModel>> getCharacterRobberyLogs(int characterId);
        public Task startRobbery(StartRobberyInputModel input);
    }
}
