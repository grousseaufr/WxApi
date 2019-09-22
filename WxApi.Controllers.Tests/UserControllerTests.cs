using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WxApi.Dtos.User;
using WxApi.Services;
using WxAPI.Controllers;

namespace Tests
{
    public class UserControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UserController_ReturnsOkResultAndCorrectUserInfos()
        {
            // Arrange
            var userName = "John Doe";
            var token = "1111-2222-3333-4444";
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Get()).Returns(new User { Name = userName, Token = token });

            //Act
            var controller = new UserController(mockUserService.Object);
            var result = controller.Get();

            //Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result);

            var userResult = ((OkObjectResult)result).Value;
            Assert.IsInstanceOf(typeof(User), userResult);

            Assert.AreEqual(token, ((User)userResult).Token);
            Assert.AreEqual(userName, ((User)userResult).Name);

        }
    }
}