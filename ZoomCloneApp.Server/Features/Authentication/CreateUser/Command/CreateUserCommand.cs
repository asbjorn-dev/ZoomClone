using MediatR;
using ZoomCloneApp.Shared.Authentication.Requests;
using ZoomCloneApp.Shared.Authentication.Responses;

namespace ZoomCloneApp.Server.Features.Authentication.CreateUser.Command
{
    /// Command to create a new user account on the server
    public record CreateUserCommand(CreateUserRequest User) : IRequest<CreateUserResponse>;
}