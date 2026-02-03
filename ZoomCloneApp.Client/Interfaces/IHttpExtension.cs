namespace ZoomCloneApp.Client.Interfaces
{
    public interface IHttpExtension
    {
        HttpClient GetPublicClient();
        Task<HttpClient> GetPrivateClient();
    }
}
