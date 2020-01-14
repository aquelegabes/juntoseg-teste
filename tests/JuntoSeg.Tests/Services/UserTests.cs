using Autofac.Extras.Moq;
using JuntoSeg.Application.Services;
using JuntoSeg.Application.ViewModel.Requests;
using JuntoSeg.Data.Context;
using JuntoSeg.Data.UoW;
using JuntoSeg.Domain.Entities;
using JuntoSeg.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static JuntoSeg.Common.HashString;

namespace JuntoSeg.Tests.Services
{
    public class UserTests : MoqConfig
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task FindAsync_WhenCalled_Ok(int id)
        {
            // arrange
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepositoryUser>()
                    .Setup(repo => repo.FindAsync(id))
                    .ReturnsAsync(Users.First(u => u.Id.Equals(id)));
                mock.Mock<IRepositoryUser>()
                    .Setup(repo => repo.Dispose())
                    .Verifiable();

                using var repoUser = mock.Create<IRepositoryUser>();
                using var repoTokens = mock.Mock<IRepositoryToken>().SetupAllProperties().Object;
                using var ctx = new JuntoSegContext("Filename =./ db.sqlite3");

                var uow = new UnitOfWork(ctx, repoUser, repoTokens);
                var service = new UserService(uow, Mapper);

                // act
                var result = await service.FindAsync(id);
                mock.Mock<IRepositoryUser>()
                    .Verify(func => func.FindAsync(id), Times.Exactly(1));
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void Add_WhenCalled_Ok()
        {
            // arrange
            var req = new UserReq()
            {
                Id = 7,
                Name = "teste teste",
                Email = "test@test.te",
                NotHashedPassword = "12345"
            };
            var model = Mapper.Map<User>(req);

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepositoryUser>()
                    .Setup(repo => repo.Add(model));
                mock.Mock<IRepositoryUser>()
                    .Setup(repo => repo.Dispose());

                using var repoUser = mock.Create<IRepositoryUser>();
                using var repoTokens = mock.Mock<IRepositoryToken>().SetupAllProperties().Object;
                using var ctx = new JuntoSegContext("Filename =./ db.sqlite3");

                var uow = new UnitOfWork(ctx, repoUser, repoTokens);
                var service = new UserService(uow, Mapper);

                // act
                var result = service.Add(req);
                Assert.NotNull(result);
            }
        }
        
        [Fact]
        public async Task Update_WhenCalled_Ok()
        {
            // arrange
            var req = new UserReq()
            {
                Id = 7,
                Name = "teste teste",
                Email = "test@test.te",
                NotHashedPassword = "12345"
            };
            var model = Mapper.Map<User>(req);
            model.Passw = CreateHashString(req.NotHashedPassword);

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepositoryUser>()
                    .Setup(repo => repo.Update(model));
                mock.Mock<IRepositoryUser>()
                    .Setup(repo => repo.FindAsync(model.Id))
                    .ReturnsAsync(model);
                mock.Mock<IRepositoryUser>()
                    .Setup(repo => repo.Dispose());

                using var repoUser = mock.Create<IRepositoryUser>();
                using var repoTokens = mock.Mock<IRepositoryToken>().SetupAllProperties().Object;
                using var ctx = new JuntoSegContext("Filename =./ db.sqlite3");

                var uow = new UnitOfWork(ctx, repoUser, repoTokens);
                var service = new UserService(uow, Mapper);

                // act
                var result = await service.UpdateAsync(req);
                Assert.NotNull(result);
            }
        }
        
        [Fact]
        public async Task Delete_WhenCalled_Ok()
        {
            // arrange
            var req = new UserReq()
            {
                Id = 7,
                Name = "teste teste",
                Email = "test@test.te",
                NotHashedPassword = "12345",
                LoginPassword = "12345"
            };
            var model = Mapper.Map<User>(req);
            model.Passw = CreateHashString(req.NotHashedPassword);

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepositoryUser>()
                    .Setup(repo => repo.Remove(model));
                mock.Mock<IRepositoryUser>()
                    .Setup(repo => repo.FindAsync(model.Id))
                    .ReturnsAsync(model);
                mock.Mock<IRepositoryUser>()
                    .Setup(repo => repo.Dispose());

                using var repoUser = mock.Create<IRepositoryUser>();
                using var repoTokens = mock.Mock<IRepositoryToken>().SetupAllProperties().Object;
                using var ctx = new JuntoSegContext("Filename =./ db.sqlite3");

                var uow = new UnitOfWork(ctx, repoUser, repoTokens);
                var service = new UserService(uow, Mapper);

                // act
                var result = await service.RemoveAsync(req);
                Assert.True(result);
            }
        }

        [Fact]
        public async Task GenerateToken_WhenCalled_Ok()
        {
            // arrange
            var req = new UserReq()
            {
                Id = 7,
                Name = "teste teste",
                Email = "test@test.te",
                NotHashedPassword = "12345",
                LoginPassword = "12345"
            };
            var model = Mapper.Map<User>(req);
            model.Passw = CreateHashString(req.NotHashedPassword);

            var tokenModel = ValidationToken.GenerateToken();
            tokenModel.User = model;
            tokenModel.UserId = model.Id;

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepositoryToken>()
                    .Setup(repo => repo.Add(tokenModel));
                mock.Mock<IRepositoryUser>()
                    .Setup(repo => repo.FindAsync(model.Id))
                    .ReturnsAsync(model);
                mock.Mock<IRepositoryToken>()
                    .Setup(repo => repo.Dispose());

                using var repoUser = mock.Create<IRepositoryUser>();
                using var repoTokens = mock.Mock<IRepositoryToken>().SetupAllProperties().Object;
                using var ctx = new JuntoSegContext("Filename =./ db.sqlite3");

                var uow = new UnitOfWork(ctx, repoUser, repoTokens);
                var service = new UserService(uow, Mapper);

                // act
                var result = await service.GenerateValidationToken(req.Id);
                Assert.NotNull(result);
                Assert.NotEmpty(result);
            }
        }
    }
}
