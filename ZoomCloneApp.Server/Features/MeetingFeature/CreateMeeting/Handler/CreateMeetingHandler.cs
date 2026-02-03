using MediatR;
using System.Security.Cryptography;
using ZoomCloneApp.Server.Data;
using ZoomCloneApp.Server.Features.MeetingFeature.CreateMeeting.Command;
using ZoomCloneApp.Shared.Meeting.Responses;
using ZoomCloneApp.Server.Models;
using ZoomCloneApp.Server.Interfaces;

namespace ZoomCloneApp.Server.Features.MeetingFeature.CreateMeeting.Handler
{
    public class CreateMeetingHandler(AppDbContext _dbContext, IConfiguration _config, ITwilioService twilioService) : IRequestHandler<CreateMeetingCommand, CreateMeetingResponse>
    {
        private readonly AppDbContext _dbContext = _dbContext;
        private readonly IConfiguration _config = _config;
        private readonly ITwilioService twilioService = twilioService;

        public async Task<CreateMeetingResponse> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
        {

            // checks if the request data is valid
            if (request.CreateMeeting == null)
            {
                return new CreateMeetingResponse
                {
                    IsSuccess = false,
                    Message = "Invalid meeting data."
                };
            }
            if (DateTimeOffset.Parse(request.CreateMeeting.StartDateOnly) < DateTimeOffset.UtcNow)
            {
                return new CreateMeetingResponse
                {
                    IsSuccess = false,
                    Message = "Meeting start time must be in the future."
                };
            }
            // enddate can not be before the startdate
            if (DateTimeOffset.Parse(request.CreateMeeting.EndDateOnly) <= DateTimeOffset.Parse(request.CreateMeeting.StartDateOnly))
            {
                return new CreateMeetingResponse
                {
                    IsSuccess = false,
                    Message = "Meeting end time must be after start time."
                };
            }

            // generates unique meeting ID and passcode
            string meetingId = GenerateMeetingId();

            // create the twilio meeting
            TwilioServiceResponse twilioResponse = twilioService.CreateRoom(meetingId);
            if (!twilioResponse.IsSuccess)
            {
                return new CreateMeetingResponse
                {
                    IsSuccess = twilioResponse.IsSuccess,
                    Message = twilioResponse.Message
                };
            }

            var meeting = new Meeting
            {
                MeetingId = meetingId,
                HostId = request.CreateMeeting.HostId,
                Title = request.CreateMeeting.Title,
                Description = request.CreateMeeting.Description,
                StartDateOnly = request.CreateMeeting.StartDateOnly,
                EndDateOnly = request.CreateMeeting.EndDateOnly,
                StartTimeOnly = request.CreateMeeting.StartTimeOnly,
                EndTimeOnly = request.CreateMeeting.EndTimeOnly,
                Passcode = GenerateMeetingPasscode()
            };

            // add to database
            meeting.Link = GenerateLink(meeting.MeetingId, meeting.Passcode);
            _dbContext.Meetings.Add(meeting);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateMeetingResponse
            {
                IsSuccess = true,
                Message = "Meeting created successfully."
            };
        }

        // Generates a unique meeting ID using a GUID and removes unwanted characters 
        private static string GenerateMeetingId()
        {
            // generates a new GUID (guranteed uniqueness) and converts it to a byte array (16 bytes) and then base64 string
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("=", string.Empty) // removes padding
                .Replace("+", string.Empty) // removes plus signs
                .Replace("/", string.Empty); // removes slashes
        }

        // Generates a joinable link for the meeting using base address, meeting ID, and passcode
        // ex. : "https://.../join-meeting/3F2504E04F89/A1B2154523"
        private string GenerateLink(string meetingId, string passCode)
        {
            return $"{_config["Client:BaseAddress"]}/join-meeting/{meetingId}/{passCode}";
        }

        // Generates a unique passcode combining random characters and a timestamp
        // ex, : "A1B2" - random characters and "154523" - timestamp (15:45:23 UTC) = "A1B2154523"
        private static string GenerateMeetingPasscode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4]; // generates 4 random bytes
            rng.GetBytes(bytes); // fills the byte array with random bytes (0-255)

            var randomPart = new char[4]; // array with random 4 characters for the passcode
            // Converts each random byte to a character from const chars
            for (int i = 0; i < randomPart.Length; i++)
            {
                randomPart[i] = chars[bytes[i] % chars.Length];
            }

            string timestamp = DateTime.UtcNow.ToString("HHmmss");

            return $"{new string(randomPart)}{timestamp}";
        }
    }
}
