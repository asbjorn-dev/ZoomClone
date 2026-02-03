using ZoomCloneApp.Shared.Meeting.Requests;
using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Client.Interfaces
{
    public interface IMeetingService
    {
        Task<CreateMeetingResponse?> CreateMeeting(CreateMeetingRequest meeting);
        Task<GetMeetingsResponse?> GetMeetings(string hostId);
        Task<GetRecentMeetingsResponse?> GetRecentMeetings(string hostId);
    }
}
