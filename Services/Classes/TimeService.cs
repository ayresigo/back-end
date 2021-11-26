using back_end.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services.Classes
{
    public class TimeService : ITimeService
    {
        public Task<long> getTime()
        {
            return Task.FromResult(DateTimeOffset.Now.ToUnixTimeSeconds());
        }
    }
}
