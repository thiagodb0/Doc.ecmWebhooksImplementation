namespace DocECMWebHooksIntegration.Requests
{
    public class NotificationPayload
    {
       public int ObjectId {  get; set; }
       public int userId {  get; set; }
       public EventType EventType { get; set; }
    }
}
