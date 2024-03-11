
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
//using ReadyTechBrewingCoApi.ReadyTechBrewingCoApi.Controllers;
using Xunit;
using Moq;
using System;
using ReadyTechBrewingCoApi.Wrappers;
using ReadyTechBrewingCoApi.Interfaces;

namespace ReadyTechBrewingCoApi.Tests;

public class ReadyTechBrewingCoControllerTests
{
    [Fact]
        public void BrewCoffee_ReturnsOkResult()
        {
           // Arrange
            int brewCount = 1;
            var memoryCacheWrapperMock = new Mock<IMemoryCacheWrapper>();
            memoryCacheWrapperMock.Setup(c => c.TryGetValue<int>(It.IsAny<object>(), out brewCount)).Returns(true);

            var loggerMock = new Mock<ILogger<ReadyTechBrewingCoController>>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.Now).Returns(DateTime.Now);

            var controller = new ReadyTechBrewingCoController(memoryCacheWrapperMock.Object, loggerMock.Object, dateTimeProviderMock.Object);

        // Act
        var result = controller.BrewCoffee();

        // Assert
        Assert.NotNull(result);
Assert.IsType<OkObjectResult>(result);

var objectResult = (ObjectResult)result;
Assert.Equal(200, objectResult.StatusCode);

        }

         [Fact]
        public void BrewCoffee_OutOfCoffee_Returns503ServiceUnavailableWithEmptyBody()
        {
            // Arrange
            int brewCount = 4;
            var memoryCacheWrapperMock = new Mock<IMemoryCacheWrapper>();
            memoryCacheWrapperMock.Setup(c => c.TryGetValue<int>(It.IsAny<object>(), out brewCount)).Returns(true);

            var loggerMock = new Mock<ILogger<ReadyTechBrewingCoController>>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.Now).Returns(DateTime.Now);

            var controller = new ReadyTechBrewingCoController(memoryCacheWrapperMock.Object, loggerMock.Object, dateTimeProviderMock.Object);

            // Act
            var result = controller.BrewCoffee();

            // Assert
            Assert.NotNull(result);            
            Assert.IsType<ContentResult>(result);      

            var contentResult = (ContentResult)result;
            Assert.Equal(503, contentResult.StatusCode);
            Assert.Equal("application/json", contentResult.ContentType);
            Assert.Equal(string.Empty, contentResult.Content);
        }

        [Fact]
        public void BrewCoffee_AprilFoolsDay_Returns418ImATeapotWithEmptyBody()
        {
            // Arrange
            int brewCount = 1;
            var memoryCacheWrapperMock = new Mock<IMemoryCacheWrapper>();
            memoryCacheWrapperMock.Setup(c => c.TryGetValue<int>(It.IsAny<object>(), out brewCount)).Returns(true);

            var loggerMock = new Mock<ILogger<ReadyTechBrewingCoController>>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.Now).Returns(new DateTime(DateTime.Now.Year, 4, 1));
            
            var controller = new ReadyTechBrewingCoController(memoryCacheWrapperMock.Object, loggerMock.Object, dateTimeProviderMock.Object);            

            // Act
            var result = controller.BrewCoffee();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ContentResult>(result);

            var contentResult = (ContentResult)result;
            Assert.Equal(418, contentResult.StatusCode);
            Assert.Equal("application/json", contentResult.ContentType);
            Assert.Equal(string.Empty, contentResult.Content);
        }

        [Fact]
        public void BrewCoffee_InternalServerError_Returns500InternalServerError()
        {
            // Arrange
           var memoryCacheWrapperMock = new Mock<IMemoryCacheWrapper>();
            memoryCacheWrapperMock.Setup(c => c.TryGetValue("brewCount", out It.Ref<int>.IsAny)).Throws(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<ReadyTechBrewingCoController>>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.Now).Returns(DateTime.Now);

            var controller = new ReadyTechBrewingCoController(memoryCacheWrapperMock.Object, loggerMock.Object, dateTimeProviderMock.Object);

            // Act
            var result = controller.BrewCoffee();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var statusCodeResult = (ObjectResult)result;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
}