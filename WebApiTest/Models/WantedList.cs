using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models
{
    public class WantedList
    {
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public bool BoughtStatus { get; set; }
        public ICollection<Wife> Wifes { get; set; }
    }
}
