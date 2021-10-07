
namespace WebApiTest.Models
{
    public class Husband
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WifeId { get; set; }
        public Wife Wife { get; set; }
    }
}
