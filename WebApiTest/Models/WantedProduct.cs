using System.Collections.Generic;

namespace WebApiTest.Models
{
    public class WantedProduct
    {
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public bool BoughtStatus { get; set; }
        public ICollection<Wife> Wifes { get; set; }
    }
}
