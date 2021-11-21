using System.Collections.Generic;

namespace HusbandGrpcService.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int ShopId { get; set; }
        public virtual Shop Shops { get; set; }
        public virtual ICollection<WantedProduct> WantedProducts { get; set; }

    }
}
