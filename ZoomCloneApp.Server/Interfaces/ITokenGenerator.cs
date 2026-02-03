using System.Security.Claims;

namespace ZoomCloneApp.Server.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(List<Claim> claims);
    }
}
