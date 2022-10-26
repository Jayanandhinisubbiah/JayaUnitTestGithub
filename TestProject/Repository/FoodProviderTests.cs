using APIProject.Models;
using APIProject.Provider;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject1.Provider;

namespace TestProject1.Repository
{
    public class FoodProviderTests
    {
        
        InMemoryDatabase db=new InMemoryDatabase();
        [Fact]
        public async void FoodProvider_GetAll_ReturnsFood()
        {
            //Arrange
           
            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            //Act
            var result = await foodProvider.GetAll();
            result.Should().NotBeNull();
            //Assert
            result.Should().BeOfType<List<Food>>();
            result.Should().HaveCount(7);


        }
        [Fact]
        public async void FoodProvider_GetFoodById_ReturnsFood()
        {//Arrange

            var Id = 90;
            var ExpectedFood = new Food()
            {

                FoodId = Id,
                CategoryName = "Soft Drinks",
                FoodName = "Lemon Juice90",
                price = 90,
                Image = "null",
                Detail = "Energetic90"

            };
            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);

            //Act
            var result = await foodProvider.GetFoodById(Id);
            //Assert
            result.Should().NotBeNull();
            //result.FoodName.Should().Be("Lemon Juice50");
            result.Should().BeEquivalentTo(ExpectedFood);
        }
        [Fact]
        public async void FoodProvider_EditFood_ReturnsBool()
        {//Arrange

            var Id = 60;
            var ExpectedFood = new Food()
            {
                FoodId = Id,
                CategoryName = "Soft Drinks",
                FoodName = "Musk Melon65",
                price = 65,
                Image = "null",
                Detail = "Energetic65"

            };

            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            var food = new Food()
            {

                CategoryName = "Soft Drinks",
                FoodName = "Musk Melon65",
                price = 65,
                Image = "null",
                Detail = "Energetic65"
            };
            //Act
            var result = await foodProvider.EditFood(Id, food);
            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async void FoodProvider_DeleteFood_ReturnsBool()
        {
            //Arrange
            var Id = 90;
            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            //Act
            var result = await foodProvider.DeleteFood(Id);
            //Assert
            result.Should().BeTrue();
            var foodList = await foodProvider.GetAll();
            foodList.Should().HaveCount(6);


        }
        [Fact]
        public async void FoodProvider_AddNewFood_ReturnsFood()
        {//Arrange

            var Id = 110;
            var Food = new Food()
            {
                FoodId = Id,
                CategoryName = "Soft Drinks",
                FoodName = "Mosambi Juice110",
                price = 110,
                Image = "null",
                Detail = "Energetic110"

            };

            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            //Act
            var result = await foodProvider.AddNewFood(Food);
            //Assert
            result.Should().NotBeNull();
            var foodList = await foodProvider.GetAll();
            foodList.Should().HaveCount(8);
        }
        [Fact]
        public async void FoodProvider_ViewNewOrder_ReturnsNewOrderList()
        {
            //Arrange
            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            //Act
            var result = await foodProvider.ViewNewOrder();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<NewOrder>>();




        }
        [Fact]
        public async void FoodProvider_DispatchNewOrderById_ReturnsNewOrder()
        {//Arrange


            var Id = 2;
            var Expected = new NewOrder()
            {
                Id = Id,
                OrderId = 150,
                Email = "ab@gmail.com",
                Image = "null",
                FoodName = "Lemon Juice50",
                Price = 50,
                Qnt = 1,
                TotalPrice = 50,
                Status = "Dispatch",

            };

            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            //Act
            var result = await foodProvider.DispatchNewOrder(Id);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(Expected);
        }

        [Fact]
        public async void FoodProvider_DispatchOrderById_ReturnsBool()
        {//Arrange
            var Id = 2;
            var Expected = new NewOrder()
            {
                Id = Id,
                OrderId = 150,
                Email = "ab@gmail.com",
                Image = "null",
                FoodName = "Lemon Juice50",
                Price = 50,
                Qnt = 1,
                TotalPrice = 50,
                Status = "Order Dispatched"

            };

            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);

            //Act
            var result = await foodProvider.DispatchOrder(Id);
            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async void FoodProvider_EmptyOrderByOrderId_ReturnsBool()
        {
            //Arrange
            var Id = 150;
            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            //Act
            var result = await foodProvider.EmptyOrder(Id);
            //Assert
            result.Should().BeTrue();
            var foodList = foodProvider.OrderDetails();
            foodList.Result.Should().HaveCount(1);
            //foodList.Should().;


        }
    }
}
