using System.Threading.Tasks;
using WebApiMonolit.Data;

namespace WebApiMonolit.Services.AccountService
{
    public interface IAccountService
    {
        public Task<Result<object>> Token(string username, string password);
    }
}
