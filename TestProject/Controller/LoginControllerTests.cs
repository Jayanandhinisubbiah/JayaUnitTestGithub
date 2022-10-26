using APIProject.Controllers;
using APIProject.Models;
using APIProject.Provider;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
namespace APITestProject.Controller
{
    public class LoginControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IProvider> _providerMock;
        private readonly LoginController _sut;
        public LoginControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _providerMock = _fixture.Freeze<Mock<IProvider>>();
            _sut = new LoginController(_providerMock.Object);
        }
        [Fact]
        public async Task PostUserList_ShouldReturnOkResponse_WhenValidRequest()
        
        {
            //Arrange
            var request = _fixture.Create<UserList>();
            var response = _fixture.Create<UserList>();
            _providerMock.Setup(x => x.AddNewUser(request)).ReturnsAsync(response);
            //Act
            var result = await _sut.PostUserList(request).ConfigureAwait(false);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<UserList>>();

            _providerMock.Verify(x => x.AddNewUser(request), Times.Once());
        }
        [Fact]
        public async Task PostUserList_ShouldReturnBadRequest_WhenInValidRequest()
        {
            //Arrange
            var request = _fixture.Create<UserList>();
            _sut.ModelState.AddModelError("UserName", "The User Name is required");
            var response = _fixture.Create<UserList>();
            _providerMock.Setup(x => x.AddNewUser(request)).ReturnsAsync(response);
            //Act
            var result = await _sut.PostUserList(request).ConfigureAwait(false);
            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestResult>();
            _providerMock.Verify(x => x.AddNewUser(request), Times.Never());
        }
        [Fact]
        public async Task LoginUser_ShouldReturnOkResponse_WhenValidRequest()

        {
            //Arrange
            var request = _fixture.Create<UserList>();
            var response = _fixture.Create<UserList>();
            _providerMock.Setup(x => x.Login(request)).ReturnsAsync(response);
            //Act
            var result = await _sut.LoginUser(request).ConfigureAwait(false);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<UserList>>();

            _providerMock.Verify(x => x.Login(request), Times.Once());
        }
        [Fact]
        public async Task LoginUser_ShouldReturnBadRequest_WhenInValidRequest()
        {
            //Arrange
            var request = _fixture.Create<UserList>();
            _sut.ModelState.AddModelError("UserName", "The User Name is required");
            var response = _fixture.Create<UserList>();
            _providerMock.Setup(x => x.Login(request)).ReturnsAsync(response);
            //Act
            var result = await _sut.LoginUser(request).ConfigureAwait(false);
            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestResult>();
            _providerMock.Verify(x => x.Login(request), Times.Never());
        }
    }
}
