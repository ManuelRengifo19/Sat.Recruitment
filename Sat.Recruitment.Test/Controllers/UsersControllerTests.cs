using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Core.IServices;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Core.Enums;
using System.Threading.Tasks;
using Xunit;


namespace Sat.Recruitment.Test.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> userServiceMock;
        private readonly UsersController controller;

        public UsersControllerTests()
        {
            userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<UsersController>>();
            controller = new UsersController(userServiceMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task Post_ValidUser_ReturnsOk()
        {
            // Arrange
            var user = new User
            {
                Name = "Manuel Rengifo",
                Email = "Manuel.Rengifo@example.com",
                Address = "Medellín",
                Phone = "+57 3174260124",
                UserType = UserTypeEnum.SuperUser.ToString(),
                Money = 100.00m
            };
            userServiceMock.Setup(x => x.ValidateUserAsync(user)).ReturnsAsync(true);

            // Act
            var result = await controller.Post(user);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("Succesfull", okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null));
            Assert.True((bool)okResult.Value.GetType().GetProperty("state").GetValue(okResult.Value, null));
            userServiceMock.Verify(x => x.ValidateUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task Post_InvalidUser_ReturnsBadRequest()
        {
            // Arrange
            var user = new User();
            userServiceMock.Setup(x => x.ValidateUserAsync(user)).ReturnsAsync(false);

            // Act
            var result = await controller.Post(user);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            userServiceMock.Verify(x => x.ValidateUserAsync(user), Times.Once);
        }
    }

}

