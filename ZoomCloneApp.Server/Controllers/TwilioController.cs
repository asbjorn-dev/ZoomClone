using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZoomCloneApp.Server.Interfaces;
using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioController : ControllerBase
    {
        private readonly ITwilioService twilioService;

        public TwilioController(ITwilioService twilioService)
        {
            this.twilioService = twilioService;
        }

        [HttpGet("token/{username}/{meetingId}")]
        [AllowAnonymous]
        public ActionResult<TwilioServiceResponse> GenerateMeetingToken(string username, string meetingId)
        {
            TwilioServiceResponse response = twilioService.GenerateMeetingToken(username, meetingId);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
