using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WebApiTest.Data;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Models;
using WebApiTest.Controllers;
using WebApiTest.Models.Dto;
using WebAPITest.XUnitTests.Extensions;
using WebAPITest.XUnitTests.Infra;
using WebApiTest.Services.UtilsService;

namespace WebAPITest.XUnitTests.Common
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
