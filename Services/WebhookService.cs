using DocECMWebHooksIntegration.Requests;
using DocECMWebHooksIntegration.Responses;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace DocECMWebHooksIntegration.Services
{
    public class WebhookService
    {
        private readonly DocECMApiService _docECMApiService;
        private static string signKey = string.Empty;
        private static Dictionary<int, string> RegisteredSignKeys = new Dictionary<int, string>();
        private List<RegisterWebhookRequest> registerWebhookRequests = new List<RegisterWebhookRequest>();
        public WebhookService(DocECMApiService apiService)
        {
            _docECMApiService = apiService;
        }

        public bool RegisterWebHook(RegisterWebhookRequest registerWebhookRequest)
        {
            string jsonBody = JsonConvert.SerializeObject(registerWebhookRequest);
            var response = _docECMApiService.ExecuteDocECMApiRequest<CreateWebhookSubscriptionResponse>("web-hook", Method.Post, jsonBody);
            RegisteredSignKeys.Add(response.WebHookId, response.SignKey);
            return true;
        }
        public bool UpdateWebHook(UpdateWebhookRequest updateWebhookRequest)
        {
            string jsonBody = JsonConvert.SerializeObject(updateWebhookRequest);
            var response = _docECMApiService.ExecuteDocECMApiRequest<CreateWebhookSubscriptionResponse>("web-hook", Method.Put, jsonBody);
            return true;
        }
        public bool DeleteWebhook(int webhookId)
        {
            var response = _docECMApiService.ExecuteDocECMApiRequest($"web-hook/{webhookId}", Method.Delete);
            return response;
        }
        public GetWebhooksResponse GetWebhookByContentTypeAndEvent(int contentTypeId, EventType eventType)
        {
            var url = $"web-hook?contentTypeId={contentTypeId}&eventType={eventType}";

            var response = _docECMApiService.ExecuteDocECMApiRequest<GetWebhooksResponse>(url, Method.Get);
            return response;
        }

        public string GetSignKey(int webHookId)
        {
            return RegisteredSignKeys[webHookId];
        }
        public bool ValidateSignature(string body, string signature, string signKey)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(signKey);
            var bodyBytes = Encoding.UTF8.GetBytes(body);

            using (var hmac = new HMACSHA256(secretKeyBytes))
            {
                var hash = hmac.ComputeHash(bodyBytes);
                var computedSignature = Convert.ToBase64String(hash);

                return signature == computedSignature;
            }
        }
        public void RegisterDeffaultWebHooks()
        {
            var defaultTestWebhook = new RegisterWebhookRequest
            {
                Condition = "_author_|s04|%MYSELF_ID%|string",
                ContentTypeId = 5058,
                EventType = EventType.UpdatedDocument,
                NotificationUrl = "https://localhost:7298/api/web-hook-listener/notification",
            };
            RegisterWebHook(defaultTestWebhook);
        }
    }
}
