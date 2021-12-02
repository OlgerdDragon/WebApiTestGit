using System.Threading.Tasks;
using Xunit;
using WebApiMonolit.Services.UtilsService;

namespace WebApiMonolit.XUnitTests.Common
{
    public class UtilsServiceTests
    {
        private UtilsService _utilsService;

        public UtilsServiceTests()
        {
        }
        
        [Fact]
        public async Task ChangeLogLevel_ShouldReturnFalse_WhenNegativeOne()
        {
            //Arrange


            //Act
            _utilsService = new UtilsService();
            var realData = await _utilsService.ChangeLogLevel(-1);

            //Assert

            Assert.False(realData.Element);
        }
        [Fact]
        public async Task ChangeLogLevel_ShouldReturnFalse_WhenSix()
        {
            //Arrange


            //Act
            _utilsService = new UtilsService();
            var realData = await _utilsService.ChangeLogLevel(6);

            //Assert

            Assert.False(realData.Element);
        }
        [Fact]
        public async Task ChangeLogLevel_ShouldReturnTrue_WhenOne()
        {
            //Arrange


            //Act
            _utilsService = new UtilsService();
            var realData = await _utilsService.ChangeLogLevel(1);

            //Assert

            Assert.True(realData.Element);
        }
    }
}
