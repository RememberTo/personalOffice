using MessageBusCore.Data;

namespace MessageBus.Data
{
    public class ConnectionData
    {
        public string? HostName { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? VirtualHost { get; set; } = "/";
        public bool IsSSL { get; set; }
        public ReceivedQueueData? ReceivedQueue { get; set; }
        public ReceivedQueueData? SubReceivedQueue { get; set; }

        public ConnectionData() { }
        public ConnectionData(string hostName, int port, string userName, string password)
        {
            HostName = hostName;
            Port = port;
            UserName = userName;
            Password = password;
        }
    }
}
