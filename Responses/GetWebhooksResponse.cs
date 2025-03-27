using DocECMWebHooksIntegration.DTO;

namespace DocECMWebHooksIntegration.Responses
{
    public class GetWebhooksResponse
    {
        public List<WebhookDTO> Webhooks { get; set; } = new List<WebhookDTO>();
    }
}
