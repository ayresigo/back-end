using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Repositories.Interfaces
{
    public interface IAuthRepository : IDisposable
    {
        public Task<bool> checkOwnership(string address, int[] ids);
    }
}