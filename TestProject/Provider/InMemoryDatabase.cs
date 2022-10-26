using APIProject.Data;
using APIProject.Models;
using APIProject.Provider;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.Provider
{
    public class InMemoryDatabase
    {
        public async Task<FoodContext> GetDatabaseContext()
        {
            
            var options=new DbContextOptionsBuilder<FoodContext>()
                .UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new FoodContext(options);
            databaseContext.Database.EnsureCreated();
            if(await databaseContext.Food.CountAsync()<=0)
            {
                for(int i=40;i<=100;i+=10)
                {
                    databaseContext.Food.Add(
                        new Food()
                        {
                          
                            FoodId = i,
                            CategoryName = "Soft Drinks",
                            FoodName = "Lemon Juice" + i,
                            price = i,
                            Image = "null",
                            Detail = "Energetic" + i,
                          
                        }) ;
                   
                }
                databaseContext.SaveChanges();

            }
            if (await databaseContext.UserList.CountAsync() <= 0)
            {

                var authors = new List<UserList>
                {
                         new UserList
                         {
                             UserId = 40,
                             Role="User",
                             FName = "Jaya",
                             Lname="s",
                             Gender="Female",
                             Email="abc@gmail.com",
                             Address="pinky Street",
                             City="Karaikudi",
                             Password="!User1",


                         },
                new UserList
                {
                     UserId = 41,
                    Role = "Admin",
                    FName = "Ajoe",
                    Lname = "V",
                    Gender = "Male",
                    Email = "joel@gmail.com",
                    Address = "Rosy Street",
                    City = "Ooty",
                    Password = "!Admin1",


                } };
                databaseContext.UserList.AddRange(authors);

                databaseContext.SaveChanges();

            }
            if (await databaseContext.Cart.CountAsync() <= 0)

            {
                //for (int i = 1; i < 5; i++)
                //{
                int i = 1;
                    //databaseContext.Cart.Add(
                    //    new Cart()
                    //    {
                    //        CartId = i++,
                    //        UserId = 40,
                    //        FoodId = 40,
                    //        Qnt = 2

                    //    });
                //}
                var authors = new List<Cart>
                {
                         new Cart
                         {
                             CartId = i++,
                            UserId = 40,
                            FoodId = 40,
                            Qnt = 2


                         },
                new Cart
                {
                      CartId = i++,
                            UserId = 40,
                            FoodId = 50,
                            Qnt = 6


                },
                 new Cart
                {
                      CartId = i++,
                            UserId = 40,
                            FoodId = 60,
                            Qnt = 8


                }
                };

                databaseContext.Cart.AddRange(authors);

                databaseContext.SaveChanges();


                //databaseContext.SaveChanges();

            }
            if (await databaseContext.OrderDetails.CountAsync() <= 0)

            {
                int j = 1;
                //for (int i = 140; i <= 200; i += 10)
                //{
                var authors = new List<OrderDetails>
                {
                        new OrderDetails()
                        {
                            Id = j++,
                            OrderId = 140,
                            FoodId = 90,
                            Qnt = 1,
                            Price = 40,
                            TotalPrice = 40

                        },
                 //new OrderDetails()
                 //       {
                 //           Id = j++,
                 //           OrderId = 150,
                 //           FoodId = 50,
                 //           Qnt = 1,
                 //           Price = 50,
                 //           TotalPrice = 50

                       /*}*/ };   

            //}
            databaseContext.OrderDetails.AddRange(authors);

            databaseContext.SaveChanges();

            }
            if (await databaseContext.OrderMaster.CountAsync() <= 0)

            {

                //for (int i = 140; i <= 200; i += 10)
                //{
                var authors = new List<OrderMaster>
                {
                        new OrderMaster()
                        {
                            OrderId = 140,
                            UserId = 40,
                            TotalPrice = 40,
                            //Type = "Online",
                            //BankName = "City Union Bank",
                            //CardNo = "123456789",
                            //CCV = 123
                            Type = "",
                            BankName = "",
                            CardNo = "",
                            CCV = 0
                        },
                new OrderMaster()
                        {
                            OrderId = 150,
                            UserId = 41,
                            TotalPrice = 50,
                            Type = "Online",
                            BankName = "City Union Bank",
                            CardNo = "123456789",
                            CCV = 456
                        } };

                //}
                databaseContext.OrderMaster.AddRange(authors);

                databaseContext.SaveChanges();

            }
            if (await databaseContext.NewOrder.CountAsync() <= 0)

            {
                int j = 1;
                //for (int i = 140; i <= 200; i += 10)
                //{


                var authors = new List<NewOrder>
                {

                    new NewOrder()
                    {
                        Id = j++,
                        OrderId = 140,
                        Email = "abc@gmail.com",
                        Image = "null",
                        FoodName = "Lemon Juice40",
                        Price = 40,
                        Qnt = 1,
                        TotalPrice = 40,
                        Status = "Dispatch",

                    },
                     new NewOrder()
                     {
                         Id = j++,
                         OrderId = 150,
                         Email = "ab@gmail.com",
                         Image = "null",
                         FoodName = "Lemon Juice50",
                         Price = 50,
                         Qnt = 1,
                         TotalPrice = 50,
                         Status = "Dispatch",

                     }};
                databaseContext.NewOrder.AddRange(authors);
                //}
                databaseContext.SaveChanges();

            }

            return databaseContext;
        }
        //[Fact]
        //public async void FoodProvider_GetAll_ReturnsFood()
        //{
        //    //Arrange
        //    var dbContext = await GetDatabaseContext();
        //    var foodProvider = new FoodProvider(dbContext);
        //    //Act
        //    var result = await foodProvider.GetAll();
        //    result.Should().NotBeNull();
        //    //Assert
        //    result.Should().BeOfType<List<Food>>();
        //    result.Should().HaveCount(7);


        //}
        //[Fact]
        //public async void FoodProvider_GetFoodById_ReturnsFood()
        //{//Arrange

        //    var Id = 90;
        //    var ExpectedFood = new Food()
        //    {
               
        //        FoodId = Id,
        //        CategoryName = "Soft Drinks",
        //        FoodName = "Lemon Juice90",
        //        price = 90,
        //        Image = "null",
        //        Detail = "Energetic90"
               
        //    };
        //    var dbContext = await GetDatabaseContext();
        //    var foodProvider = new FoodProvider(dbContext);

        //    //Act
        //    var result = await foodProvider.GetFoodById(Id);
        //    //Assert
        //    result.Should().NotBeNull();
        //    //result.FoodName.Should().Be("Lemon Juice50");
        //    result.Should().BeEquivalentTo(ExpectedFood);
        //}
        //[Fact]
        //public async void FoodProvider_EditFood_ReturnsBool()
        //{//Arrange

        //    var Id = 60;
        //    var ExpectedFood = new Food()
        //    {
        //        FoodId = Id,
        //        CategoryName = "Soft Drinks",
        //        FoodName = "Musk Melon65",
        //        price = 65,
        //        Image = "null",
        //        Detail = "Energetic65"

        //    };

        //    var dbContext = await GetDatabaseContext();
        //    var foodProvider = new FoodProvider(dbContext);
        //    var food = new Food()
        //    {
               
        //        CategoryName = "Soft Drinks",
        //        FoodName = "Musk Melon65",
        //        price = 65,
        //        Image = "null",
        //        Detail = "Energetic65"
        //    };
        //    //Act
        //    var result = await foodProvider.EditFood(Id, food);
        //    //Assert
        //    result.Should().BeTrue();
        //}
        //[Fact]
        //public async void FoodProvider_DeleteFood_ReturnsBool()
        //{
        //    //Arrange
        //    var Id = 90;
        //    var dbContext = await GetDatabaseContext();
        //    var foodProvider = new FoodProvider(dbContext);
        //    //Act
        //    var result = await foodProvider.DeleteFood(Id);
        //    //Assert
        //    result.Should().BeTrue();
        //    var foodList = await foodProvider.GetAll();
        //    foodList.Should().HaveCount(6);


        //}
        //[Fact]
        //public async void FoodProvider_AddNewFood_ReturnsFood()
        //{//Arrange

        //    var Id = 110;
        //    var Food = new Food()
        //    {
        //        FoodId = Id,
        //        CategoryName = "Soft Drinks",
        //        FoodName = "Mosambi Juice110",
        //        price = 110,
        //        Image = "null",
        //        Detail = "Energetic110"

        //    };

        //    var dbContext = await GetDatabaseContext();
        //    var foodProvider = new FoodProvider(dbContext);
        //    //Act
        //    var result = await foodProvider.AddNewFood(Food);
        //    //Assert
        //    result.Should().NotBeNull();
        //    var foodList = await foodProvider.GetAll();
        //    foodList.Should().HaveCount(8);
        //}
        //[Fact]
        //public async void FoodProvider_ViewNewOrder_ReturnsNewOrderList()
        //{
        //    //Arrange
        //    var dbContext = await GetDatabaseContext();
        //    var foodProvider = new FoodProvider(dbContext);
        //    //Act
        //    var result = await foodProvider.ViewNewOrder();

        //    //Assert
        //    result.Should().NotBeNull();
        //    result.Should().BeOfType<List<NewOrder>>();




        //}
        //[Fact]
        //public async void FoodProvider_DispatchNewOrderById_ReturnsNewOrder()
        //{//Arrange
           

        //    var Id = 2;
        //    var Expected = new NewOrder()
        //    {
        //        Id = Id,
        //        OrderId = 150,
        //        Email = "ab@gmail.com",
        //        Image = "null",
        //        FoodName = "Lemon Juice50",
        //        Price = 50,
        //        Qnt = 1,
        //        TotalPrice = 50,
        //        Status = "Dispatch",

        //    };

        //    var dbContext = await GetDatabaseContext();
        //    var foodProvider = new FoodProvider(dbContext);
        //    //Act
        //    var result = await foodProvider.DispatchNewOrder(Id);
        //    //Assert
        //    result.Should().NotBeNull();
        //    result.Should().BeEquivalentTo(Expected);
        //}

        //[Fact]
        //public async void FoodProvider_DispatchOrderById_ReturnsBool()
        //{//Arrange
        //    var Id = 2;
        //    var Expected = new NewOrder()
        //    {
        //        Id = Id,
        //        OrderId = 150,
        //        Email = "ab@gmail.com",
        //        Image = "null",
        //        FoodName = "Lemon Juice50",
        //        Price = 50,
        //        Qnt = 1,
        //        TotalPrice = 50,
        //        Status = "Order Dispatched"

        //    };

        //    var dbContext = await GetDatabaseContext();
        //var foodProvider = new FoodProvider(dbContext);
        
        ////Act
        //var result = await foodProvider.DispatchOrder(Id);
        ////Assert
        //result.Should().BeTrue();
        //}
        //[Fact]
        //public async void FoodProvider_EmptyOrderByOrderId_ReturnsBool()
        //{
        //    //Arrange
        //    var Id = 150;
        //    var dbContext = await GetDatabaseContext();
        //    var foodProvider = new FoodProvider(dbContext);
        //    //Act
        //    var result = await foodProvider.EmptyOrder(Id);
        //    //Assert
        //    result.Should().BeTrue();
        //    var foodList =  foodProvider.OrderDetails();
        //    foodList.Should().HaveCount(1);


        //}

    }
}
