using LineBot_api.Enum;

namespace LineBot_api.Models
{
    public class Messages
    {
    }
    public class BroadcastMessageRequestDto<T>
    {
        public List<T> Messages { get; set; }
        public bool? NotificationDisabled { get; set; }
    }
    public class ReplyMessageRequestDto<T>
    {
        public string ReplyToken { get; set; }
        public List<T> Messages { get; set; }
        public bool? NotificationDisabled { get; set; }
    }
    public class BaseMessageDto
    {
        public string Type { get; set; }
    }
    public class TextMessageDto : BaseMessageDto
    {
        public TextMessageDto()
        {
            Type = MessageTypeEnum.Text;
        }

        public string Text { get; set; }
    }
}
