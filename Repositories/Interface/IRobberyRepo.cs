using back_end.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositories.Interface
{
    public interface IRobberyRepo : IDisposable
    {
        public Task<RobberyViewModel> getRobbery(int id);
        public Task<List<RobberyViewModel>> getRobberies(int status);
        public Task startRobbery(int robbery, string sender, int[] participants);
        public Task addLogs(RobberyLogViewModel log);
        public Task<List<RobberyLogViewModel>> getCharacterRobberyLogs(int characterId);

    }
}
