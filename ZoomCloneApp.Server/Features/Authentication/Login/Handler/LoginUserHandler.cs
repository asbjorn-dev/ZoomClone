using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ZoomCloneApp.Server.Features.Authentication.Login.Command;
using ZoomCloneApp.Server.Models;
using ZoomCloneApp.Shared.Authentication.Responses;

namespace ZoomCloneApp.Server.Features.Authentication.Login.Handler
{
    /// <summary>
    /// Handles user login, validates credentials, and generates JWT token
    /// </summary>
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserReponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public LoginUserHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<LoginUserReponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Validate request
            ArgumentNullException.ThrowIfNull(request.Login);

            // Find user by email
            var user = await _userManager.FindByEmailAsync(request.Login.Email);
            if (user == null)
            {
                return new LoginUserReponse(null!)
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }

            // Verify password
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Login.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return new LoginUserReponse(null!)
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }

            // Generate JWT token
            var token = await GenerateJwtToken(user);

            return new LoginUserReponse(token)
            {
                IsSuccess = true,
                Message = "Login successful",
                Data = token
            };
        }

        /// Generates a JWT token for the authenticated user
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Get JWT settings from configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

            // Create token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}