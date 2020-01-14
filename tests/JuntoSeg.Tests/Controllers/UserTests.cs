using Autofac.Extras.Moq;
using JuntoSeg.Api.Controllers;
using JuntoSeg.Application.Interfaces;
using JuntoSeg.Application.ViewModel.Requests;
using JuntoSeg.Application.ViewModel.Response;
using JuntoSeg.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static JuntoSeg.Common.HashString;

namespace JuntoSeg.Tests.Controllers
{
    public class UserTests : MoqConfig
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Get_WhenCalled_Ok(int id)
        {
            using (var mock = AutoMock.GetStrict())
            {
                // arrange
                mock.Mock<IUserService>()
                    .Setup(serv => serv.FindAsync(id))
                    .ReturnsAsync(Mapper.Map<UserResp>(Users.First(u => u.Id.Equals(id))));

                var service = mock.Create<IUserService>();
                var controller = new UserController(service);

                // act
                var okResult = await controller.Get(id);
                var okObjectResult = okResult as OkObjectResult;
                var okResultModel = okObjectResult.Value as UserResp;

                // assert
                mock.Mock<IUserService>()
                   .Verify(func => func.FindAsync(id), Times.Exactly(1));
                Assert.NotNull(okResult);
                Assert.IsType<OkObjectResult>(okResult);
                Assert.NotNull(okObjectResult);
                Assert.NotNull(okResultModel);
            }
        }
        
        [Theory]
        [InlineData(1)]
        public async Task Get_WhenCalled_Timeout(int id)
        {
            using (var mock = AutoMock.GetStrict())
            {
                // arrange
                mock.Mock<IUserService>()
                    .Setup(serv => serv.FindAsync(id))
                    .ThrowsAsync(new TimeoutException());

                var service = mock.Create<IUserService>();
                var controller = new UserController(service);

                // act
                var status500 = await controller.Get(id);
                var objectResult = status500 as ObjectResult;

                // assert
                mock.Mock<IUserService>()
                   .Verify(func => func.FindAsync(id), Times.Exactly(1));
                Assert.NotNull(status500);
                Assert.IsAssignableFrom<ObjectResult>(status500);
                Assert.True(objectResult.StatusCode == 500);
            }
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_WhenCalled_BadReq(int id)
        {
            using (var mock = AutoMock.GetStrict())
            {
                // arrange
                var service = mock.Mock<IUserService>().SetupAllProperties().Object;
                var controller = new UserController(service);

                // act
                var badReq = await controller.Get(id);

                // assert
                Assert.NotNull(badReq);
                Assert.IsType<BadRequestResult>(badReq);
            }
        }

        [Fact]
        public void Add_WhenCalled_Ok()
        {
            using (var mock = AutoMock.GetStrict())
            {
                // arrange
                var model = new User()
                {
                    Id = 7,
                    Name = "teste teste",
                    Email = "test@test.te",
                    CreatedAt = DateTime.UtcNow,
                    Passw = CreateHashString("12345"),
                    UpdatedAt = DateTime.UtcNow
                };
                var req = Mapper.Map<UserReq>(model);
                req.NotHashedPassword = "12345";

                mock.Mock<IUserService>()
                    .Setup(serv => serv.Add(req))
                    .Returns(Mapper.Map<UserResp>(model));

                var service = mock.Create<IUserService>();
                var controller = new UserController(service);

                // act
                var result = controller.Add(req);
                var createdResult = result as CreatedResult;
                var modelResult = createdResult.Value as UserResp;

                // assert
                mock.Mock<IUserService>()
                    .Verify(func => func.Add(req), Times.Exactly(1));
                Assert.IsType<CreatedResult>(result);
                Assert.NotNull(createdResult);
                Assert.IsAssignableFrom<UserResp>(createdResult.Value);
                Assert.NotNull(modelResult);
            }
        }

        [Fact]
        public async Task Update_WhenCalled_Ok()
        {
            using (var mock = AutoMock.GetStrict())
            {
                // arrange
                var model = new User()
                {
                    Id = 7,
                    Name = "teste teste",
                    Email = "test@test.te",
                    CreatedAt = DateTime.UtcNow,
                    Passw = CreateHashString("12345"),
                    UpdatedAt = DateTime.UtcNow
                };
                var req = Mapper.Map<UserReq>(model);
                req.NotHashedPassword = "12345";

                mock.Mock<IUserService>()
                    .Setup(serv => serv.UpdateAsync(req))
                    .ReturnsAsync(Mapper.Map<UserResp>(model));

                var service = mock.Create<IUserService>();
                var controller = new UserController(service);

                // act
                var result = await controller.Update(req);
                var okObjectResult = result as OkObjectResult;
                var modelResult = okObjectResult.Value as UserResp;

                // assert
                mock.Mock<IUserService>()
                    .Verify(func => func.UpdateAsync(req), Times.Exactly(1));
                Assert.IsType<OkObjectResult>(result);
                Assert.NotNull(okObjectResult);
                Assert.IsAssignableFrom<UserResp>(okObjectResult.Value);
                Assert.NotNull(modelResult);
            }
        }

        [Fact]
        public async Task Delete_WhenCalled_Ok()
        {
            // arrange
            using (var mock = AutoMock.GetStrict())
            {
                var model = new User()
                {
                    Id = 7,
                    Name = "teste teste",
                    Email = "test@test.te",
                    CreatedAt = DateTime.UtcNow,
                    Passw = CreateHashString("12345"),
                    UpdatedAt = DateTime.UtcNow
                };

                var req = Mapper.Map<UserReq>(model);
                req.NotHashedPassword = "12345";

                mock.Mock<IUserService>()
                    .Setup(serv => serv.RemoveAsync(req))
                    .ReturnsAsync(true);

                var service = mock.Create<IUserService>();
                var controller = new UserController(service);

                // act
                var result = await controller.Delete(req);
                var okResult = result as OkObjectResult;
                var boolResult = okResult.Value as bool?;

                Assert.NotNull(result);
                Assert.IsAssignableFrom<OkObjectResult>(result);
                Assert.NotNull(okResult);
                Assert.IsAssignableFrom<bool?>(okResult.Value);
                Assert.True(boolResult);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GenerateToken_WhenCalled_Ok(int userid)
        {
            // arrange
            string token = ValidationToken.GenerateToken().Token;
            using (var mock = AutoMock.GetStrict())
            {
                mock.Mock<IUserService>()
                    .Setup(serv => serv.GenerateValidationToken(userid))
                    .ReturnsAsync(token);

                var service = mock.Create<IUserService>();
                var controller = new UserController(service);

                // act
                var result = await controller.GenerateToken(userid);
                var okResult = result as OkObjectResult;
                string resltToken = okResult.Value as string;

                mock.Mock<IUserService>()
                    .Verify(func => func.GenerateValidationToken(userid), Times.Exactly(1));
                Assert.NotNull(result);
                Assert.IsAssignableFrom<OkObjectResult>(result);
                Assert.NotNull(okResult);
                Assert.IsAssignableFrom<string>(okResult.Value);
                Assert.NotEmpty(resltToken);
            }
        }
    }
}
