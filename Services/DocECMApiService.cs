using DocECMWebHooksIntegration.DTO;
using Newtonsoft.Json;
using RestSharp;

namespace DocECMWebHooksIntegration.Services
{
    public class DocECMApiService
    {
        private string ApiURL;
        private string Username;
        private string Password;
        private string ApiToken = "";

        public DocECMApiService(IConfiguration configuration)
        {
            ApiURL = configuration["DocECMApiService:ApiURL"];
            Username = configuration["DocECMApiService:Username"];
            Password = configuration["DocECMApiService:Password"];
        }
        public void GetToken()
        {
            RestClient client = new RestClient($"{ApiURL}");
            RestRequest request = new RestRequest("/token", Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("application/x-www-form-urlencoded", $"username={Username}&password={Password}&grant_type=password", ParameterType.RequestBody);
            RestResponse<DocECMAPITokenResponse> response = client.Execute<DocECMAPITokenResponse>(request);
            if (response.IsSuccessful)
            {
                ApiToken = response.Data.access_token;
            }
            else
            {
                throw new Exception(response.Content);
            }
        }
        public T ExecuteDocECMApiRequest<T>(string url, Method method, string jsonBody = "", bool isFile = false)
        {
            RestClient client = new RestClient(ApiURL);
            RestRequest request = new RestRequest($"/api/{url}", method);
            request.AddHeader("cache-control", "no-cache");
            GetToken();
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", $"bearer {ApiToken}");
            if (!string.IsNullOrEmpty(jsonBody))
            {
                request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            }
            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                GetToken();
                return ExecuteDocECMApiRequest<T>(url, method, jsonBody);
            }
            else
            {
                throw new Exception(response.Content);
            }
        }
        public bool ExecuteDocECMApiRequest(string url, Method method, string jsonBody = "", bool isFile = false)
        {
            RestClient client = new RestClient(ApiURL);
            RestRequest request = new RestRequest($"/api/{url}", method);
            request.AddHeader("cache-control", "no-cache");
            GetToken();
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", $"bearer {ApiToken}");
            if (!string.IsNullOrEmpty(jsonBody))
            {
                request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
            }
            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                GetToken();
                return ExecuteDocECMApiRequest(url, method, jsonBody);
            }
            else
            {
                throw new Exception(response.Content);
            }
        }
        public List<ImputationDTO> GetImputations(int objectID, string dbTableName)
        {
            return ExecuteDocECMApiRequest<List<ImputationDTO>>($"plugin/get-imputations/{objectID}?dbTableName={dbTableName}", Method.Get);
        }
    }
}
