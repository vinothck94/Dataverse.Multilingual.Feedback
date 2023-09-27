using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Dataverse.Multilingual.Feedback.Helper
{
    public interface IDataverseHelper
    {
        Task<string> GetAccessToken(string resource, string clientId, string secret, string authority);
    }
    public class DataverseHelper : IDataverseHelper
    {
        public async Task<string> GetAccessToken(string resource, string clientId, string secret, string authority)
        {
            AuthenticationContext authContext = new AuthenticationContext(authority);
            ClientCredential credential = new ClientCredential(clientId, secret);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, credential);
            return result.AccessToken;
        }
    }
}
