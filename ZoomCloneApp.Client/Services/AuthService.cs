using System.Net.Http.Json;
using NetcodeHub.Packages.Extensions.LocalStorage;
using ZoomCloneApp.Client.Interfaces;
using ZoomCloneApp.Shared.Authentication.Requests;
using ZoomCloneApp.Shared.Authentication.Responses;

namespace ZoomCloneApp.Client.Services
{
    public class AuthService(IHttpExtension httpExtension) : IAuthService
    {
        public async Task<LoginUserReponse?> Login(LoginUserRequest user)
        {
            try
            {
                var result = await httpExtension.GetPublicClient().PostAsJsonAsync("auth/login", user);

                // takes clean C# object data out from HTTP protocol response
                return await result.Content.ReadFromJsonAsync<LoginUserReponse>();
            }
            catch
            {
                return new LoginUserReponse(null!)
                {
                    IsSuccess = false,
                    Message = "An error occurred while logging in."
                };
            }
        }

        public async Task<CreateUserResponse?> CreateUserAccount(CreateUserRequest user)
        {
            try
            {
                var result = await httpExtension.GetPublicClient().PostAsJsonAsync("auth/create", user);

                // takes clean C# object data out from HTTP protocol response
                return await result.Content.ReadFromJsonAsync<CreateUserResponse>();
            }
            catch
            {
                return new CreateUserResponse
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating the account."
                };
            }
        }
    }
}
