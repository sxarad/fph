using Domain;
using Exercise.Core;
using Exercise.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Exercise.Unit.Tests
{
    public class AuthControllerTests
    {
        [Fact]
        public void Should_Return_Http_Ok()
        {
            // Arrange
            var customAuthService = new Mock<ICustomAuthenticationService>();
            customAuthService.Setup(a => a.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new System.IdentityModel.Tokens.Jwt.JwtSecurityToken());
            var context = Mock.Of<HttpContext>();

            // Act
            var controller = new AuthController(customAuthService.Object);
            controller.ControllerContext.HttpContext = context;
            var credential = new Credential() {  UserName = "test", Password = "P@ssword1100" };
            var result = controller.Authenticate(credential);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, ((Microsoft.AspNetCore.Mvc.OkObjectResult)result).StatusCode);
        }
    }
}