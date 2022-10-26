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
    public class CartProviderTests
    {
        InMemoryDatabase db = new InMemoryDatabase();
        [Fact]
        public async void FoodProvider_AddtoCart_ReturnsCart()
        {//Arrange

            //var Id = 2;
            var Cart = new Cart()
            {
               
                 //CartId = 2,
                UserId = 40,
               FoodId = 40,
                Qnt = 4
            };

            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            //Act
            var result = await foodProvider.AddtoCart(Cart);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Cart>();
        }
        [Fact]
        public async void FoodProvider_GetCartByUserId_ReturnsCartList()
        {//Arrange

            var Id = 40;

            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);

            //Act
            var result = await foodProvider.GetCartByUserId(Id);
            //Assert
            result.Should().NotBeNull();
            //result.FoodName.Should().Be("Lemon Juice50");
            result.Count.Should().Be(3);    
            
        }
        [Fact]
        public async void FoodProvider_ViewCart_ReturnsTrue()
        {//Arrange

            var Id = 40;
           

            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            //Act
            var result = await foodProvider.ViewCart(Id);
            //Assert
            result.Should().BeTrue();
            var OrderList = await foodProvider.OrderDetails();
            OrderList.Should().HaveCount(3);
           
        }
        [Fact]
        public async void FoodProvider_DeleteConfirmed_ReturnsBool()
        {
            //Arrange
            var Id = 1;
            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            //Act
            var result = await foodProvider.DeleteConfirmed(Id);
            //Assert
            result.Should().BeTrue();


        }
        [Fact]
        public async void FoodProvider_Payment_ReturnsOrderMaster()
        {//Arrange

            var Id = 140;
          
            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);


            string Type = "Offline";
               
          
            //Act
            var result = await foodProvider.Payment(Id, Type);
            //Assert
               result.Type.Should().Be(Type);
        }
    }
}
