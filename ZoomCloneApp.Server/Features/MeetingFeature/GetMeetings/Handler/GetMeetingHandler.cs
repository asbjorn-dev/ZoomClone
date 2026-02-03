using MediatR;
using Microsoft.EntityFrameworkCore;
using ZoomCloneApp.Server.Data;
using ZoomCloneApp.Server.Features.MeetingFeature.GetMeetings.Query;
using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Server.Features.MeetingFeature.GetMeetings.Handler
{
    public class GetMeetingHandler(AppDbContext _appDbContext) : IRequestHandler<GetMeetingQuery, GetMeetingsResponse>
    {
        // Handler that retrieves all active meetings for a specific host from db 
        // returns a list of meetings as a DTO
        public async Task<GetMeetingsResponse> Handle(GetMeetingQuery request, CancellationToken cancellationToken)
        {
            // Retrieve all meetings from the database with query. 
            var meetings = await _appDbContext.Meetings.AsNoTracking()
                .Where(m => m.HostId == request.HostId && m.IsCompleted == false) // filter meetings by hostId and meetings that are not completed
                .ToListAsync(cancellationToken: cancellationToken);

            // return null if no meetings found
            if (meetings == null || meetings.Count == 0)
            {
                return new GetMeetingsResponse
                {
                    IsSuccess = false,
                    Message = "No meetings found for the specified host."
                };
            }

            // Map db entities to DTOs 
            var _meetings = meetings.Select(m => new GetMeeting
            {
                HostId = m.HostId,
                Title = m.Title,
                Description = m.Description,
                StartDateOnly = m.StartDateOnly,
                EndDateOnly = m.EndDateOnly,
                StartTimeOnly = m.StartTimeOnly,
                EndTimeOnly = m.EndTimeOnly,
                Id = m.Id,
                Passcode = m.Passcode,
                MeetingId = m.MeetingId,
                Link = m.Link,
            }).ToList();

            // return successful response with meetings DTO to client
            return new GetMeetingsResponse
            {
                IsSuccess = true,
                Message = $"{_meetings.Count} meetings found",
                Data = _meetings
            };
        }
    }
}
