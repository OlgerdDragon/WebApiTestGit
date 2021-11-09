using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Data;

namespace WebApiTest.Services.UtilsService
{
    public class UtilsService : IUtilsService
    {
        public async Task<Result<bool>> ChangeLogLevel(int level)
        {
            try
            {
                if(level<0 ||level>5) return new Result<bool>(false);
                var levelSwitch = new LoggingLevelSwitch();
                levelSwitch.MinimumLevel = (LogEventLevel)level;

                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .CreateLogger();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                return new Result<bool>(ex);
            }
            
        }
    }
}
