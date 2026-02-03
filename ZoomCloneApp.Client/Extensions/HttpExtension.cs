using System.Net.Http.Headers;
using NetcodeHub.Packages.Extensions.LocalStorage;
using ZoomCloneApp.Client.Interfaces;

namespace ZoomCloneApp.Client.Extensions
{
    public class HttpExtension(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService, IConfiguration config) : IHttpExtension
    {
        // Extension class to handle HTTP client and authentication in a clean resuable way so no need to repeat token-handling logic everywhere.
        // GetPublicClient() is for endpoints that do not require authentication.
        // GetPrivateClient() is for endpoints that require authentication with JWT token. 
        HttpClient CreateClient() => httpClientFactory.CreateClient(config["HttpClient:Name"]!);
        public HttpClient GetPublicClient()
        {
            return CreateClient();
        }

        // Automatically retrives token from local storage and adds it to the request headers.
        // if no token is found, it returns a regular client without authentication headers (moves authentication to the API instead)
        public async Task<HttpClient> GetPrivateClient()
        {
            var client = CreateClient();

            // Get token storage key from configuration
            string? key = config["Token:Key"];
            if (string.IsNullOrEmpty(key))
                return client;

            // Retrieve JWT token from local storage
            string? token = await localStorageService.GetItemAsStringAsync(key);

            // Add Bearer token to Authorization header if token exists
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}
