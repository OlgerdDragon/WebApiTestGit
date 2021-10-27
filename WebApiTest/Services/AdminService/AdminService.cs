using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using WebApiTest.Data;
using WebApiTest.Models;
using WebApiTest.Models.Dto;


namespace WebApiTest.Services.AdminService
{
    public class AdminService: IAdminService
    {
        private readonly TownContext _context;
        private readonly ILogger<AdminService> _logger;
        public AdminService(TownContext context, ILogger<AdminService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result<List<ProductDto>>> GetProductsAsync()
        {
            try
            {
                return new Result<List<ProductDto>>(await _context.Products.Select(i => new ProductDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    ShopId = i.ShopId
                }).ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception("GetProductsAsync method:" + ex.Message);
            }
        }

        public async Task<Result<List<ShopDto>>> GetShopsAsync()
        {
            try
            {
                return new Result<List<ShopDto>>(await _context.Shops.Select(i => new ShopDto
                {
                    Id = i.Id,
                    Name = i.Name
                }).ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception("GetShopsAsync method:" + ex.Message);
            }
        }
        public async Task<Result<ShopDto>> UpdateShopAsync(ShopDto newShop)
        {
            try
            {
                var shop = _context.Shops
                .Where(i => i.Id == newShop.Id)
                .FirstOrDefault();

                shop.Name = newShop.Name;
                await SaveChangesAsync();
                return new Result<ShopDto>(newShop);
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateShopAsync method:" + ex.Message);
            }
        }
        public async Task<Result<ProductDto>> UpdateProductAsync(ProductDto newProduct)
        {
            try
            {
                var product = _context.Products
                .Where(i => i.Id == newProduct.Id)
                .FirstOrDefault();

                product.Name = newProduct.Name;
                product.Price = newProduct.Price;
                product.ShopId = newProduct.ShopId;

                await SaveChangesAsync();
                return new Result<ProductDto>(newProduct);
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateProductAsync method:" + ex.Message);
            }
        }
        public async Task<Result<Shop>> FindShopAsync(int id)
        {
            try
            {
                return new Result<Shop>(await _context.Shops.FindAsync(id));
            }
            catch (Exception ex)
            {
                throw new Exception("FindShopAsync method:" + ex.Message);
            }
        }

        public async Task<Result<Product>> FindProductAsync(int id)
        {
            try
            {
                return new Result<Product>(await _context.Products.FindAsync(id));
            }
            catch (Exception ex)
            {
                throw new Exception("FindProductAsync method:" + ex.Message);
            }
        }
        public async Task<Result<ActionResult<ShopDto>>> GetShopAsync(int id)
        {
            try
            {
                var shopItem = await FindShopAsync(id);
                if (shopItem.Element == null)
                {
                    return null;
                }
                return new Result<ActionResult<ShopDto>>(ShopDto.ItemShopDTO(shopItem.Element));
            }
            catch (Exception ex)
            {
                throw new Exception("GetShopAsync method:" + ex.Message);
            }
            
        }
        public async Task<Result<ActionResult<ProductDto>>> GetProductAsync(int id)
        {
            try
            {
                var productItem = await FindProductAsync(id);

                if (productItem.Element == null)
                {
                    return null;
                }

                return new Result<ActionResult<ProductDto>>(ProductDto.ItemProductDTO(productItem.Element));
            }
            catch (Exception ex)
            {
                throw new Exception("GetProductAsync method:" + ex.Message);
            }
        }
        public async Task<Result<bool>> RemoveProduct(int id)
        {
            try
            {
                var status = true;
                var productItem = await FindProductAsync(id);
                if (productItem == null)
                    status = false;

                _context.Products.Remove(productItem.Element);
                await SaveChangesAsync();
                return new Result<bool>(status);
            }
            catch (Exception ex)
            {
                throw new Exception("RemoveProduct method:" + ex.Message);
            }
        }

        public async Task<Result<bool>> RemoveShop(int id)
        {
            try
            {
                var status = true;
                var shopItem = await FindShopAsync(id);
                if (shopItem == null)
                    status = false;

                _context.Shops.Remove(shopItem.Element);
                await SaveChangesAsync();
                return new Result<bool>(status);
            }
            catch (Exception ex)
            {
                throw new Exception("RemoveShop method:" + ex.Message);
            }
        }
        public async Task<Result<bool>> AddProduct(ProductDto productDtoItem)
        {
            try
            {
                var shop = FindShopAsync(productDtoItem.ShopId);
                var productItem = productDtoItem.Product(shop.Result.Element);

                _context.Products.Add(productItem);
                await SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                throw new Exception("AddProduct method:" + ex.Message);
            }
        }
        public async Task<Result<bool>> AddShop(ShopDto shopDtoItem)
        {
            try
            {
                var shop = _context.Shops.Add(shopDtoItem.Shop());
                await SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                throw new Exception("AddShop method:" + ex.Message);
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
    }
}
