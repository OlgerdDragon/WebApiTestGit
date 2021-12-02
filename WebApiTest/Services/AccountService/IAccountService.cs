using System.Threading.Tasks;
using WebApiGeneralGrpc.Data;

namespace WebApiGeneralGrpc.Services.AccountService
{
    public interface IAccountService
    {
        public Task<Result<object>> Token(string username, string password);
    }
}
