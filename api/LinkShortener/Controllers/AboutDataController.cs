using LinkShortener.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace LinkShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AboutDataController : ControllerBase
    {
        RepositoryAbouts _repository;

        public AboutDataController(RepositoryAbouts repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<string> GetData()
        {
            return await _repository.GetAboutData();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<OkResult> ChangeData([FromForm] string aboutText)
        {
            await _repository.ChangeAboutData(aboutText);

            return Ok();
        }
    }
}
