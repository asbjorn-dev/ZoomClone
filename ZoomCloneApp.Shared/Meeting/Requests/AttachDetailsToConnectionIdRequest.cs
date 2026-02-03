using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomCloneApp.Shared.Meeting.Requests
{
    public record AttachDetailsToConnectionIdRequest(string ConnectionId, string UserId, string Name);
}