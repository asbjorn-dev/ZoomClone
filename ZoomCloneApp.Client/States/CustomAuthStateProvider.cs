using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using NetcodeHub.Packages.Extensions.LocalStorage;

namespace ZoomCloneApp.Client.States
{
    public class CustomAuthStateProvider(ILocalStorageService localStorageService, IConfiguration config) : AuthenticationStateProvider
    {
        // default unauthenticated user with no claims
        private ClaimsPrincipal User = new(new ClaimsIdentity());

        // gets the current authentication state from the local storage
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // gets the token key
            string key = config["Token:Key"]!;
            if (key == null)
                return await Task.FromResult(new AuthenticationState(User));

            //gets the token from browser local storage
            string token = await localStorageService.GetItemAsStringAsync(key);
            if (token == null)
                return await Task.FromResult(new AuthenticationState(User));

            // parse token and set claims
            User = SetClaim(token);
            return await Task.FromResult(new AuthenticationState(User));
        }

        // Sets the user as authenticated by storing the JWT token and notifying the app of authentication state change.
        public async Task SetUserAuthenticated(string token)
        {
            // gets token key from configuration
            string key = config["Token:Key"]!;
            if (config["Token:Key"] == null)
                return;

            // Save the JWT token to browser local storage
            await localStorageService.SaveAsStringAsync(key, token);
            User = SetClaim(token);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(User)));
        }

        // Logs out the user by removing the token and resetting authentication state
        public async Task SetUserLoggedOut()
        {
            string key = config["Token:Key"]!;
            if (key == null)
                return;

            // Remove the JWT token from browser local storage
            await localStorageService.DeleteItemAsync(key);
            User = new ClaimsPrincipal(new ClaimsIdentity()); // sets the user to default no claims
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(User)));
        }

        // Parses a JWT token and extracts user claims (name, email, roles, etc.).
        // Creates a ClaimsPrincipal that Blazor can use for authorization.
        private static ClaimsPrincipal SetClaim(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler(); // create JWT token handler to read and validate token
                var jwtToken = tokenHandler.ReadJwtToken(token); // read the token
                var claims = jwtToken.Claims; // extract all claims (user info) from token

                // create authenticated user with claims. "JwtAuth" is the authentication type identifier
                return new ClaimsPrincipal(new ClaimsIdentity(claims, "JwtAuth")); 
            }
            catch
            {
                // If token is invalid or expired, return unauthenticated user
                return new ClaimsPrincipal(new ClaimsIdentity());
            }
        
        }
    }
}
