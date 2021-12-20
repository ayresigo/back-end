using cryminals.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryminals.Repositories.Interfaces
{
    public interface IItemRepository : IDisposable
    {
        public Task<ItemDBViewModel> getItemDB(int id);
        public Task<List<ItemViewModel>> fetchItems(string token);
    }
}
