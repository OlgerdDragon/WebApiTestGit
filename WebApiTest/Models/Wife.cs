using System.Collections.Generic;

namespace WebApiTest.Models
{
    public class Wife
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WantedProductId { get; set; }
        public WantedProduct WantedProduct { get; set; }
        public ICollection<Husband> Husbands { get; set; }
    }
}
