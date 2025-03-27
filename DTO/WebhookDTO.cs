using DocECMWebHooksIntegration.Requests;

namespace DocECMWebHooksIntegration.DTO
{
    public class WebhookDTO
    {
        public int Id { get; set; }
        public string NotificationUrl { get; set; }
        public string Condition { get; set; }
        public EventType EventType { get; set; }
        public WebHooksStatus Status { get; set; }
    }
}
