using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Data;
using WebApiTest.Models;
using WebApiTest.Models.Dto;

namespace WebApiTest.Services.WifeService
{
    public class WifeService : IWifeService
    {
        private readonly TownContext _context;

        public WifeService(TownContext context)
        {
            _context = context;
        }
        
        public async Task<List<WantedProduct>> GetWantedProductsAsync()
        {
            return await _context.WantedProducts.Select(i => new WantedProduct
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
            }).ToListAsync();
        }
        public async Task<string> GetTotalAmountWantedProductsAsync()
        {
            var _wantedProductsList = await _context.WantedProducts.Select(i => new WantedProduct
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
            }).ToListAsync();
            int _totalAmount = 0;
            foreach (var item in _wantedProductsList)
            {
                var product = await FindProductAsync(item.ProductId);
                _totalAmount += product.Price;
            }
            return $"Total Amount: {_totalAmount}";
        }
        public async Task<WantedProductDto> AddProduct(int id)
        {
            var _productItem = await FindProductAsync(id);
            var _wantedProductItem = WantedProductDto.ConvertProductInWantedProduct(_productItem);
            _context.WantedProducts.Add(_wantedProductItem);
            await SaveChangesAsync();
            return WantedProductDto.ItemWantedProductDTO(_wantedProductItem);
        }
        async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<Product> FindProductAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<WantedProduct> FindWantedProductAsync(int id)
        {
            return await _context.WantedProducts.FindAsync(id);
        }
        public async Task<ActionResult<WantedProductDto>> GetWantedProductItemAsync(int id)
        {
            var wantedProductItem = await FindWantedProductAsync(id);

            if (wantedProductItem == null)
            {
                return null;
            }

            return WantedProductDto.ItemWantedProductDTO(wantedProductItem);
        }
        public async Task<bool> RemoveWantedProduct(int id)
        {
            var _wantedProductItem = await FindWantedProductAsync(id);

            if (_wantedProductItem == null)
            {
                return false;
            }

            _context.WantedProducts.Remove(_wantedProductItem);
            await SaveChangesAsync();
            return true;
        }
        public async Task RemoveAllWantedProducts()
        {
            var _wantedProductList = _context.WantedProducts.Select(i => new WantedProduct
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
            }).ToList();
            foreach (var item in _wantedProductList)
            {
                _context.WantedProducts.Remove(item);
            }
            await SaveChangesAsync();
        }
    }
}
