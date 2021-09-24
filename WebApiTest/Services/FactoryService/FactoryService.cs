using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.FactoryService
{
    public class FactoryService : IFactoryService
    {
        public Product Product (ProductDto productItemDto)
        {
            return new Product
            {
                Id = productItemDto.Id,
                Name = productItemDto.Name,
                Price = productItemDto.Price,
                ShopId = productItemDto.Price
            };
        }
        public Shop Shop (ShopDto productItemDto)
        {
            var r = new Shop
            {
                Id = productItemDto.Id,
                Name = productItemDto.Name
            };
            return r;
        }
        public WantedList WantedList (WantedListDto productItemDto)
        {
            return new WantedList
            {
                Id = productItemDto.Id,
                NameProduct = productItemDto.NameProduct,
                BoughtStatus = productItemDto.BoughtStatus
            };
        }
    }
}
