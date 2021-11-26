using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services.Interfaces
{
    public interface ITimeService
    {
        public Task<long> getTime();
    }
}
