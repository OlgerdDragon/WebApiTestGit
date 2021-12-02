
namespace WebApiGeneralGrpc.Models
{
    public class WantedProduct
    {
        public int Id { get; set; }
        public bool BoughtStatus { get; set; }
        public int ProductId { get; set; }
        public virtual Product Products { get; set; }
        public int WifeId { get; set; }
        public virtual Wife Wifes { get; set; }
        
    }
}
