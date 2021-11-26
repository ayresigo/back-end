using back_end.ViewModel;
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
    }
}
