using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;
using ZoomCloneApp.Server.Interfaces;
using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Server.Services
{
    public class TwilioService : ITwilioService
    {
        private readonly string _accountSid;
        private readonly string _apikey;
        private readonly string _apiSecret;
        private readonly string _authToken;

        public TwilioService(IConfiguration config)
        {
            _accountSid = config["Twilio:AccountSid"]!;
            _apikey = config["Twilio:ApiKey"]!;
            _apiSecret = config["Twilio:ApiSecret"]!;
            _authToken = config["Twilio:AuthToken"]!;

            // Initialize Twilio client with API credentials for authentication 
            TwilioClient.Init(_accountSid, _authToken);
        }

        public TwilioServiceResponse CreateRoom(string roomName)
        {
            // Create a new video room with specified parameters using Twilio's Video API
            var response = RoomResource.Create(
                type: RoomResource.RoomTypeEnum.Group,
                uniqueName: roomName,
                maxParticipants: 10,
                recordParticipantsOnConnect: false);

            if (response.Sid != null)
            {
                return new TwilioServiceResponse
                {
                    IsSuccess = true,
                    Message = $"Room created successfully: {response.Status}"
                };
            }
            else
            {
                return new TwilioServiceResponse
                {
                    IsSuccess = false,
                    Message = "Failed to create room."
                };
            }
        }

        // Generates a Twilio access token for a user to join a specific meeting (room)
        public TwilioServiceResponse GenerateMeetingToken(string username, string meetingId)
        {
            // Define the video grant for the token, specifying the room (meeting) the user will join
            var grants = new HashSet<IGrant>
            {
                new VideoGrant
                {
                    Room = meetingId
                }
            };

            // Create the token with the account SID, API key, API secret, username, and the defined grants
            var token = new Token(
                _accountSid,
                _apikey,
                _apiSecret,
                username,
                grants: grants
            );

            // Return the generated token as a successful response
            return new TwilioServiceResponse
            {
                IsSuccess = true,
                Message = "Token generated successfully.",
                Data = token.ToJwt()
            };
        }
    }
}
