using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Extras.Moq;
using AutoMapper;
using JuntoSeg.Application.Mapper;
using JuntoSeg.Domain.Entities;
using Moq;
using static JuntoSeg.Common.HashString;

namespace JuntoSeg.Tests
{
    public abstract class MoqConfig
    {
        protected ICollection<User> Users { get; }
        protected ICollection<ValidationToken> Tokens { get; }
        protected IMapper Mapper { get; }

        public MoqConfig()
        { 
            // users
            Users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Name = "Gabriel Santos",
                    Email = "gsantos13@gmail.com",
                    Passw = CreateHashString("123")
                },new User()
                {
                    Id = 2,
                    Name = "Ricardo Oliveira",
                    Email = "roliver@gmail.com",
                    Passw = CreateHashString("123")
                },new User()
                {
                    Id = 3,
                    Name = "Paulo Oliveira",
                    Email = "pauloliveira@gmail.com",
                    Passw = CreateHashString("123"),
                },
            };

            var token1 = ValidationToken.GenerateToken();
            token1.Id = 1;
            token1.UserId = 1;
            token1.CreatedAt = DateTime.UtcNow;
            token1.User = Users.First(f => f.Id.Equals(1));

            Tokens = new List<ValidationToken>()
            {
                token1
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            Mapper = config.CreateMapper();
        }
    }
}
