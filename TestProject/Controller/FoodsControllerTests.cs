using Xunit;
using Moq;
using FluentAssertions;
using AutoFixture;
using APIProject.Provider;
using APIProject.Controllers;
using APIProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace APITestProject.Controller
{
    public class FoodsControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IProvider> _providerMock;
        private readonly FoodsController _sut;
        public FoodsControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _providerMock = _fixture.Freeze<Mock<IProvider>>();
            _sut = new FoodsController(_providerMock.Object);
        }
        [Fact]
        public async Task GetFood_ShouldReturnOkResponse_WhenDataFound()
        {
            var foodsMock = _fixture.Create<List<Food>>();
            _providerMock.Setup(x => x.GetAll()).ReturnsAsync(foodsMock);
            var result = await _sut.GetFood().ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<List<Food>>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(foodsMock.GetType());
            _providerMock.Verify(x => x.GetAll(), Times.Once());
        }
        [Fact]
        public async Task GetFood_ShouldReturnNotFound_WhenDataNotFound()
        {
            List<Food> response = null;
            _providerMock.Setup(x => x.GetAll()).ReturnsAsync(response);
            var result = await _sut.GetFood().ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundResult>();
            _providerMock.Verify(x => x.GetAll(), Times.Once());
        }
        [Fact]
        public async Task GetFoodById_ShouldReturnOkResponse_WhenValidInput()
        {
            var foodMock = _fixture.Create<Food>();
            var Id = _fixture.Create<int>();
            _providerMock.Setup(x => x.GetFoodById(Id)).ReturnsAsync(foodMock);
            var result = await _sut.GetFoodById(Id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<Food>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
               .Should()
               .NotBeNull()
               .And.BeOfType(foodMock.GetType());
            _providerMock.Verify(x => x.GetFoodById(Id), Times.Once());
        }
        [Fact]
        public async Task GetFoodById_ShouldReturnNotFound_WhenDataNotFound()
        {
            Food response = null;
            var Id = _fixture.Create<int>();
            _providerMock.Setup(x => x.GetFoodById(Id)).ReturnsAsync(response);
            var result = await _sut.GetFoodById(Id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundResult>();
            _providerMock.Verify(x => x.GetFoodById(Id), Times.Once());

        }
        [Fact]
        public async Task GetFoodById_ShouldReturnBadRequest_WhenInputIsEqualsZero()
        {
            var foodMock = _fixture.Create<Food>();
            var Id = 0;
            _providerMock.Setup(x => x.GetFoodById(Id)).ReturnsAsync(foodMock);
            var result = await _sut.GetFoodById(Id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestResult>();
            _providerMock.Verify(x => x.GetFoodById(Id), Times.Never());
        }
        [Fact]
        public async Task GetFoodById_ShouldReturnBadRequest_WhenInputIsLessThanZero()
        {
            var foodMock = _fixture.Create<Food>();
            var Id = -1;
            _providerMock.Setup(x => x.GetFoodById(Id)).ReturnsAsync(foodMock);
            var result = await _sut.GetFoodById(Id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestResult>();
            _providerMock.Verify(x => x.GetFoodById(Id), Times.Never());
        }
        [Fact]
        public async Task PostFood_ShouldReturnOkResponse_WhenValidRequest()
        {
            var request = _fixture.Create<Food>();
            var response = _fixture.Create<Food>();
            _providerMock.Setup(x => x.AddNewFood(request)).ReturnsAsync(response);
            var result = await _sut.PostFood(request).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<Food>>();
            result.Result.Should().BeAssignableTo<CreatedAtActionResult>();

            _providerMock.Verify(x => x.AddNewFood(request), Times.Once());
        }
        [Fact]
        public async Task PostFood_ShouldReturnBadRequest_WhenInValidRequest()
        {
            var request = _fixture.Create<Food>();
            _sut.ModelState.AddModelError("FoodName", "The Food Name is required");
            var response = _fixture.Create<Food>();
            _providerMock.Setup(x => x.AddNewFood(request)).ReturnsAsync(response);
            var result = await _sut.PostFood(request).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestResult>();
            _providerMock.Verify(x => x.AddNewFood(request), Times.Never());
        }
        [Fact]
        public async Task DeleteFoodById_ShouldReturnNoContents_WhenDeletedARecord()
        {
            var id = _fixture.Create<int>();
            _providerMock.Setup(x => x.DeleteFood(id)).ReturnsAsync(true);
            var result = await _sut.DeleteFood(id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NoContentResult>();

        }
        [Fact]
        public async Task DeleteFoodById_ShouldReturnNotFound_WhenRecordNotFound()
        {
            var id = _fixture.Create<int>();
            _providerMock.Setup(x => x.DeleteFood(id)).ReturnsAsync(false);
            var result = await _sut.DeleteFood(id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();

        }
        [Fact]
        public async Task DeleteFoodById_ShouldReturnBadResponse_WhenInputIsEqualsZero()
        {
            var Id = 0;
            _providerMock.Setup(x => x.DeleteFood(Id)).ReturnsAsync(false);
            var result = await _sut.DeleteFood(Id).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();

            _providerMock.Verify(x => x.DeleteFood(Id), Times.Never());
        }
        [Fact]
        public async Task EditFood_ShouldReturnOkResponse_WhenRecordIsUpdated()
        {
            var foodMock = _fixture.Create<Food>();
            var Id = _fixture.Create<int>();
            _providerMock.Setup(x => x.EditFood(Id, foodMock)).ReturnsAsync(true);
            var result = await _sut.EditFood(Id, foodMock).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();

            _providerMock.Verify(x => x.EditFood(Id, foodMock), Times.Once());
        }
        [Fact]
        public async Task EditFood_ShouldReturnNotFound_WhenRecordNotFound()
        {
            var foodMock = _fixture.Create<Food>();
            var Id = _fixture.Create<int>();
            _providerMock.Setup(x => x.EditFood(Id, foodMock)).ReturnsAsync(false);
            var result = await _sut.EditFood(Id, foodMock).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();

            _providerMock.Verify(x => x.EditFood(Id, foodMock), Times.Once());

        }
        [Fact]
        public async Task EditFood_ShouldReturnBadRequest_WhenInValidRequest()
        {
            var id = _fixture.Create<int>();
            var request = _fixture.Create<Food>();
            _sut.ModelState.AddModelError("FoodName", "The Food Name is required");
            _providerMock.Setup(x => x.EditFood(id, request)).ReturnsAsync(false);
            var result = await _sut.EditFood(id, request).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _providerMock.Verify(x => x.EditFood(id, request), Times.Never());
        }
        [Fact]
        public async Task EditFood_ShouldReturnBadResponse_WhenInputIsEqualsZero()
        {
            var Id = 0;
            var request = _fixture.Create<Food>();
            _providerMock.Setup(x => x.EditFood(Id, request)).ReturnsAsync(false);
            var result = await _sut.EditFood(Id, request).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();

            _providerMock.Verify(x => x.EditFood(Id, request), Times.Never());
        }
        [Fact]
        public async Task GetNewOrder_ShouldReturnOkResponse_WhenDataFound()
        {
            var foodsMock = _fixture.Create<List<NewOrder>>();
            _providerMock.Setup(x => x.ViewNewOrder()).ReturnsAsync(foodsMock);
            var result = await _sut.GetNewOrder().ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<List<NewOrder>>>();

            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should()
                .NotBeNull()
                .And.BeOfType(foodsMock.GetType());
            _providerMock.Verify(x => x.ViewNewOrder(), Times.Once());
        }
        [Fact]
        public async Task GetNewOrder_ShouldReturnNotFound_WhenDataNotFound()
        {
            List<NewOrder> response = null;
            _providerMock.Setup(x => x.ViewNewOrder()).ReturnsAsync(response);
            var result = await _sut.GetNewOrder().ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundResult>();
            _providerMock.Verify(x => x.ViewNewOrder(), Times.Once());
        }
        //[Fact]
        //public async Task DeleteFoodById_ShouldReturnNoContents_WhenDeletedARecord()
        //{
        //    var id = _fixture.Create<int>();
        //    _providerMock.Setup(x => x.DeleteFood(id)).ReturnsAsync(true);
        //    var result = await _sut.DeleteFood(id).ConfigureAwait(false);
        //    result.Should().NotBeNull();
        //    result.Should().BeAssignableTo<NoContentResult>();

        //}
        //[Fact]
        //public async Task DeleteFoodById_ShouldReturnNotFound_WhenRecordNotFound()
        //{
        //    var id = _fixture.Create<int>();
        //    _providerMock.Setup(x => x.DeleteFood(id)).ReturnsAsync(false);
        //    var result = await _sut.DeleteFood(id).ConfigureAwait(false);
        //    result.Should().NotBeNull();
        //    result.Should().BeAssignableTo<NotFoundResult>();

        //}
        //[Fact]
        //public async Task DeleteFoodById_ShouldReturnBadResponse_WhenInputIsEqualsZero()
        //{
        //    var Id = 0;
        //    _providerMock.Setup(x => x.DeleteFood(Id)).ReturnsAsync(false);
        //    var result = await _sut.DeleteFood(Id).ConfigureAwait(false);
        //    result.Should().NotBeNull();
        //    result.Should().BeAssignableTo<BadRequestResult>();

        //    _providerMock.Verify(x => x.DeleteFood(Id), Times.Never());
        //}
    }
}
