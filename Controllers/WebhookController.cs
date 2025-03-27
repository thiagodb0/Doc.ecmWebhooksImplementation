using DocECMWebHooksIntegration.DTO;
using DocECMWebHooksIntegration.Requests;
using DocECMWebHooksIntegration.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace DocECMWebHooksIntegration.Controllers
{
    [ApiController]
    [Route("api/web-hook-listener")]
    public class WebhookController : Controller
    {
        private readonly WebhookService _webhookService;
        private readonly MyBussinesService _bussinesService;
        private const string SignatureHeaderName = "X-Signature";

        public WebhookController(WebhookService webhookService, MyBussinesService bussinesService)
        {
            _webhookService = webhookService;
            _bussinesService = bussinesService;
        }

        [HttpPost]
        [Route("notification")]
        public async Task<IActionResult> ReceiveWebhookNotification()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            if (!Request.Headers.TryGetValue("X-Signature", out var signatureHeader))
            {
                return BadRequest("Origin not recognized.");
            }

            var signature = signatureHeader.ToString();
            if(signature == string.Empty)
            {
                return Ok("Web hook validated");
            }

            var notification = JsonConvert.DeserializeObject<WebHookNotification>(body);

            var signKey = _webhookService.GetSignKey(notification.WebhookId);

            if (signKey == null)
            {
                return BadRequest("Sign key for webhook was not found.");
            }

            if (!_webhookService.ValidateSignature(body, signature, signKey))
            {
                return BadRequest("Invalid signature.");
            }
            else
            {
                await _bussinesService.ProcessWebhook(notification);
            }

            return Ok("Notificación procesada correctamente.");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterWebhook([FromBody] RegisterWebhookRequest registerWebhookRequest)
        {
            return Ok(_webhookService.RegisterWebHook(registerWebhookRequest));
        }

        [HttpGet]
        [Route("get-webhook")]
        public async Task<IActionResult> GetByContentTypeAndEventType([FromQuery] int contentTypeId, [FromQuery] EventType eventType)
        {
            var result =  _webhookService.GetWebhookByContentTypeAndEvent(contentTypeId, eventType);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateWebHook([FromBody] UpdateWebhookRequest updateWebhookRequest)
        {
            _webhookService.UpdateWebHook(updateWebhookRequest);
            return Ok();
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
             _webhookService.DeleteWebhook(id);
            return Ok();
        }
    }
}
