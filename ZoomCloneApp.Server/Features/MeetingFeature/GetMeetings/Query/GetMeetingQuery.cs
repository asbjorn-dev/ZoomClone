using MediatR;
using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Server.Features.MeetingFeature.GetMeetings.Query
{
    public record GetMeetingQuery(string HostId) : IRequest<GetMeetingsResponse>;

}
