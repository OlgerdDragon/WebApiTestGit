using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;


namespace WebApiTest.Services.HusbandService
{
    public interface IHusbandService
    {
        public Task<List<WantedList>> GetWantedListAsync();
        public Task<Shop> FindShopAsync(long id);
        public Task<Product> FindProductAsync(string name);
    }
}
