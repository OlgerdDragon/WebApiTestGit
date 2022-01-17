using TownContextForWebService.Models;

namespace HusbandGrpcService.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int ShopId { get; set; }

        public Product Product(Shop shop)
        {
            return new Product
            {
                Name = this.Name,
                Price = this.Price,
                ShopId = this.Price,
                Shops = shop
            };
        }
        public static ProductDto ItemProductDTO(Product productItem) =>
           new ProductDto
           {
               Id = productItem.Id,
               Name = productItem.Name,
               Price = productItem.Price,
               ShopId = productItem.ShopId
           };
        public static ProductDtoMessage ItemProductDTOMessage(Product productItem) =>
          new ProductDtoMessage
          {
              Id = productItem.Id,
              Name = productItem.Name,
              Price = productItem.Price,
              ShopId = productItem.ShopId
          };
    }
}
