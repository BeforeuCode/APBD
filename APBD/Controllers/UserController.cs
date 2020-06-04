using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using cw3.DAL.DTO;
using cw3.DAL.Services;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserDbService _dbService;
        public IConfiguration Configuration { get; set; }

        public UserController(IUserDbService dbService, IConfiguration configuration)
        {
            _dbService = dbService;
            Configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto request)
        {
            return BadRequest();
        }

        [HttpPost("refrsh-token/{token}")]
        public IActionResult RefreshToken(string refToken)
        {
            return BadRequest();
        }

        [HttpPost("register")]
        public IActionResult Register(LoginDto request)
        {
         
            return BadRequest();
        }
    }
}