using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
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
        private readonly ILogger<WifeService> _logger;
        public WifeService(TownContext context, ILogger<WifeService> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<Result<List<WantedProductDto>>> GetWantedProductsAsync()
        {
            try
            {
                var wantedProducts = await _context.WantedProducts.Select(i => new WantedProductDto
                {
                    Id = i.Id,
                    BoughtStatus = i.BoughtStatus,
                    ProductId = i.ProductId,
                    WifeId = i.WifeId

                }).ToListAsync();
                return new Result<List<WantedProductDto>>(wantedProducts);
            }
            catch (Exception ex)
            {
                throw new Exception("GetWantedProductsAsync method:" + ex.Message);
            }
        }
        public async Task<Result<string>> GetTotalAmountWantedProductsAsync()
        {
            try
            {
                var _wantedProductsList = await _context.WantedProducts.Select(i => new WantedProduct
                {
                    Id = i.Id,
                    BoughtStatus = i.BoughtStatus,
                    ProductId = i.ProductId,
                    WifeId = i.WifeId

                }).ToListAsync();
                int _totalAmount = 0;
                foreach (var item in _wantedProductsList)
                {
                    var product = await FindProductAsync(item.ProductId);
                    _totalAmount += product.Element.Price;
                }
                return new Result<string>($"Total Amount: {_totalAmount}");
            }
            catch (Exception ex)
            {
                throw new Exception("GetTotalAmountWantedProductsAsync method:" + ex.Message);
            }
            
        }
        public async Task<Result<WantedProductDto>> AddProduct(int id)
        {
            try
            {
                var _productItem = await FindProductAsync(id);
                var _wantedProductItem = WantedProductDto.ConvertProductInWantedProduct(_productItem.Element);
                _context.WantedProducts.Add(_wantedProductItem);
                await SaveChangesAsync();

                var wantedProductDTO = WantedProductDto.ItemWantedProductDTO(_wantedProductItem);
                return new Result<WantedProductDto>(wantedProductDTO);
            }
            catch (Exception ex)
            {
                throw new Exception("AddProduct method:" + ex.Message);
            }
        }
        async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("SaveChangesAsync method:" + ex.Message);
            }
        }
        public async Task<Result<Product>> FindProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                return new Result<Product>(product);

            }
            catch (Exception ex)
            {
                throw new Exception("FindProductAsync method:" + ex.Message);
            }

        }
        public async Task<Result<WantedProduct>> FindWantedProductAsync(int id)
        {
            try
            {
                var wantedProduct = await _context.WantedProducts.FindAsync(id);
                return new Result<WantedProduct>(wantedProduct);
            }
            catch (Exception ex)
            {
                throw new Exception("FindWantedProductAsync method:" + ex.Message);
            }
        }
        public async Task<Result<ActionResult<WantedProductDto>>> GetWantedProductItemAsync(int id)
        {
            try
            {
                var wantedProductItem = await FindWantedProductAsync(id);

                if (wantedProductItem == null)
                {
                    return null;
                }

                var wantedProductDTO = WantedProductDto.ItemWantedProductDTO(wantedProductItem.Element);
                return new Result<ActionResult<WantedProductDto>>(wantedProductDTO);
            }
            catch (Exception ex)
            {
                throw new Exception("GetWantedProductItemAsync method:" + ex.Message);
            }
        }
        public async Task<Result<bool>> RemoveWantedProduct(int id)
        {
            try
            {
                var status = true;
                var _wantedProductItem = await FindWantedProductAsync(id);

                if (_wantedProductItem == null)
                    status = false;

                _context.WantedProducts.Remove(_wantedProductItem.Element);
                await SaveChangesAsync();

                return new Result<bool>(status);
            }
            catch (Exception ex)
            {
                throw new Exception("RemoveWantedProduct method:" + ex.Message);
            }
        }
        public async Task<Result<bool>> RemoveAllWantedProducts()
        {
            try
            {
                var status = true;
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

                return new Result<bool>(status);
            }
            catch (Exception ex)
            {
                throw new Exception("RemoveAllWantedProducts method:" + ex.Message);
            }
        }
    }
}
