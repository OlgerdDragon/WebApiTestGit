using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HusbandService.Data;
using HusbandService.Models.Dto;
using Google.Protobuf.WellKnownTypes;
using TownContextForWebService;
using TownContextForWebService.Models;
using AdminService;
using HusbandService.Services.AdminServiceFactory;

namespace HusbandService.Services
{
    public class HusbandGreeterService : HusbandGreeter.HusbandGreeterBase, IHusbandGreeterService
    {

        private readonly TownContext _context; 
        private readonly ILogger<HusbandGreeterService> _logger;
        private readonly AdminGreeter.AdminGreeterClient _adminServiceClient;

        public HusbandGreeterService(TownContext context, ILogger<HusbandGreeterService> logger, IAdminServiceFactory adminServiceFactory)
        {
            _context = context;
            _logger = logger;
            _adminServiceClient = adminServiceFactory.GetGrpcClient();
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
                _logger.LogError(ex, $"GetWantedProductsAsync userLogin: {request.UserLogin}");
                return new GetWantedProductsReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<GetShopsForVisitReply> GetShopsForVisit(UserLoginRequest request, ServerCallContext context)
        {
            try
            {
                var wantedProductList = GetWantedProducts().Element;
                var wantedProdutListRequest = new global::AdminService.GetShopsForVisitRequest { WantedProductList = new global::AdminService.ListOfWantedProductDto() };
                wantedProdutListRequest.WantedProductList.WantedProductDtoMessage.AddRange(wantedProductList);
                var getProductsInShopReplyHusband = await _adminServiceClient.GetShopsForVisitAsync(new AdminService.GetShopsForVisitRequest()
                {
                    UserLogin = request.UserLogin,
                    WantedProductList = wantedProdutListRequest.WantedProductList
                });
                var replyList = getProductsInShopReplyHusband.Element.ShopDtoMessage.ToList();
                var resulList = new List<ShopDtoMessage>();
                foreach (var replyItem in replyList)
                {
                    resulList.Add(new ShopDtoMessage
                    {
                        Id = replyItem.Id,
                    });
                }
                var result = new GetShopsForVisitReply
                {
                    Successfully = getProductsInShopReplyHusband.Successfully,
                    Element = new ListOfShopDto()
                };
                result.Element.ShopDtoMessage.AddRange(resulList);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetShopsForVisitAsync");
                return new GetShopsForVisitReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        public override async Task<GetProductsInShopReply> GetProductsInShop(GetProductsInShopRequest request, ServerCallContext context)
        {
            try
            {
                _logger.LogDebug($"GetProductsInShopAsync  userLogin: {request.UserLogin} - ShopId: {request.ShopId} ");
                var wantedProductList = GetWantedProducts().Element;
                var wantedProdutListRequest = new global::AdminService.GetProductsInShopRequest { WantedProductList = new global::AdminService.ListOfWantedProductDto() };
                wantedProdutListRequest.WantedProductList.WantedProductDtoMessage.AddRange(wantedProductList);
                var getProductsInShopReplyHusband = await _adminServiceClient.GetProductsInShopAsync(new AdminService.GetProductsInShopRequest()
                {
                    ShopId = request.ShopId,
                    UserLogin = request.UserLogin,
                    WantedProductList = wantedProdutListRequest.WantedProductList
                });
                var replyList = getProductsInShopReplyHusband.Element.ProductDtoMessage.ToList();
                var resulList = new List<ProductDtoMessage>();
                foreach (var replyItem in replyList)
                {
                    resulList.Add(new ProductDtoMessage
                    {
                        Id = replyItem.Id,
                        Name = replyItem.Name,
                        Price = replyItem.Price,
                        ShopId = replyItem.ShopId
                    });
                }
                var result = new GetProductsInShopReply
                {
                    Successfully = getProductsInShopReplyHusband.Successfully,
                    Element = new ListOfProductDto()
                };
                result.Element.ProductDtoMessage.AddRange(resulList);    
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetProductsInShopAsync userLogin: {request.UserLogin}  - ShopId: { request.ShopId}");
                return new GetProductsInShopReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }
        
        Result<List<global::AdminService.WantedProductDtoMessage>> GetWantedProducts()
        {
            try
            {
                var wantedProductList = _context.WantedProducts.Select(i => new AdminService.WantedProductDtoMessage
                {
                    Id = i.Id,
                    BoughtStatus = i.BoughtStatus,
                    ProductId = i.ProductId

                }).ToList();
                _logger.LogDebug($"GetWantedProducts return - wantedProductList: {wantedProductList}");
                return new Result<List<AdminService.WantedProductDtoMessage>>(wantedProductList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetWantedProducts");
                return new Result<List<AdminService.WantedProductDtoMessage>>(ex);
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
                    var product = await _adminServiceClient.GetProductAsync(new AdminService.ItemRequest {Id = item.ProductId, UserLogin = request.UserLogin } );
                    totalAmount += product.Element.Price;
                    _logger.LogDebug($"GetTotalAmountWantedProductsAsync - _totalAmount: {totalAmount} item.ProductId:{item.ProductId} product.Price{product.Element.Price}");
                }
                _logger.LogDebug($"GetTotalAmountWantedProductsAsync - _totalAmount: {totalAmount}");
                return new GetTotalAmountWantedProductsReply { Element = totalAmount, Successfully = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetTotalAmountWantedProducts - UserLogin: {request.UserLogin}");
                return new GetTotalAmountWantedProductsReply { ErrorMessage = ex.Message, Successfully = false };
            }

        }
        public override async Task<WantedProductReply> AddWantedProduct(ItemRequest request, ServerCallContext context)
        {
            try
            {
                var id = request.Id;
                var productItem = await _adminServiceClient.GetProductAsync(new AdminService.ItemRequest { Id = id, UserLogin = request.UserLogin });
                if (!productItem.Successfully)
                {
                    _logger.LogError(productItem.ErrorMessage, $"AddProduct id: {id} UserLogin: {request.UserLogin}");
                    return new WantedProductReply { ErrorMessage = productItem.ErrorMessage, Successfully = false };
                }
                if (productItem.Element == null)
                {
                    _logger.LogDebug($"AddProduct userLogin: {request.UserLogin} id: {id} return  null object. Product not found");
                    return new WantedProductReply { Element = new WantedProductDtoMessage(), Successfully = true };
                }
                var wantedProductItem = WantedProductDto.ConvertProductInWantedProductDtoMessage(productItem.Element);
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

    }
}
