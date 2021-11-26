using back_end.Repositories.Interface;
using back_end.Services.Interfaces;
using back_end.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services.Classes
{
    public class RobberyService : IRobberyService
    {
        private readonly IRobberyRepo _robberyRepo;

        public RobberyService(IRobberyRepo robberyRepo)
        {
            _robberyRepo = robberyRepo;
        }

        public async Task<List<RobberyViewModel>> getRobberies(int status)
        {
            return await _robberyRepo.getRobberies(status);
        }

        public async Task<RobberyViewModel> getRobbery(int id)
        {
            return await _robberyRepo.getRobbery(id);
        }

        public async Task addLogs(RobberyLogViewModel log)
        {
            await _robberyRepo.addLogs(log);
        }
    }
}
