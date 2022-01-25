
namespace DbApiContextForService.Models
{
    public class Husband
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WifeId { get; set; }
        public virtual Wife Wifes { get; set; }
        public int PersonId { get; set; }
        public virtual Person Persons { get; set; }
    }
}
