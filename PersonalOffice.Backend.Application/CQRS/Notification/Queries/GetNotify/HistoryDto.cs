namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotify
{
    internal class HistoryDto
    {
        public required IEnumerable<NotificationVm> History { get; set; } = [];
    }
}
