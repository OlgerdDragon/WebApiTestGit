using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WifeService.Data;
using WifeService.Models.Dto;
using DbApiContextForService;
using DbApiContextForService.Models;
using Google.Protobuf.WellKnownTypes;
using HusbandService;
using WifeService.Services.HusbandServiceFactory;
using System.Collections.Generic;

namespace WifeService.Services
{
    public class WifeGreeterService : WifeGreeter.WifeGreeterBase, IWifeGreeterService
    {
        private readonly DbApiContext _context;
        private readonly ILogger<WifeGreeterService> _logger;
        public HusbandGreeter.HusbandGreeterClient _husbandServiceClient;
        public WifeGreeterService(DbApiContext context, ILogger<WifeGreeterService> logger, IHusbandServiceFactory husbandServiceFactory)
        {
            _context = context;
            _logger = logger;
            _husbandServiceClient = husbandServiceFactory.GetGrpcClient();
        }
        public override Task<BoolValue> Health(Empty _, ServerCallContext context)
        {
            return Task.FromResult(new BoolValue() { Value = true });
        }
        public override async Task<GetWantedProductsReply> GetWantedProducts(UserLoginRequest request, ServerCallContext context)
        {
            try
            {
                var getWantedProductsReplyHusband = await _husbandServiceClient.GetWantedProductsAsync(new global::HusbandService.UserLoginRequest() { UserLogin = request.UserLogin });
                var listHusband = getWantedProductsReplyHusband.Element.WantedProductDtoMessage.ToList();
                var resultList = new List<WantedProductDtoMessage>();
                foreach (var itemHusband in listHusband)
                {
                    var itemWife = new WantedProductDtoMessage
                    {
                        Id = itemHusband.Id,
                        BoughtStatus = itemHusband.BoughtStatus,
                        ProductId = itemHusband.ProductId,
                        WifeId = itemHusband.WifeId
                    };
                    resultList.Add(itemWife);
                }

                var result = new GetWantedProductsReply { Element = new ListOfWantedProductDto() };
                result.Element.WantedProductDtoMessage.AddRange(resultList);
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
                var getTotalAmountWantedProductsReplyHusband = await _husbandServiceClient.GetTotalAmountWantedProductsAsync(new global::HusbandService.UserLoginRequest() { UserLogin = request.UserLogin });
                _logger.LogDebug($"GetTotalAmountWantedProductsAsync - _totalAmount: {getTotalAmountWantedProductsReplyHusband.Element}");
                return new GetTotalAmountWantedProductsReply { Element = getTotalAmountWantedProductsReplyHusband.Element, Successfully = getTotalAmountWantedProductsReplyHusband .Successfully};
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
                var wantedProductReplyHusband = await _husbandServiceClient.AddWantedProductAsync(new global::HusbandService.ItemRequest() { Id = request.Id, UserLogin = request.UserLogin });
                var wantedProductDtoMessage = new WantedProductDtoMessage
                {
                    Id = wantedProductReplyHusband.Element.Id,
                    BoughtStatus = wantedProductReplyHusband.Element.BoughtStatus,
                    ProductId = wantedProductReplyHusband.Element.ProductId,
                };
                _logger.LogInformation($"AddProduct(id) userLogin: {request.UserLogin} id: {request.Id} return - wantedProductDTO.Id: {wantedProductDtoMessage.Id} wantedProductDTO.ProductId: {wantedProductDtoMessage.ProductId}");
                return new WantedProductReply { Element = wantedProductDtoMessage, Successfully = wantedProductReplyHusband.Successfully };
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
                var wantedProductReplyHusband = await _husbandServiceClient.GetWantedProductItemAsync(new global::HusbandService.ItemRequest() { Id = request.Id, UserLogin = request.UserLogin });
                _logger.LogDebug($"GetWantedProductItemAsync(id) userLogin: {request.UserLogin} id: {request.Id} return - wantedProductDTO.Id: {wantedProductReplyHusband.Element.Id} wantedProductDTO.ProductId: {wantedProductReplyHusband.Element.ProductId}");
                var wantedProductDtoMessage = new WantedProductDtoMessage
                {
                    Id = wantedProductReplyHusband.Element.Id,
                    BoughtStatus = wantedProductReplyHusband.Element.BoughtStatus,
                    ProductId = wantedProductReplyHusband.Element.ProductId,
                    WifeId = wantedProductReplyHusband.Element.WifeId
                };
                return new WantedProductReply { Element = wantedProductDtoMessage, Successfully = wantedProductReplyHusband.Successfully };
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
                var boolReplyHusband = await _husbandServiceClient.RemoveWantedProductAsync(new global::HusbandService.ItemRequest() { Id = request.Id, UserLogin = request.UserLogin });
                var status = boolReplyHusband.Element;
                _logger.LogInformation($"RemoveWantedProduct(id) userLogin: {request.UserLogin} id: {request.Id}  return - status: {status}");
                return new BoolReply { Element = status, Successfully = boolReplyHusband.Successfully };
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
                var boolReplyHusband = await _husbandServiceClient.RemoveAllWantedProductsAsync(new global::HusbandService.UserLoginRequest() { UserLogin = request.UserLogin });
                var status = boolReplyHusband.Element;
                _logger.LogInformation($"RemoveAllWantedProducts userLogin: {request.UserLogin} return - status: {status}");
                return new BoolReply { Element = status, Successfully = boolReplyHusband.Successfully };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RemoveAllWantedProducts - UserLogin: {request.UserLogin}");
                return new BoolReply { ErrorMessage = ex.Message, Successfully = false };
            }
        }

    }
}
