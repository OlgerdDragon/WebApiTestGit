using System.Collections.Generic;

namespace WebApiGeneralGrpc.Models
{
    public class Wife
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<WantedProduct> WantedProducts { get; set; }
        public virtual Husband Husbands { get; set; }
        public int PersonId { get; set; }
        public virtual Person Persons { get; set; }
    }
}
