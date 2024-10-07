namespace MessageBusCore.Data
{
    public class ReceivedQueueData
    {
        public Guid QueueID { get; private set; } = Guid.NewGuid();
        public string? QueueName { get; set; }
        public string Exchange { get; set; } = "SolidMain";
        public bool IsAutoCreatedQueue { get; set; }
        public bool Exclusive { get; set; }
        public bool BindQueue { get; set; }
        public bool AutoDelete { get; set; }

        public override string ToString()
        {
            return "\n\t\t\t\tID: " + QueueID.ToString() +
                "\n\t\t\t\tОчередь: " + QueueName +
                "\n\t\t\t\tExchange: " + Exchange +
                "\n\t\t\t\tАвтоматическое создание очереди: " + IsAutoCreatedQueue +
                "\n\t\t\t\tЭксклюзивность: " + Exclusive +
                "\n\t\t\t\tПривязка к exchange: " + BindQueue +
                "\n\t\t\t\tАвтоматическое удаление: " + AutoDelete;
        }
    }
}
