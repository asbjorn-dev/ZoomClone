using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomCloneApp.Shared.Meeting.Responses
{
    // Response containing a collection of meetings for a host
    public record CreateMeetingResponse : ServiceResponse<string>;
}
