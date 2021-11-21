
namespace HusbandGrpcService.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public virtual Admin Admins { get; set; }
        public virtual Husband Husbands { get; set; }
        public virtual Wife Wifes { get; set; }
    }
}
