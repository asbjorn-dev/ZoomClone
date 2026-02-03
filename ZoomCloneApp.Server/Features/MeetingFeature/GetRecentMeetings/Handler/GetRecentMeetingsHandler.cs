using MediatR;
using Microsoft.EntityFrameworkCore;
using ZoomCloneApp.Server.Data;
using ZoomCloneApp.Server.Features.MeetingFeature.GetRecentMeetings.Query;
using ZoomCloneApp.Shared.Meeting.Responses;

namespace ZoomCloneApp.Server.Features.MeetingFeature.GetRecentMeetings.Handler
{
    public class GetRecentMeetingsHandler(AppDbContext _appDbContext) : IRequestHandler<GetRecentMeetingsQuery, GetRecentMeetingsResponse>
    {
        // Handler that retrieves all completed meetings for a specific host from db
        // this shows past meetings that have already ended
        public async Task<GetRecentMeetingsResponse> Handle(GetRecentMeetingsQuery request, CancellationToken cancellationToken)
        {
            // query to db for completed Meetings
            var meetings = await _appDbContext.Meetings.AsNoTracking()
                .Where(m => m.HostId == request.HostId && m.IsCompleted)
                .ToListAsync(cancellationToken: cancellationToken);

            // check if any completed meetings exist
            if (meetings == null || meetings.Count == 0)
            {
                return new GetRecentMeetingsResponse
                {
                    IsSuccess = false,
                    Message = "No recent meetings found for the specified host."
                };
            }

            // transform db entities to DTOs
            var recentMeetings = meetings.Select(m => new GetMeeting
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

            // return successful response with recent meetings DTO to client
            return new GetRecentMeetingsResponse
            {
                IsSuccess = true,
                Message = $"{recentMeetings.Count} recent meetings found",
                Data = recentMeetings
            };
        }
    }
}
