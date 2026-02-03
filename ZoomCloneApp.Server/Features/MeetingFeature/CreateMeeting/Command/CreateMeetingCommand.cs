using MediatR;
using ZoomCloneApp.Shared.Meeting.Requests;
using ZoomCloneApp.Shared.Meeting.Responses;


namespace ZoomCloneApp.Server.Features.MeetingFeature.CreateMeeting.Command
{
    public record CreateMeetingCommand(CreateMeetingRequest CreateMeeting) : IRequest<CreateMeetingResponse>;
}
