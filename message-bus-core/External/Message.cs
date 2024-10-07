namespace MessageBus.External
{
    public enum MessageType { Request, Response };
    public enum MessageContentType { Binary, JSON, XML };

    [Serializable]
    public class Message
    {

#if DEBUG
        public string? SenderServiceName { get; set; }  //Сервис который отправил сообщение
#endif
        public required string ID { get; set; }
        public MessageType Type { get; set; }
        public string? Method { get; set; }
        public required string Destination { get; set; }
        public string? Source { get; set; }
        public string? AuthToken { get; set; }
        public object? Data { get; set; }
        public int Version { get; set; }
        [NonSerialized]
        public MessageContentType ContentType = MessageContentType.JSON;
    }
}
