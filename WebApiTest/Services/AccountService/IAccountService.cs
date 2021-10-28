using System.Threading.Tasks;
using WebApiTest.Data;

namespace WebApiTest.Services.AccountService
{
    public interface IAccountService
    {
        public Task<Result<object>> Token(string username, string password);
    }
}
