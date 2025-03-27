using DocECMWebHooksIntegration.Requests;

namespace DocECMWebHooksIntegration.DTO
{
    public class WebHookNotification
    {
        public int WebhookId { get; set; }
        public EventType EventType { get; set; }
        public DocumentContent DocumentContent { get; set; }

    }
    public class DocumentContent
    {
        public List<FieldForDocumentEventDTO> Fields { get; set; }

        public int ObjectID { get; set; }
        public int ContentTypeId { get; set; }
        public string ContentType { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsLastItem { get; set; }
        public string Extension { get; set; }
        public bool IsDigitallySigned { get; set; }
        public bool IsLastVersion { get; set; }
        public string Distributor { get; set; }
        public int TotalComments { get; set; }
        public int TotalAttachments { get; set; }
        public int TotalAlerts { get; set; }
        public DateTimeOffset? OriginalCreationDate { get; set; }
        public int ObjectOriginalID { get; set; }
        public bool IsInWorkflow { get; set; }
        public bool IsDistributed { get; set; }
        public bool IsPendingConfirmation { get; set; }
        public bool HasPlugins { get; set; }
        public short MajVersion { get; set; }
        public short MinVersion { get; set; }
        public int AuthorID { get; set; }
    }
    public class FieldForDocumentEventDTO
    {
        public string FieldCode { get; set; }
        public string Value { get; set; }
        public string ValueStr { get; set; }
        public decimal? ValueNum { get; set; }
        public DateTime? ValueDate { get; set; }
    }
}
