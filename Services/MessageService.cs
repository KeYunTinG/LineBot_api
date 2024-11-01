﻿using isRock.LineBot;
using LineBot_api.Enum;
using LineBot_api.Models;
using LineBot_api.Providers;
using LineBot_api.Service.Interface;
using System.Net.Http.Headers;
using System.Text;

namespace LineBot_api.Service
{
    public class MessageService(IConfiguration _configuration) : IMessageService
    {
        string MyUserID = _configuration["MyUserID"];
        string ChannelAccessToken = _configuration["ChannelAccessToken"];
        private readonly string replyMessageUri = "https://api.line.me/v2/bot/message/reply";
        private readonly string broadcastMessageUri = "https://api.line.me/v2/bot/message/broadcast";


        private static HttpClient client = new HttpClient();
        private readonly JsonProvider _jsonProvider = new JsonProvider();
        public MidReturn seed(string message)
        {
            Bot bot = new Bot(ChannelAccessToken);
            try
            {
                bot.PushMessage(MyUserID, message);
                var result = "success";
                return new MidReturn { msg = "0000" };
            }
            catch (Exception ex)
            {
                // 捕捉例外並記錄錯誤訊息
                Console.Error.WriteLine($"Error sending message: {ex.Message}");

                // 返回錯誤訊息
                return new MidReturn
                {
                    msg = "0001"
                };
            }

        }
        public MidReturn ReceiveWebhook(WebhookRequestBodyDto requestBody)
        {
            foreach (var eventObject in requestBody.Events)
            {
                switch (eventObject.Type)
                {
                    case WebhookEventTypeEnum.Message:
                        //var groupId = eventObject.Source.GroupId ?? eventObject.Source.RoomId;
                        string userMessage = eventObject.Message.Text;
                        List<TextMessageDto> messages = new List<TextMessageDto>();

                        switch (userMessage)
                        {
                            case string message when message.Equals("Hello"):
                                messages.Add(new TextMessageDto { Text = $"希望您今天過得愉快！" });
                                break;

                            case string message when message.Equals("群組"):
                                var groupId = eventObject.Source.GroupId ?? eventObject.Source.RoomId;
                                messages.Add(new TextMessageDto { Text = $"群組 ID 是：\"{groupId}\"。" });
                                break;

                            case string message when message.Equals("時間"):
                                messages.Add(new TextMessageDto { Text = $"現在時間是：{DateTime.Now:HH:mm:ss}" });
                                break;
                            case string message when message.Equals("Echo"):
                                messages.Add(new TextMessageDto { Text = $"您剛剛說了：\"{userMessage}\"" });
                                break;

                            default:
                                // 預設回復
                                break;
                        }

                        // 設定回覆訊息請求
                        var replyMessage = new ReplyMessageRequestDto<TextMessageDto>()
                        {
                            ReplyToken = eventObject.ReplyToken,
                            Messages = messages
                        };

                        // 傳送回覆訊息
                        ReplyMessageHandler("text", replyMessage);
                        break;
                        //case WebhookEventTypeEnum.Unsend:
                        //    Console.WriteLine($"使用者{eventObject.Source.UserId}在聊天室收回訊息！");
                        //    break;
                        //case WebhookEventTypeEnum.Follow:
                        //    Console.WriteLine($"使用者{eventObject.Source.UserId}將我們新增為好友！");
                        //    break;
                        //case WebhookEventTypeEnum.Unfollow:
                        //    Console.WriteLine($"使用者{eventObject.Source.UserId}封鎖了我們！");
                        //    break;
                        //case WebhookEventTypeEnum.Join:
                        //    Console.WriteLine("我們被邀請進入聊天室了！");
                        //    break;
                        //case WebhookEventTypeEnum.Leave:
                        //    Console.WriteLine("我們被聊天室踢出了");
                        //    break;
                        //case WebhookEventTypeEnum.MemberJoined:
                        //    string joinedMemberIds = "";
                        //    foreach (var member in eventObject.Joined.Members)
                        //    {
                        //        joinedMemberIds += $"{member.UserId} ";
                        //    }
                        //    Console.WriteLine($"使用者{joinedMemberIds}加入了群組！");
                        //    break;
                        //case WebhookEventTypeEnum.MemberLeft:
                        //    string leftMemberIds = "";
                        //    foreach (var member in eventObject.Left.Members)
                        //    {
                        //        leftMemberIds += $"{member.UserId} ";
                        //    }
                        //    Console.WriteLine($"使用者{leftMemberIds}離開了群組！");
                        //    break;
                        //case WebhookEventTypeEnum.Postback:
                        //    Console.WriteLine($"使用者{eventObject.Source.UserId}觸發了postback事件");
                        //    break;
                        //case WebhookEventTypeEnum.VideoPlayComplete:
                        //    Console.WriteLine($"使用者{eventObject.Source.UserId}");
                        //    break;
                }
            }
            return new MidReturn { msg = "0000" };
        }

        /// <summary>
        /// 接收到廣播請求時，在將請求傳至 Line 前多一層處理，依據收到的 messageType 將 messages 轉換成正確的型別，這樣 Json 轉換時才能正確轉換。
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="requestBody"></param>
        public MidReturn BroadcastMessageHandler(string messageType, object requestBody)
        {
            string strBody = requestBody.ToString();
            switch (messageType)
            {
                case MessageTypeEnum.Text:
                    var messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<TextMessageDto>>(strBody);
                    BroadcastMessage(messageRequest);
                    break;
            }
            return new MidReturn { msg = "0000" };

        }

        /// <summary>
        /// 將廣播訊息請求送到 Line
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        public async Task<MidReturn> BroadcastMessage<T>(BroadcastMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ChannelAccessToken); // 帶入 channel access token

            var json = _jsonProvider.Serialize(request);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(broadcastMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            return new MidReturn { msg = "0000" };
        }

        /// <summary>
        /// 接收到回覆請求時，在將請求傳至 Line 前多一層處理(目前為預留)
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="requestBody"></param>
        public MidReturn ReplyMessageHandler<T>(string messageType, ReplyMessageRequestDto<T> requestBody)
        {
            ReplyMessage(requestBody);
            return new MidReturn { msg = "0000" };
        }

        /// <summary>
        /// 將回覆訊息請求送到 Line
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        public async Task<MidReturn> ReplyMessage<T>(ReplyMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ChannelAccessToken); // 帶入 channel access token

            var json = _jsonProvider.Serialize(request);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(replyMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            return new MidReturn { msg = "0000" };
        }
    }
}
