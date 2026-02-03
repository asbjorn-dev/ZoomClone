using System.Security.Claims;
using ZoomCloneApp.Server.Interfaces;

namespace ZoomCloneApp.Server.Helpers
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken(List<Claim> claims)
        {
            throw new NotImplementedException();
        }
    }
}
