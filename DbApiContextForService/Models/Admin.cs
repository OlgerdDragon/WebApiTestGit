namespace DbApiContextForService.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PersonId { get; set; }
        public virtual Person Persons { get; set; }
    }
}
