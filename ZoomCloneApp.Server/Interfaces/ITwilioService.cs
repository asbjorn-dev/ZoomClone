using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Server.Interfaces
{
    public interface ITwilioService
    {
        TwilioServiceResponse GenerateMeetingToken(string username, string meetingId);
        TwilioServiceResponse CreateRoom(string roomName);
    }
}
