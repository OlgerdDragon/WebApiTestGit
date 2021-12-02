
namespace WebApiMonolit.Models.Dto
{
    public class WantedProductDto
    {
        public int Id { get; set; }
        public bool BoughtStatus { get; set; }
        public int ProductId { get; set; }
        public int WifeId { get; set; }
        public WantedProduct WantedProduct() => new WantedProduct
        {
            Id = this.Id,
            BoughtStatus = this.BoughtStatus,
            ProductId = this.ProductId
            
        };
        public static WantedProductDto ItemWantedProductDTO(WantedProduct wantedProductItem) => new WantedProductDto
        {
            Id = wantedProductItem.Id,
            BoughtStatus = wantedProductItem.BoughtStatus,
            ProductId = wantedProductItem.ProductId,
            WifeId = wantedProductItem.WifeId
        };
        public static WantedProduct ConvertProductInWantedProduct(Product productItem) => new WantedProduct
        {
            BoughtStatus = false,
            ProductId = productItem.Id,
            WifeId = 1
            
        };
    }
    
}
