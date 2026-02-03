using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomCloneApp.Shared.Meeting.Responses
{
    // gets a list of meetings
    public record GetMeetingsResponse : ServiceResponse<IEnumerable<GetMeeting>>;
}
