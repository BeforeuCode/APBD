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
            User user = _dbService.GetUserCredentials(request.Username);

            if (user != null)
            {
                var userValidated = _dbService.Validate(request.Password, user.Salt, user.HashedPassword);
                if (userValidated)
                {
                    var claims = new[]
                    {
                     new Claim(ClaimTypes.Name, user.Username),
                     new Claim(ClaimTypes.Role, user.Role),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken
                    (
                        issuer: "Gakko",
                        audience: "Students",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: creds
                    );

                    return Ok(new
                    {
                        accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        refreshToken = Guid.NewGuid()
                    });
                }else
                {
                    return BadRequest();
                }

            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("refrsh-token/{token}")]
        public IActionResult RefreshToken(string refToken)
        {
            //check refresh token in db 
            User user = _dbService.GetUserByRefreshToken(refToken);

            if (user != null)
            {
       
               
                    var claims = new[]
                    {
                     new Claim(ClaimTypes.Name, user.Username),
                     new Claim(ClaimTypes.Role, user.Role),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken
                    (
                        issuer: "Gakko",
                        audience: "Students",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: creds
                    );

               

                    return Ok(new
                    {
                        accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        refreshToken = Guid.NewGuid()
                    }
                    );
          
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("register")]
        public IActionResult Register(LoginDto request)
        {
            _dbService.AddUser(request);

            return Ok();
        }
    }
}