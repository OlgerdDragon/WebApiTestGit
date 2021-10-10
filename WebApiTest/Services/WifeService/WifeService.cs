﻿using Microsoft.EntityFrameworkCore;
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

        public WifeService(TowerContext context)
        {
            _context = context;
        }
        public async Task<List<WantedProduct>> GetWantedProductsAsync()
        {
            return await _context.WantedProducts.Select(i => new WantedProduct
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
                NameProduct = i.NameProduct
            }).ToListAsync();
        }
        public async Task AddProduct(WantedProductDto wantedProductItem)
        {
            var _wantedProductItem = new WantedProduct
            {
                Id = wantedProductItem.Id,
                NameProduct = wantedProductItem.NameProduct,
                BoughtStatus = wantedProductItem.BoughtStatus
            };
            _context.WantedProducts.Add(_wantedProductItem);
            await SaveChangesAsync();
        }
        async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<WantedProduct> FindWantedProductAsync(int id)
        {
            return await _context.WantedProducts.FindAsync(id);
        }
        public async Task RemoveWantedProduct(WantedProduct wantedProductItem)
        {
            _context.WantedProducts.Remove(wantedProductItem);
            await SaveChangesAsync();
        }
        public async Task RemoveAllWantedProducts()
        {
            var wantedProductList = _context.WantedProducts.Select(i => new WantedProduct
            {
                Id = i.Id,
                BoughtStatus = i.BoughtStatus,
                NameProduct = i.NameProduct
            }).ToList();
            foreach (var item in wantedProductList)
            {
                _context.WantedProducts.Remove(item);
            }
            await SaveChangesAsync();
        }
    }
}
