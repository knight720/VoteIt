using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoteIt.Services;

namespace VoteIt.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly NotifyService _notifyService;

        public NotifyController(NotifyService notifyService)
        {
            this._notifyService = notifyService;
        }

        [HttpGet("Send/{message}")]
        public async Task<IActionResult> Send(string message)
        {
            var response = await this._notifyService.Send(message);

            return Ok(response);
        }
    }
}