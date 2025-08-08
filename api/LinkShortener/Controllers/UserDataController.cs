using LinkShortener.Constants;
using LinkShortener.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LinkShortener.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace LinkShortener.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        RepositoryUsers _repository;

        IConfiguration _configuration;

        public UserDataController(RepositoryUsers repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        private string Generate(string id, string role, string login)
        {
            var claims = new[]
            {
                new Claim("id", $"{id}"),
                new Claim(ClaimTypes.Role, $"{role}"),
                new Claim("login", $"{login}")
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var loginCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken
                (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: loginCredentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<List<User>> GetData()
        {
            return await _repository.GetUserData();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddData(   [FromForm] string login,
                                                    [FromForm] string password,
                                                    [FromForm] UserRole role)
        {
            if (!await _repository.TryAddUserData(login, password, role))
            {
                return Conflict("Login already exist");
            }

            return Ok();
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser( [FromForm] string login,
                                                    [FromForm] string password)
        {
            var user = await _repository.LoginUser(login, password);

            if (user == null)
            {
                return Unauthorized();
            }

            var id = user.UserId.ToString();
            var role = user.Role.ToString();

            var token = Generate(id, role, login);

            return Ok(new { token });
        }
    }
}