﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.WifeService
{
    public interface IWifeService
    {
        public Task<List<WantedProduct>> GetWantedProductsAsync(); 
        public Task AddProduct(WantedProductDto wantedProductItem);
        public Task<WantedProduct> FindWantedProductAsync(int id);
        public Task<ActionResult<WantedProductDto>> GetWantedProductItemAsync(int id);
        public Task<bool> RemoveWantedProduct(int id);
        public Task RemoveAllWantedProducts();
    }
}
