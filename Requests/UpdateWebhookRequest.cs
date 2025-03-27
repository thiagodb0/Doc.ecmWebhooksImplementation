namespace DocECMWebHooksIntegration.Requests
{
    public class UpdateWebhookRequest
    {
        public int WebHookId { get; set; }
        public string? Condition { get; set; }
        public EventType? EventType { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }
    }
}
