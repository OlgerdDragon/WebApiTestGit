using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.FactoryService
{
    public interface IFactoryService
    {
        public Product Product(ProductDto productItemDto, Shop shop);
        
        public Shop Shop(ShopDto productItemDto);
        public WantedProduct WantedProduct(WantedProductDto productItemDto);
    }
}
