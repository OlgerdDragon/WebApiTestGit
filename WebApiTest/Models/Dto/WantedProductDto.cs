
namespace WebApiTest.Models.Dto
{
    public class WantedProductDto
    {
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public bool BoughtStatus { get; set; }
        public static WantedProductDto ItemWantedProductDTO(WantedProduct wantedProductItem) => new WantedProductDto
        {
            Id = wantedProductItem.Id,
            NameProduct = wantedProductItem.NameProduct,
            BoughtStatus = wantedProductItem.BoughtStatus
        };
    }
    
}
