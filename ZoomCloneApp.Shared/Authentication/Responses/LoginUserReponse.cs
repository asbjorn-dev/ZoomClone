using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomCloneApp.Shared.Authentication.Responses
{
    // When users login they will receive a JWT token + success status + message
    public record LoginUserReponse(string JwtToken) : ServiceResponse<string>;
}
