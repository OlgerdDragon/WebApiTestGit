using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HusbandGrpcService.Data;
using HusbandGrpcService.Models.Dto;
using HusbandGrpcService.Services.HusbandService;

namespace HusbandGrpcService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly TownContext _context;
        public GreeterService(ILogger<GreeterService> logger, TownContext context)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<HelloReply> GetWantedProductsAsyncStream(HelloRequest request, ServerCallContext context)
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
                var res =  new HelloReply { Element = new ListOfWantedProductDto()};
                res.Element.WantedProductDtoMessage.AddRange(wantedProducts);
                res.Successfully = true;
                return res;
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetWantedProductsAsync userLogin: {request.UserLogin}");
                return new HelloReply {ErrorMessage = ex.Message, Successfully = false};
            }
        }
    }
}
