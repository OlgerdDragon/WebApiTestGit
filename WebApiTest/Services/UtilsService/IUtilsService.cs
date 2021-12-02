using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGeneralGrpc.Data;

namespace WebApiGeneralGrpc.Services.UtilsService
{
    public interface IUtilsService
    {
        public Task<Result<bool>> ChangeLogLevel(int level);
    }
}
