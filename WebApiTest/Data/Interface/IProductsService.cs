using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Models;

namespace WebApiTest.Data.Interface
{
    public interface IProductsService
    {
        IEnumerable<Product> AllProducts(int shopID);
    }
}
