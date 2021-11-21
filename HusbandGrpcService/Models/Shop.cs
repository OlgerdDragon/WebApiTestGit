﻿using System.Collections.Generic;

namespace HusbandGrpcService.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products {get; set;}
        
    }
}
