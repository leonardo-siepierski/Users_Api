using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using usersapi.Controllers;
using usersapi.Models;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace usersapi_tests
{
    public class UsersControllerTest
    {
        private readonly UserContext _context;
        public UsersControllerTest()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;
            _context = new UserContext(options);
        }


        [Fact]
        public async Task PostUser_CreatesNewUser()
        {
            // Arrange

            var loggerStub = new Mock<ILogger<UsersController>>();

            var controller = new UsersController(_context, loggerStub.Object);

            // Act

            User user = new User()
            {
                FirstName = "teste",
                Surname = "teste",
                Age = 35
            };

            await controller.PostUser(user);
            User createdUser = await _context.Users.FirstAsync();
            var result = await controller.GetUser(createdUser.Id);

            // Assert

            result.Value.Should().BeOfType<User>();
        }

        [Fact]
        public async Task GetUserById_WithInvalidId_NotFound()
        {
            // Arrange

            var loggerStub = new Mock<ILogger<UsersController>>();

            var controller = new UsersController(_context, loggerStub.Object);

            // Act

            var result = await controller.GetUser("testing");

            // Assert

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetUserById_WithValidId_ReturnsUser()
        {
            // Arrange

            var loggerStub = new Mock<ILogger<UsersController>>();

            var controller = new UsersController(_context, loggerStub.Object);

            // Act

            User user = new User()
            {
                FirstName = "teste",
                Surname = "teste",
                Age = 35
            };

            await controller.PostUser(user);
            User createdUser = await _context.Users.FirstAsync();
            var result = await controller.GetUser(createdUser.Id);

            // Assert

            result.Value.Should().BeOfType<User>();
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnUsers()
        {
            // Arrange

            var loggerStub = new Mock<ILogger<UsersController>>();

            var controller = new UsersController(_context, loggerStub.Object);

            // Act

            User user = new User()
            {
                FirstName = "teste",
                Surname = "teste",
                Age = 35
            };

            await controller.PostUser(user);
            var result = controller.GetUsers();

            // Assert

            result.Value.Should().BeOfType<List<User>>();
        }

        [Fact]
        public async Task PutUser_ShouldUpdateUser()
        {
            // Arrange

            var loggerStub = new Mock<ILogger<UsersController>>();

            var controller = new UsersController(_context, loggerStub.Object);

            // Act

            User user = new User()
            {
                FirstName = "teste",
                Surname = "teste",
                Age = 35
            };

            await controller.PostUser(user);
            User createdUser = await _context.Users.FirstAsync();

            user.FirstName = "teste2";
            user.Surname = "teste2";
            user.Age = 32;

            var result = await controller.PutUser(createdUser.Id, user);

            // Assert

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteUser_WithInvalidId_CantDeleteAnUser()
        {
            // Arrange

            var loggerStub = new Mock<ILogger<UsersController>>();

            var controller = new UsersController(_context, loggerStub.Object);

            // Act

            User user = new User()
            {
                FirstName = "teste",
                Surname = "teste",
                Age = 35
            };

            var result = await controller.DeleteUser("something");

            // Assert

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteUser_DeletesAnUser()
        {
            // Arrange

            var loggerStub = new Mock<ILogger<UsersController>>();

            var controller = new UsersController(_context, loggerStub.Object);

            // Act

            User user = new User()
            {
                FirstName = "teste",
                Surname = "teste",
                Age = 35
            };

            await controller.PostUser(user);
            User createdUser = await _context.Users.FirstAsync();
            var result = await controller.DeleteUser(createdUser.Id);

            // Assert

            result.Value.Should().BeOfType<User>();
        }
    }
}
