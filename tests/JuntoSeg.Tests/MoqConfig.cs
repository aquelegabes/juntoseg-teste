using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extras.Moq;
using JuntoSeg.Domain.Entities;
using Moq;
using static JuntoSeg.Common.HashString;

namespace JuntoSeg.Tests
{
    public abstract class MoqConfig
    {
        protected ICollection<User> users { get; }

        public MoqConfig()
        { 
            // users
            users = new List<User>()
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
        }
    }
}
