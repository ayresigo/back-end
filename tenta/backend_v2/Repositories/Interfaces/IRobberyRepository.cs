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
    }
}
