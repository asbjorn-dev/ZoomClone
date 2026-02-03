using MediatR;
using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Server.Features.MeetingFeature.GetRecentMeetings.Query
{
    public record GetRecentMeetingsQuery(string HostId) : IRequest<GetRecentMeetingsResponse>;
}
