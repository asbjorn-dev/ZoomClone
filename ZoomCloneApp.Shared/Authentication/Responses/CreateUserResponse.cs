using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomCloneApp.Shared.Authentication.Responses
{
    // Response from user creation containing the new user's ID
    public record CreateUserResponse : ServiceResponse<string>;
}
