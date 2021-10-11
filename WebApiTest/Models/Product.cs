
namespace WebApiTest.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int ShopId { get; set; }
        public virtual Shop Shop { get; set; }

    }
}
