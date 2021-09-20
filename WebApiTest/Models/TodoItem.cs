 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedDateTime { get; set; }
        
    }
}
