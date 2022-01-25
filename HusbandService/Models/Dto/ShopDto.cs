using TownContextForWebService.Models;

namespace HusbandService.Models.Dto
{
    public class ShopDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Shop Shop()
        {
            return new Shop
            {
                Name = this.Name
            };
        }
        public static ShopDto ItemShopDTO(Shop shopItem) =>
           new ShopDto
           {
               Id = shopItem.Id,
               Name = shopItem.Name
           };
        public static ShopDtoMessage ItemShopDTOMessage(Shop shopItem) =>
           new ShopDtoMessage
           {
               Id = shopItem.Id,
               Name = shopItem.Name
           };
    }
}
