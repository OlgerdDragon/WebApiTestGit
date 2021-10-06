using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models.Dto
{
    public class ShopDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Shop Shop()
        {
            return new Shop
            {
                Name = this.Name
            };
        }

    }
}
