using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;

namespace WebApiTest.Services.WifeService
{
    public interface IWifeService
    {
        public Task<List<WantedList>> GetWantedListAsync(); 
        public void AddProduct(WantedList wantedListItem);
        public Task<int> SaveChangesAsync();
        public Task<WantedList> FindWantedListAsync(int id);
        public void RemoveWantedList(WantedList wantedListItem);
        public void RemoveAllWantedList();
    }
}
