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
    public class CartsControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IProvider> _providerMock;
        private readonly CartsController _sut;
        public CartsControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _providerMock = _fixture.Freeze<Mock<IProvider>>();
            _sut = new CartsController(_providerMock.Object);
        }

        [Fact]
        public async Task AddtoCart_ShouldReturnOkResponse_WhenValidInput()
        {
            var foodMock = _fixture.Create<Food>();
            var Id = _fixture.Create<int>();
            _providerMock.Setup(x => x.GetFoodById(Id)).ReturnsAsync(foodMock);
            var result = await _sut.AddtoCart(Id).ConfigureAwait(false);
            result.Should().NotBeNull();

            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
               .Should()
               .NotBeNull()
               .And.BeOfType(foodMock.GetType());
            _providerMock.Verify(x => x.GetFoodById(Id), Times.Once());
        }
        [Fact]
        public async Task AddtoCart_ShouldReturnOkResponse_WhenValidRequest()
        {
            var request = _fixture.Create<Cart>();
            var response = _fixture.Create<Cart>();
            _providerMock.Setup(x => x.AddtoCart(request)).ReturnsAsync(response);
            var result = await _sut.AddtoCart(request).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
               .Should()
               .NotBeNull()
               .And.BeOfType(response.GetType());

            _providerMock.Verify(x => x.AddtoCart(request), Times.Once());
        }
        [Fact]
        public async Task ViewCart_ShouldReturnOkResponse_WhenValidInput()
        {
            var foodMock = _fixture.Create<List<Cart>>();
            var Id = _fixture.Create<int>();
            _providerMock.Setup(x => x.GetCartByUserId(Id)).ReturnsAsync(foodMock);
            var result = await _sut.ViewCart(Id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<List<Cart>>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(foodMock.GetType());
            _providerMock.Verify(x => x.GetCartByUserId(Id), Times.Once());
        }
        [Fact]
        public async Task ViewCart_ShouldReturnNotFound_WhenDataNotFound()
        {
            List<Cart> response = null;
            var Id = _fixture.Create<int>();
            _providerMock.Setup(x => x.GetCartByUserId(Id)).ReturnsAsync(response);
            var result = await _sut.ViewCart(Id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundResult>();
            _providerMock.Verify(x => x.GetCartByUserId(Id), Times.Once());
        }
        [Fact]
        public async Task viewCart_ShouldReturnNoContents_WhenInsertedOrder()
        {
            var id = _fixture.Create<int>();
            _providerMock.Setup(x => x.ViewCart(id)).ReturnsAsync(true);
            var result = await _sut.viewCart(id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NoContentResult>();

        }
        [Fact]
        public async Task DeleteConfirmed_ShouldReturnNoContents_WhenDeletedARecord()
        {
            var id = _fixture.Create<int>();
            _providerMock.Setup(x => x.DeleteConfirmed(id)).ReturnsAsync(true);
            var result = await _sut.DeleteCartConfirmed(id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NoContentResult>();

        }
        [Fact]
        public async Task DeleteConfirmed_ShouldReturnNotFound_WhenRecordNotFound()
        {
            var id = _fixture.Create<int>();
            _providerMock.Setup(x => x.DeleteConfirmed(id)).ReturnsAsync(false);
            var result = await _sut.DeleteConfirmed(id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();

        }
        [Fact]
        public async Task EmptyList_ShouldReturnNoContents_WhenDeletedRecord()
        {
            var id = _fixture.Create<int>();
            _providerMock.Setup(x => x.EmptyList(id)).ReturnsAsync(true);
            var result = await _sut.EmptyList(id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NoContentResult>();

        }
        [Fact]
        public async Task EmptyList_ShouldReturnNotFound_WhenRecordNotFound()
        {
            var id = _fixture.Create<int>();
            _providerMock.Setup(x => x.EmptyList(id)).ReturnsAsync(false);
            var result = await _sut.EmptyList(id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();

        }
        [Fact]
        public async Task OrderDetails_ShouldReturnOkResponse_WhenDataFound()
        {
            var foodMock = _fixture.Create<List<OrderDetails>>();
            _providerMock.Setup(x => x.OrderDetails()).ReturnsAsync(foodMock);
            var result = await _sut.OrderDetails().ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<List<OrderDetails>>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(foodMock.GetType());
            _providerMock.Verify(x => x.OrderDetails(), Times.Once());
        }
        [Fact]
        public async Task OrderDetails_ShouldReturnNotFound_WhenDataNotFound()
        {
            List<OrderDetails> response = null;
            _providerMock.Setup(x => x.OrderDetails()).ReturnsAsync(response);
            var result = await _sut.OrderDetails().ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundResult>();
            _providerMock.Verify(x => x.OrderDetails(), Times.Once());
        }
        [Fact]
        public async Task Buy_ShouldReturnOkResponse_WhenValidInput()
        {
            var foodMock = _fixture.Create<OrderMaster>();
            var Id = _fixture.Create<int>();
            _providerMock.Setup(x => x.Buy(Id)).ReturnsAsync(foodMock);
            var result = await _sut.Buy(Id).ConfigureAwait(false);
            result.Should().NotBeNull();

            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
               .Should()
               .NotBeNull()
               .And.BeOfType(foodMock.GetType());
            _providerMock.Verify(x => x.Buy(Id), Times.Once());
        }
        [Fact]
        public async Task On_ShouldReturnOkResponse_WhenValidInput()
        {
            var foodMock = _fixture.Create<OrderMaster>();
            var Id = _fixture.Create<int>();
            _providerMock.Setup(x => x.Pay(Id)).ReturnsAsync(foodMock);
            var result = await _sut.On(Id).ConfigureAwait(false);
            result.Should().NotBeNull();

            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
               .Should()
               .NotBeNull()
               .And.BeOfType(foodMock.GetType());
            _providerMock.Verify(x => x.Pay(Id), Times.Once());
        }
        [Fact]
        public async Task On_ShouldReturnOkResponse_WhenRecordIsUpdated()
        {
            var foodMock = _fixture.Create<OrderMaster>();
            var Id = _fixture.Create<int>();
            _providerMock.Setup(x => x.Pay(Id, foodMock)).ReturnsAsync(true);
            var result = await _sut.On(Id, foodMock).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NoContentResult>();

            _providerMock.Verify(x => x.Pay(Id, foodMock), Times.Once());
        }
    }
}
