using NUnit.Framework;
using WebApiTest.Services.AccountService;
using WebApiTest.Services.AdminService;
using WebApiTest.Services.HusbandService;
using WebApiTest.Services.UtilsService;
using WebApiTest.Services.WifeService;

namespace WebAPITest.NUnitTests
{
    public class Tests
    {
        private IAccountService _accountService(TownContext context, ILogger<AccountService> logger);
        private readonly IAdminService _adminService;
        private readonly IHusbandService _husbandService;
        private readonly IUtilsService _utilsService;
        private readonly IWifeService _wifeService;
        
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            
            var result = Token("", "").Result;
            var actual = result.Element;

            if (actual == null) Assert.Pass();
        }
    }
}