using DocECMWebHooksIntegration.DTO;

namespace DocECMWebHooksIntegration.Services
{
    public class MyBussinesService
    {
        private readonly DocECMApiService _docECMApiService;
        public MyBussinesService(DocECMApiService docECMApiService)
        {
            this._docECMApiService = docECMApiService;
        }
        public async Task ProcessWebhook(WebHookNotification notificationObject)
        {
            DocumentContent document = notificationObject.DocumentContent;
            //You can get the imputations of your document
            _docECMApiService.GetImputations(document.ObjectID, "Imputations");

            //Process the fields as you wish
            foreach (var field in document.Fields)
            {
            }
        }
    }
}
