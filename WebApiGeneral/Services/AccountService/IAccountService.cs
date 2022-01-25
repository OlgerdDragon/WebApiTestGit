using System.Threading.Tasks;
using WebApiGeneral.Data;

namespace WebApiGeneral.Services.AccountService
{
    public interface IAccountService
    {
        public Task<Result<object>> Token(string username, string password);
    }
}
