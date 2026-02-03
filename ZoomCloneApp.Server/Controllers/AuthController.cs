using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ZoomCloneApp.Server.Features.Authentication.CreateUser.Command;
using ZoomCloneApp.Server.Features.Authentication.Login.Command;
using ZoomCloneApp.Shared.Authentication.Requests;
using ZoomCloneApp.Shared.Authentication.Responses;

namespace ZoomCloneApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<CreateUserResponse>> CreateUserAsync(CreateUserRequest user)
    {
        CreateUserResponse Response = await sender.Send(new CreateUserCommand(user));

        return Response.IsSuccess ? Ok(Response) : BadRequest(Response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserReponse>> LoginUserAsync(LoginUserRequest login)
    {
        LoginUserReponse Response = await sender.Send(new LoginUserCommand(login));

        return Response.IsSuccess ? Ok(Response) : BadRequest(Response);
    }
}
