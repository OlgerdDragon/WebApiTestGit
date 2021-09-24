using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.FactoryService
{
    public interface IFactoryService
    {
        public Product Product(ProductDto productItemDto);
        
        public Shop Shop(ShopDto productItemDto);
        public WantedList WantedList(WantedListDto productItemDto);
    }
}
