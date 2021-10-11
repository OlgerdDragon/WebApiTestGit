using System.Collections.Generic;

namespace WebApiTest.Models
{
    public class Wife
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WantedProductId { get; set; }
        public virtual WantedProduct WantedProduct { get; set; }
        public virtual ICollection<Husband> Husbands { get; set; }
    }
}
