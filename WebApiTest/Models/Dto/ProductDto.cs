using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models.Dto
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
                Shop = shop
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
    }
}
