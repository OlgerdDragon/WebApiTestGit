using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.FactoryService
{
    public class FactoryService : IFactoryService
    {
        public Product Product (ProductDto productItemDto, Shop shop)
        {
            return new Product
            {
                //Id = productItemDto.Id,
                Name = productItemDto.Name,
                Price = productItemDto.Price,
                ShopId = productItemDto.Price,
                Shop = shop
                
            };
        }
        public Shop Shop (ShopDto productItemDto)
        {
            var r = new Shop
            {
                //Id = productItemDto.Id,
                Name = productItemDto.Name
            };
            return r;
        }
        public WantedProduct WantedList (WantedProductDto productItemDto)
        {
            return new WantedProduct
            {
                //Id = productItemDto.Id,
                NameProduct = productItemDto.NameProduct,
                BoughtStatus = productItemDto.BoughtStatus
            };
        }
    }
}
