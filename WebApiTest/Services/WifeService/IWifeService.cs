using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.WifeService
{
    public interface IWifeService
    {
        public Task<List<WantedProduct>> GetWantedProductsAsync(); 
        public void AddProduct(WantedProductDto wantedProductItem);
        public Task<int> SaveChangesAsync();
        public Task<WantedProduct> FindWantedProductAsync(int id);
        public void RemoveWantedProduct(WantedProduct wantedProductItem);
        public void RemoveAllWantedProducts();
    }
}
