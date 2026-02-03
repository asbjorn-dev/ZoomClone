using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomCloneApp.Shared.Meeting.Responses
{
    // IsSuccess - whether token generation succeeded
    // Message - error message or success info
    // Data - the actual Twilio token string (if successful)
    public record TwilioServiceResponse : ServiceResponse<string>;
}
