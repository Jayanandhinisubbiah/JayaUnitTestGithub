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
    public class LoginProviderTests
    {
        InMemoryDatabase db = new InMemoryDatabase();
        [Fact]
        public async void FoodProvider_AddNewUser_ReturnsUserList()
        {//Arrange

            var Id = 42;
            var user = new UserList()
            {
                UserId = Id,
                Role = "User",
                FName = "Meena",
                Lname = "s",
                Gender = "Female",
                Email = "abcde@gmail.com",
                Address = "Vicky Street",
                City = "Karaikal",
                Password = "!USer1",

            };

            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            //Act
            var result = await foodProvider.AddNewUser(user);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserList>();
        }
        [Fact]
        public async void FoodProvider_Login_ReturnsUserList()
        {//Arrange

            var Id = 41;
            var user = new UserList()
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

            };

            var dbContext = await db.GetDatabaseContext();
            var foodProvider = new FoodProvider(dbContext);
            //Act
            var result = await foodProvider.Login(user);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserList>();
        }
    }
}
