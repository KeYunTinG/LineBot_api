using LineBot_api.Models;

namespace LineBot_api.Service.Interface
{
    public interface IMessageService
    {
        MidReturn seed(string message);
        MidReturn ReceiveWebhook(WebhookRequestBodyDto requestBody);
        MidReturn BroadcastMessageHandler(string messageType, object requestBody);
        Task<MidReturn> BroadcastMessage<T>(BroadcastMessageRequestDto<T> request);
        MidReturn ReplyMessageHandler<T>(string messageType, ReplyMessageRequestDto<T> requestBody);
        Task<MidReturn> ReplyMessage<T>(ReplyMessageRequestDto<T> request);
    }
}
