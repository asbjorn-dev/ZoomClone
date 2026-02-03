using System.Net.Http.Json;
using ZoomCloneApp.Client.Interfaces;
using ZoomCloneApp.Shared.Meeting.Requests;
using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Client.Services
{
    public class MeetingService(IHttpExtension httpExtension) : IMeetingService
    {
        // creates a post request to create a meeting to the meeting API
        public async Task<CreateMeetingResponse?> CreateMeeting(CreateMeetingRequest meeting)
        {
            try
            {
                // send post request with meeting data as json
                var result = await (await httpExtension.GetPrivateClient()).PostAsJsonAsync("meeting/create", meeting);
                // Deserialize the response into a CreateMeetingResponse object
                var response = await result.Content.ReadFromJsonAsync<CreateMeetingResponse>();

                return response;
            }
            catch
            {
                return new CreateMeetingResponse
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the meeting."
                };
            }
        }
        

        // Retrieves all meetings (List) for a specific host
        public async Task<GetMeetingsResponse?> GetMeetings(string hostId)
        {
            try
            {
                // send GET request to meeting API with hostId
                var result = await (await httpExtension.GetPrivateClient()).GetAsync($"meeting/host/{hostId}");
                // deserialize the response into a GetMeetingsResponse object
                var response = await result.Content.ReadFromJsonAsync<GetMeetingsResponse>();

                return response;
            }
            catch
            {
                return new GetMeetingsResponse
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving meetings."
                };
            }
        }

        // Retrieves recent meetings for a specific host
        public async Task<GetRecentMeetingsResponse?> GetRecentMeetings(string hostId)
        {
            try
            {
                // Sends GET request to receive alle recent meetings for a specific host
                var result = await (await httpExtension.GetPrivateClient()).GetAsync($"meeting/recent/{hostId}");
                var response = await result.Content.ReadFromJsonAsync<GetRecentMeetingsResponse>();

                return response;
            }
            catch
            {
                return new GetRecentMeetingsResponse
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving recent meetings."
                };
            }
        }
    }
}
