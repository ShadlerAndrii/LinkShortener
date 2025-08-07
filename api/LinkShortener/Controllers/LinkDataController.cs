using LinkShortener.Constants;
using LinkShortener.Data;
using LinkShortener.DTOs;
using LinkShortener.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LinkShortener.Controllers
{    
    [ApiController]
    public class LinkDataController : ControllerBase
    {
        RepositoryLinks _repository;

        IConfiguration _configuration;

        public LinkDataController(RepositoryLinks repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        private string Generate(int length)
        {
            const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var bytes = new byte[length];
            RandomNumberGenerator.Create().GetBytes(bytes);

            var code = new StringBuilder(length);
            foreach (var b in bytes)
            {
                code.Append(Alphabet[b % Alphabet.Length]);
            }

            return code.ToString();
        }

        [AllowAnonymous]
        [Route("")]
        [HttpGet]
        public async Task<List<UnAuthorizedLinkDTO>> GetUnAuthorizedData()
        {
            return await _repository.GetLinkData();
        }

        [Authorize]
        [Route("[controller]")]
        [HttpGet]
        public async Task<List<Link>> GetAuthorizedData()
        {
            int userId = int.Parse(User.FindFirst("id")!.Value);
            string role = User.FindFirst("role")!.Value;

            return await _repository.GetLinkData(userId, role);
        }

        [Authorize]
        [Route("[controller]")]
        [HttpPost]
        public async Task<string> AddData([FromForm] string longLink)
        {
            int userId = int.Parse(User.FindFirst("id")!.Value);
            string shortLink;
            string code;


            while (true)
            {
                code = Generate(8);
                shortLink = $"{Request.Scheme}://{Request.Host}/{code}";

                if (await _repository.TryAddLinkData(userId, longLink, shortLink))
                    break;
            }

            return shortLink;
        }

        [Authorize]
        [Route("[controller]/{linkId}")]
        [HttpDelete]
        public async Task<OkResult> DeleteData(int linkId)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value);
            var role = User.FindFirst("role")?.Value;

            await _repository.DeleteLinkData(linkId, userId, role);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{*code}")]
        public async Task<IActionResult> GetReversedData(string code)
        {
            string shortLink = $"{Request.Scheme}://{Request.Host}/{code}";

            string longLink = await _repository.GetLinkData(shortLink);

            if (string.IsNullOrEmpty(longLink))
                return NotFound("Link not found");

            return Redirect(longLink);
        }
    }
}