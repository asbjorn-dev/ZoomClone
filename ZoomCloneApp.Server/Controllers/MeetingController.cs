using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZoomCloneApp.Server.Features.MeetingFeature.CreateMeeting.Command;
using ZoomCloneApp.Server.Features.MeetingFeature.GetMeetings.Query;
using ZoomCloneApp.Server.Features.MeetingFeature.GetRecentMeetings.Query;
using ZoomCloneApp.Shared.Meeting.Requests;
using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController(ISender sender) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<ActionResult<CreateMeetingResponse>> CreateMeetingAsync(CreateMeetingRequest meeting)
        {
            CreateMeetingResponse Response = await sender.Send(new CreateMeetingCommand(meeting));

            return Response.IsSuccess ? Ok(Response) : BadRequest(Response);
        }

        [HttpGet("host/{meetingId}")]
        [Authorize]
        public async Task<ActionResult<GetMeetingsResponse>> GetMeetingByIdAsync(string meetingId)
        {
            GetMeetingsResponse Response = await sender.Send(new GetMeetingQuery(new(meetingId)));

            return Response.IsSuccess ? Ok(Response) : BadRequest(Response);
        }

        [HttpGet("recent/{meetingId}")]
        [Authorize]
        public async Task<ActionResult<GetRecentMeetingsResponse>> GetRecentMeetingsAsync(string meetingId)
        {
            GetRecentMeetingsResponse Response = await sender.Send(new GetRecentMeetingsQuery(new(meetingId)));

            return Response.IsSuccess ? Ok(Response) : BadRequest(Response);
        }
    }
}
