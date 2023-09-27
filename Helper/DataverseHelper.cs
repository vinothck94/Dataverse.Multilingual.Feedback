using Dataverse.Multilingual.Feedback.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Dataverse.Multilingual.Feedback.Helper
{
    public interface IDataverseHelper
    {
        Task<string> GetAccessToken(string resource, string clientId, string secret, string authority);
        Task<bool> UpdateField(string resource, string accessToken, string entityName, string recordId, FeedbackViewModel feedbackViewModel);
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

        public async Task<bool> UpdateField(string resource, string accessToken, string entityName, string recordId, FeedbackViewModel feedbackViewModel)
        {
            try
            {
                //var httpClient = new HttpClient();
                //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                //var response = await httpClient.GetAsync($"{resource}/api/data/v9.2/{entityName}(a466d72c-035d-ee11-be6f-000d3a4f73e8)");
                //string responseBody = await response.Content.ReadAsStringAsync();

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                string jsonPayload = $"{{\"crd69_comment\": \"{feedbackViewModel.Comment}\", \"crd69_rating\": \"{feedbackViewModel.Rating}\"}}";

                var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), resource + "/api/data/v9.2/" + entityName + "(" + recordId + ")")
                {
                    Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json"),
                };
                HttpResponseMessage response = await httpClient.SendAsync(patchRequest); if (response.IsSuccessStatusCode) ;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }



                //var url = resource + "/api/data/v9.2/" + entityName + "(" + recordId + ")";
                //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                //httpRequest.Method = "PATCH";
                //httpRequest.Headers["If-None-Match"] = "null";
                //httpRequest.Headers["OData-Version"] = "4.0";
                //httpRequest.Headers["OData-MaxVersion"] = "4.0";
                //httpRequest.ContentType = "application/json";
                //httpRequest.Accept = "application/json";
                //httpRequest.Headers["scope"] = $"{resource}/.default";
                //httpRequest.Headers["Authorization"] = "Bearer " + accessToken;

                //var data = new
                //{
                //    crd69_comment = feedbackViewModel.Comment,
                //    crd69_rating = feedbackViewModel.Rating,
                //};
                //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                //{
                //    streamWriter.Write(data);
                //}
                //var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //{
                //    var result = streamReader.ReadToEnd();
                //}

                //if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}
