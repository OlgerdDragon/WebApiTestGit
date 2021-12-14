using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WifeGrpcService.Data;
using WifeGrpcService.Models.Dto;
using WifeGrpcService.Models;
using Google.Protobuf.WellKnownTypes;

namespace WifeGrpcService.Services
{
    public class WifeGreeterService : WifeGreeter.WifeGreeterBase, IWifeGreeterService
    {

        private readonly TownContext _context;
        private readonly ILogger<WifeGreeterService> _logger;
        public WifeGreeterService(TownContext context, ILogger<WifeGreeterService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public override Task<BoolValue> Health(Empty _, ServerCallContext context)
        {
            return Task.FromResult(new BoolValue() { Value = true });
        }
        public override async Task<GetWantedProductsReply> GetWantedProducts(UserLoginRequest request, ServerCallContext context)
        {
            try
            {
                var wantedProducts = await _context.WantedProducts.Select(i => new WantedProductDtoMessage
                {
                    Id = i.Id,
                    BoughtStatus = i.BoughtStatus,
                    ProductId = i.ProductId,
                    WifeId = i.WifeId
                }).ToListAsync();
                var result = new GetWantedProductsReply { Element = new ListOfWantedProductDto() };
                result.Element.WantedProductDtoMessage.AddRange(wantedProducts);
                result.Successfully = true;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetWantedProducts - UserLogin: {request.UserLogin}");
                return new GetWantedProductsReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<GetTotalAmountWantedProductsReply> GetTotalAmountWantedProducts(UserLoginRequest request, ServerCallContext context)
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
                return new GetTotalAmountWantedProductsReply { Element = totalAmount, Successfully = true};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetTotalAmountWantedProducts - UserLogin: {request.UserLogin}");
                return new GetTotalAmountWantedProductsReply { ErrorMessage = ex.Message, Successfully = false };
            }

        }
        public override async Task<WantedProductReply> AddProduct(ItemRequest request, ServerCallContext context)
        {
            try
            {
                var id = request.Id;
                var productItem = await FindProductAsync(id);
                if (!productItem.Successfully)
                {
                    _logger.LogError(productItem.ExceptionMessage, $"AddProduct id: {id} UserLogin: {request.UserLogin}");
                    return new WantedProductReply { ErrorMessage = productItem.ExceptionMessage.Message, Successfully = false};
                }
                if (productItem.Element == null)
                {
                    _logger.LogDebug($"AddProduct userLogin: {request.UserLogin} id: {id} return  null object. Product not found");
                    return new WantedProductReply { Element = new WantedProductDtoMessage(), Successfully = true };
                }
                var wantedProductItem = WantedProductDto.ConvertProductInWantedProduct(productItem.Element);
                _context.WantedProducts.Add(wantedProductItem);
                await SaveChangesAsync();

                var wantedProductDtoMessage = WantedProductDto.ItemWantedProductDTOMessage(wantedProductItem);

                _logger.LogInformation($"AddProduct(id) userLogin: {request.UserLogin} id: {id} return - wantedProductDTO.Id: {wantedProductDtoMessage.Id} wantedProductDTO.ProductId: {wantedProductDtoMessage.ProductId}");
                return new WantedProductReply { Element = wantedProductDtoMessage, Successfully = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"AddProduct id: {request.Id} UserLogin: {request.UserLogin}");
                return new WantedProductReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }

        public override async Task<WantedProductReply> GetWantedProductItem(ItemRequest request, ServerCallContext context)
        {
            try
            {
                var id = request.Id;
                var wantedProductItem = await FindWantedProductAsync(id);
                if (!wantedProductItem.Successfully)
                {
                    _logger.LogError(wantedProductItem.ExceptionMessage, $"AddProduct id: {id} UserLogin: {request.UserLogin}");
                    return new WantedProductReply { ErrorMessage = wantedProductItem.ExceptionMessage.Message, Successfully = false };
                }
                if (wantedProductItem.Element == null)
                {
                    _logger.LogDebug($"AddProduct userLogin: {request.UserLogin} id: {id} return  null object. Product not found");
                    return new WantedProductReply { Element = new WantedProductDtoMessage(), Successfully = true };
                }
                var wantedProductDtoMessage = WantedProductDto.ItemWantedProductDTOMessage(wantedProductItem.Element);
                _logger.LogDebug($"GetWantedProductItemAsync(id) id: {id} return - wantedProductDTO.Id: {wantedProductDtoMessage.Id} wantedProductDTO.ProductId: {wantedProductDtoMessage.ProductId}");
                return new WantedProductReply { Element = wantedProductDtoMessage, Successfully = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetWantedProductItemAsync id: {request.Id}");
                return new WantedProductReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<BoolReply> RemoveWantedProduct(ItemRequest request, ServerCallContext context)
        {
            try
            {
                var id = request.Id;
                var status = true;
                var wantedProductItem = await FindWantedProductAsync(id);

                if (!wantedProductItem.Successfully)
                {
                    _logger.LogError(wantedProductItem.ExceptionMessage, $"RemoveWantedProduct id: {request.Id} UserLogin: {request.UserLogin}");
                    return new BoolReply { ErrorMessage = wantedProductItem.ExceptionMessage.Message, Successfully = false };
                }
                if (wantedProductItem.Element == null)
                {
                    _logger.LogDebug($"RemoveWantedProduct userLogin: {request.UserLogin} id: {id} - WantedProduct not found");
                    status = false;
                }
                else
                {
                    _context.WantedProducts.Remove(wantedProductItem.Element);
                    await SaveChangesAsync();
                }

                _logger.LogInformation($"RemoveWantedProduct(id) userLogin: {request.UserLogin} id: {id}  return - status: {status}");
                return new BoolReply { Element = status, Successfully = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RemoveWantedProduct id: {request.Id} UserLogin: {request.UserLogin}");
                return new BoolReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<BoolReply> RemoveAllWantedProducts(UserLoginRequest request, ServerCallContext context)
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
                _logger.LogInformation($"RemoveAllWantedProducts userLogin: {request.UserLogin} return - status: {status}");
                return new BoolReply { Element = status, Successfully = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RemoveAllWantedProducts - UserLogin: {request.UserLogin}");
                return new BoolReply { ErrorMessage = ex.Message, Successfully = false };
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
                if (product == null)
                    _logger.LogDebug($"FindProductAsync return - id: {id}  Null oject");
                else
                    _logger.LogDebug($"FindProductAsync return - id: {id} product.Id: {product.Id} product.Name: {product.Name} product.Price: {product.Price} product.ShopId: {product.ShopId}");
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
                if (wantedProduct == null)
                    _logger.LogDebug($"FindWantedProductAsync return - id: {id}  Null oject");
                else
                    _logger.LogDebug($"FindWantedProductAsync id: {id} return -  wantedProduct.Id: {wantedProduct.Id} wantedProduct.Name: {wantedProduct.ProductId}");
                return new Result<WantedProduct>(wantedProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"FindWantedProductAsync id: {id}");
                return new Result<WantedProduct>(ex);
            }
        }
    }
}
