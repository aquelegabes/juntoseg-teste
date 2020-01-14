using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JuntoSeg.Application.Interfaces;
using JuntoSeg.Application.ViewModel.Requests;
using JuntoSeg.Application.ViewModel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JuntoSeg.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public UserController(
            IUserService service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResp), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 0)
                return BadRequest();
            try
            {
                var user = await _service.FindAsync(id);
                if (user == null)
                    return NoContent();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorMessage = ex.Message,
                });
            }
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(UserResp), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add(
            [FromBody] UserReq req)
        {
            if (string.IsNullOrWhiteSpace(req.Email)
                || string.IsNullOrWhiteSpace(req.NotHashedPassword)
                || string.IsNullOrWhiteSpace(req.Name))
                return BadRequest();

            try
            {
                var response = _service.Add(req);
                return Created("", response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorMessage = ex.Message,
                });
            }
        }

        [HttpPut("")]
        [ProducesResponseType(typeof(UserResp), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(
            [FromBody] UserReq req)
        {
            if (string.IsNullOrWhiteSpace(req.Email)
               || string.IsNullOrWhiteSpace(req.Name))
                return BadRequest();
            try
            {
                var response = await _service.UpdateAsync(req);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorMessage = ex.Message,
                });
            }
        }

        [HttpDelete("")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(
            [FromBody] UserReq req)
        {
            if (req.Id <= 0)
                return BadRequest();
            try
            {
                return Ok(await _service.RemoveAsync(req));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorMessage = ex.Message,
                });
            }
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status208AlreadyReported)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ResetPassword(
            [FromHeader] string token, [FromHeader] string newPassword)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(newPassword))
                return BadRequest();
            
            try
            {
                if (_service.ResetPassword(token, newPassword))
                    return Ok();
                else
                    return StatusCode(208);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorMessage = ex.Message,
                });
            }
        }

        [HttpGet("generate-token")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateToken([FromHeader]int uid)
        {
            try
            {
                var token = await _service.GenerateValidationToken(uid);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorMessage = ex.Message,
                });
            }
        }
    }
}