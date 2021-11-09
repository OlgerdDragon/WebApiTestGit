using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                _logger.LogError(ex, "GetWantedProductsAsync");
                return new Result<List<WantedProductDto>>(ex);
            }
        }
        public async Task<Result<int>> GetTotalAmountWantedProductsAsync()
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
                int totalAmount = 0;
                foreach (var item in _wantedProductsList)
                {
                    var product = await FindProductAsync(item.ProductId);
                    totalAmount += product.Element.Price;
                    _logger.LogDebug($"GetTotalAmountWantedProductsAsync - _totalAmount: {totalAmount} item.ProductId:{item.ProductId} product.Price{product.Element.Price}");
                }

                _logger.LogDebug($"GetTotalAmountWantedProductsAsync - _totalAmount: {totalAmount}");
                return new Result<int>(totalAmount);
            }
            catch (Exception ex)
            {
                return new Result<int>(ex);
            }

        }
        public async Task<Result<WantedProductDto>> AddProduct(int id, string userLogin)
        {
            try
            {
                var _productItem = await FindProductAsync(id);
                var _wantedProductItem = WantedProductDto.ConvertProductInWantedProduct(_productItem.Element);
                _context.WantedProducts.Add(_wantedProductItem);
                await SaveChangesAsync();

                var wantedProductDTO = WantedProductDto.ItemWantedProductDTO(_wantedProductItem);

                _logger.LogInformation($"AddProduct(id) userLogin: {userLogin} id: {id} return - wantedProductDTO.Id: {wantedProductDTO.Id} wantedProductDTO.ProductId: {wantedProductDTO.ProductId}");
                return new Result<WantedProductDto>(wantedProductDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"AddProduct id: {id}");
                return new Result<WantedProductDto>(ex);
            }
        }
        async Task<Result<int>> SaveChangesAsync()
        {
            try
            {
                var res = await _context.SaveChangesAsync();
                _logger.LogDebug($"SaveChangesAsync done!");
                return new Result<int>(res);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetWantedProductsAsync");
                return new Result<int>(ex);
            }
        }
        async Task<Result<Product>> FindProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                _logger.LogDebug($"FindProductAsync id: {id} return - product.Id: {product.Id} product.Name: {product.Name} product.Price: {product.Price} product.ShopId: {product.ShopId}");
                return new Result<Product>(product);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"FindProductAsync id: {id}");
                return new Result<Product>(ex);
            }

        }
        async Task<Result<WantedProduct>> FindWantedProductAsync(int id)
        {
            try
            {
                var wantedProduct = await _context.WantedProducts.FindAsync(id);

                _logger.LogDebug($"FindWantedProductAsync id: {id} return -  wantedProduct.Id: {wantedProduct.Id} wantedProduct.Name: {wantedProduct.ProductId}");
                return new Result<WantedProduct>(wantedProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"FindWantedProductAsync id: {id}");
                return new Result<WantedProduct>(ex);
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

                _logger.LogDebug($"GetWantedProductItemAsync(id) id: {id} return - wantedProductDTO.Id: {wantedProductDTO.Id} wantedProductDTO.ProductId: {wantedProductDTO.ProductId}");
                return new Result<ActionResult<WantedProductDto>>(wantedProductDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetWantedProductItemAsync id: {id}");
                return new Result<ActionResult<WantedProductDto>>(ex);
            }
        }
        public async Task<Result<bool>> RemoveWantedProduct(int id, string userLogin)
        {
            try
            {
                var status = true;
                var _wantedProductItem = await FindWantedProductAsync(id);

                if(!_wantedProductItem.Successfully) return new Result<bool>(_wantedProductItem.ExceptionMessage);

                if (_wantedProductItem.Element == null)
                    status = false;
                else
                {
                    _context.WantedProducts.Remove(_wantedProductItem.Element);
                    await SaveChangesAsync();
                }

                _logger.LogInformation($"RemoveWantedProduct(id) userLogin: {userLogin} id: {id}  return - status: {status}");
                return new Result<bool>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RemoveWantedProduct id: {id}");
                return new Result<bool>(ex);
            }
        }
        public async Task<Result<bool>> RemoveAllWantedProducts(string userLogin)
        {
            try
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
                var status = true;
                _logger.LogInformation($"RemoveAllWantedProducts userLogin: {userLogin} return - status: {status}");
                return new Result<bool>(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RemoveAllWantedProducts");
                return new Result<bool>(ex);
            }
        }
    }
}
