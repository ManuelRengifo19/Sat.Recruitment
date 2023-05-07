using System;
using System.Threading.Tasks;
using Moq;
using Sat.Recruitment.Core.Enums;
using Sat.Recruitment.Core.Services;
using Sat.Recruitment.Domain.Entities;
using Sat.Recruitment.Infraestructure.IRepository;
using Xunit;

namespace Sat.Recruitment.Test.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task ValidateUserAsync_NullUser_ThrowsArgumentNullException()
        {
            //Arrange
            User user = null;

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.ValidateUserAsync(user));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ValidateUserAsync_NullOrEmptyName_ThrowsArgumentException(string name)
        {
            //Arrange
            User user = new User
            {
                Name = name,
                Email = "Manuel.Rengifo@example.com",
                Address = "Medellín",
                Phone = "+57 3174260124",
                UserType = UserTypeEnum.Normal.ToString(),
                Money = 100
            };

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.ValidateUserAsync(user));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ValidateUserAsync_NullOrEmptyEmail_ThrowsArgumentException(string email)
        {
            //Arrange
            User user = new User
            {
                Name = "Manuel Rengifo",
                Email = email,
                Address = "Medellín",
                Phone = "+57 3174260124",
                UserType = UserTypeEnum.Normal.ToString(),
                Money = 100
            };

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.ValidateUserAsync(user));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ValidateUserAsync_NullOrEmptyAddress_ThrowsArgumentException(string address)
        {
            //Arrange
            User user = new User
            {
                Name = "Manuel Rengifo",
                Email = "Manuel.Rengifo@example.com",
                Address = address,
                Phone = "+57 3174260124",
                UserType = UserTypeEnum.Normal.ToString(),
                Money = 100
            };

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.ValidateUserAsync(user));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ValidateUserAsync_NullOrEmptyPhone_ThrowsArgumentException(string phone)
        {
            //Arrange
            User user = new User
            {
                Name = "Manuel Rengifo",
                Email = "Manuel.Rengifo@example.com",
                Address = "Medellín",
                Phone = phone,
                UserType = UserTypeEnum.Normal.ToString(),
                Money = 100
            };

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.ValidateUserAsync(user));
        }

        [Fact]
        public async Task ValidateUserAsync_UserTypeNormal_AndMoneyGreaterThan100_AddsMoneyToUser()
        {
            //Arrange
            User user = new User
            {
                Name = "Manuel Rengifo",
                Email = "Manuel.Rengifo@example.com",
                Address = "Medellín",
                Phone = "+57 3174260124",
                UserType = UserTypeEnum.Normal.ToString(),
                Money = 150
            };

            //Act
            await _userService.ValidateUserAsync(user);

            //Assert
            Assert.Equal(168, user.Money);
        }

        [Fact]
        public async Task ValidateUserAsync_UserTypeNormal_AndMoneyGreaterThan10ButLessThan100_AddsMoneyToUser()
        {
            // Arrange
            User user = new User
            {
                Name = "Manuel Rengifo",
                Email = "Manuel.Rengifo@example.com",
                Address = "Medellín",
                Phone = "+57 3174260124",
                UserType = UserTypeEnum.Normal.ToString(),
                Money = 50
            };

            // Act
            await _userService.ValidateUserAsync(user);

            // Assert
            Assert.Equal(90, user.Money);
        }

        [Fact]
        public async Task ValidateUserAsync_UserTypeNormal_AndMoneyLessThan10_DoesNotAddMoneyToUser()
        {
            // Arrange
            User user = new User
            {
                Name = "Manuel Rengifo",
                Email = "Manuel.Rengifo@example.com",
                Address = "Medellín",
                Phone = "+57 3174260124",
                UserType = UserTypeEnum.Normal.ToString(),
                Money = 5
            };

            // Act
            await _userService.ValidateUserAsync(user);

            // Assert
            Assert.Equal(5, user.Money);
        }

        [Fact]
        public async Task ValidateUserAsync_UserTypeSuperUser_AndMoneyGreaterThan100_AddsMoneyToUser()
        {
            // Arrange
            User user = new User
            {
                Name = "Manuel Rengifo",
                Email = "Manuel.Rengifo@example.com",
                Address = "Medellín",
                Phone = "+57 3174260124",
                UserType = UserTypeEnum.SuperUser.ToString(),
                Money = 200
            };

            // Act
            await _userService.ValidateUserAsync(user);

            // Assert
            Assert.Equal(240, user.Money);
        }

        [Fact]
        public async Task ValidateUserAsync_UserTypeSuperUser_AndMoneyLessThanOrEqual100_DoesNotAddMoneyToUser()
        {
            // Arrange
            User user = new User
            {
                Name = "Manuel Rengifo",
                Email = "Manuel.Rengifo@example.com",
                Address = "Medellín",
                Phone = "+57 3174260124",
                UserType = UserTypeEnum.SuperUser.ToString(),
                Money = 100
            };

            // Act
            await _userService.ValidateUserAsync(user);

            // Assert
            Assert.Equal(100, user.Money);
        }

        [Fact]
        public async Task ValidateUserAsync_UserTypePremuin_AndMoneyGreaterThan100_AddsMoneyToUser()
        {
            // Arrange
            User user = new User
            {
                Name = "Manuel Rengifo",
                Email = "Manuel.Rengifo@example.com",
                Address = "Medellín",
                Phone = "+57 3174260124",
                UserType = UserTypeEnum.Premuin.ToString(),
                Money = 150
            };

            // Act
            await _userService.ValidateUserAsync(user);

            // Assert
            Assert.Equal(450, user.Money);
        }

        [Fact]
        public async Task ValidateUserAsync_UserTypePremuin_AndMoneyLessThanOrEqual100_DoesNotAddMoneyToUser()
        {
            // Arrange
            User user = new User
            {
                Name = "Manuel Rengifo",
                Email = "Manuel.Rengifo@example.com",
                Address = "Medellín",
                Phone = "+57 3174260124",
                UserType = UserTypeEnum.Premuin.ToString(),
                Money = 100
            };

            // Act
            await _userService.ValidateUserAsync(user);

            // Assert
            Assert.Equal(100, user.Money);
        }
    }
}


