using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JuntoSeg.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JuntoSeg.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(
            IUserService service)
        {
            _service = service;
        }
    }
}