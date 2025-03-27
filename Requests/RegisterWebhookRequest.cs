namespace DocECMWebHooksIntegration.Requests
{
    public class RegisterWebhookRequest
    {
        public int ContentTypeId { get; set; }
        public string NotificationUrl { get; set; }
        public string Condition { get; set; }
        public EventType EventType { get; set; }
        public DateTimeOffset? ExpirationDate { get; set; }

    }
    public enum EventType
    {
        NewDocument = 0,
        UpdatedDocument = 1,
        UpdatedCT = 2,
    }
    public enum WebHooksStatus
    {
        Active = 0,
        Inactive = 1,
    }
}
