using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiMonolit.Data;

namespace WebApiMonolit.Services.UtilsService
{
    public interface IUtilsService
    {
        public Task<Result<bool>> ChangeLogLevel(int level);
    }
}
