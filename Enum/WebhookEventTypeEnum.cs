namespace LineBot_api.Enum
{
    public static class WebhookEventTypeEnum
    {
        public const string Message = "message";
        public const string Unsend = "unsend";
        public const string Follow = "follow";
        public const string Unfollow = "unfollow";
        public const string Join = "join";
        public const string Leave = "leave";
        public const string MemberJoined = "memberJoined";
        public const string MemberLeft = "memberLeft";
        public const string Postback = "postback";
        public const string VideoPlayComplete = "videoPlayComplete";
    }
    public static class SendMessageMethodEnum
    {
        public const string Reply = "Reply";
        public const string Push = "Push";
        public const string Multicast = "Multicast";
        public const string Narrowcast = "Narrowcast";
        public const string Broadcast = "Broadcast";
    }
    public static class MessageTypeEnum
    {
        public const string Text = "text";
        public const string Sticker = "sticker";
        public const string Image = "image";
        public const string Video = "video";
        public const string Audio = "audio";
        public const string Location = "location";
        public const string Imagemap = "imagemap";
        public const string Template = "template";
        public const string Flex = "flex";
    }
}
