using MediatR;
using Microsoft.AspNetCore.Identity;
using ZoomCloneApp.Server.Features.Authentication.CreateUser.Command;
using ZoomCloneApp.Server.Models;
using ZoomCloneApp.Shared.Authentication.Responses;

namespace ZoomCloneApp.Server.Features.Authentication.CreateUser.Handler
{
    /// Handles user creation with password hashing and Identity integration
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateUserHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate request
            ArgumentNullException.ThrowIfNull(request.User);

            // Create ApplicationUser from request
            var user = new ApplicationUser
            {
                UserName = request.User.Name,
                Email = request.User.Email,
                Name = request.User.Name
            };

            // UserManager automatically hashes the password
            var result = await _userManager.CreateAsync(user, request.User.Password);

            if (result.Succeeded)
            {
                return new CreateUserResponse
                {
                    IsSuccess = true,
                    Message = "User created successfully",
                    Data = user.Id 
                };
            }

            // Return errors if creation failed
            return new CreateUserResponse
            {
                IsSuccess = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }
    }
}