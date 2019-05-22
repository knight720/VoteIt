using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace VoteIt.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvironmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EnvironmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> environment = new Dictionary<string, string>
            {
                {"Tag",  _configuration["Tag"]},
            };

            return new JsonResult(environment);
        }
    }
}