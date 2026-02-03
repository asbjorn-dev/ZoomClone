using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Client.Interfaces
{
    public interface ITwilioService
    {
        Task<TwilioServiceResponse?> GenerateMeetingToken(string username, string meetingId);
        Task JoinMeeting(string token, string roomName, string containerId);

    }
}
