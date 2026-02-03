using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomCloneApp.Shared.Meeting.Responses
{
    public record GetRecentMeetingsResponse : ServiceResponse<IEnumerable<GetMeeting>>;
}