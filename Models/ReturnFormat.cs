namespace LineBot_api.Models
{
    public class ReturnFormat
    {
        public string traceId { get; set; } = string.Empty;
        public string rtnCode { get; set; } = string.Empty;
        public string msg { get; set; } = string.Empty;
        public object info { get; set; } = string.Empty;
    }
}
