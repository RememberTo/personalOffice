namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotifyById
{
    internal class MarkNotifyDto
    {
        public int NotifyID { get; set; }
        public int MarkCount { get; set; }
        public Exception? Error { get; set; }
    }
}
