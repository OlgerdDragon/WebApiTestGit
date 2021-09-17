using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Models;

namespace WebApiTest
{
    public class Factory
    {
        public static Product Product(int id, string productName, int productPrice)
        {
            Product product = new Product();
            product.Id = id;
            product.Name = productName;
            product.Price = productPrice;
            return product;
        }
               
    }
}
