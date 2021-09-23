using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models
{
    public class Wife
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WantedListId { get; set; }
        public WantedList WantedList{ get; set; }
        public ICollection<Husband> Husbands { get; set; }
    }
}
