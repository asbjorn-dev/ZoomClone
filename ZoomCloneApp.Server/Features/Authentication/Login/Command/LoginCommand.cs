using MediatR;
using ZoomCloneApp.Shared.Authentication.Requests;
using ZoomCloneApp.Shared.Authentication.Responses;

namespace ZoomCloneApp.Server.Features.Authentication.Login.Command
{
    // Command to authenticate a user and generate JWT token
    public record LoginUserCommand(LoginUserRequest Login) : IRequest<LoginUserReponse>;
}