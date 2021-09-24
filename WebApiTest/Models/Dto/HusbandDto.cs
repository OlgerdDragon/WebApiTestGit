using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models.Dto
{
    public class HusbandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WifeId { get; set; }
    }
}
