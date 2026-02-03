using System.ComponentModel;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using ZoomCloneApp.Client.Interfaces;
using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Client.Services
{
    public class TwilioService(IJSRuntime _js, IHttpExtension httpExtension) : ITwilioService
    {
        public async Task<TwilioServiceResponse?> GenerateMeetingToken(string username, string meetingId)
        {
            // sends GET request to Twilio API to generate a meeting token for a specific user and meeting (no authentication required)
            var result = await httpExtension.GetPublicClient().GetAsync($"twilio/token/{username}/{meetingId}");
            var response = await result.Content.ReadFromJsonAsync<TwilioServiceResponse>();

            // returns a temporary Twilio access token for the specific user to join the specific meeting
            return response;
        }

        public async Task JoinMeeting(string token, string roomName, string containerId)
        {
            // calls a js function with 4 paramters to connect to a twilio video room
            // optionally pass a flag for guests to use fake video
            await _js.InvokeVoidAsync("window.twilioVideo.connectToRoom", token, roomName, containerId, true);
        }
    }
}
