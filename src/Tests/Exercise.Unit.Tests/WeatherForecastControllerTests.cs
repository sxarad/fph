using Exercise.Model;
using Exercise.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Exercise.Unit.Tests
{
    public class WeatherForecastControllerTests
    {
        [Fact]
        public void Should_Return_Forecast()
        {
            // Arrange
            var logger = Mock.Of<ILogger<WeatherForecastController>>();
            var context = Mock.Of<HttpContext>();

            // Act
            var controller = new WeatherForecastController(logger);
            controller.ControllerContext.HttpContext = context;
            var result = controller.Get();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<WeatherForecast>>(result);
        }
    }
}