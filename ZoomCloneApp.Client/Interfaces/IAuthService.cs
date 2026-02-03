using ZoomCloneApp.Shared.Authentication.Requests;
using ZoomCloneApp.Shared.Authentication.Responses;

namespace ZoomCloneApp.Client.Interfaces
{
    public interface IAuthService
    {
        Task<LoginUserReponse?> Login(LoginUserRequest user);
        Task<CreateUserResponse?> CreateUserAccount(CreateUserRequest user);
    }
}
