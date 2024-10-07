namespace PersonalOffice.Backend.Domain.Entites.User
{
    /// <summary>
    /// Информация о подписках пользователя
    /// </summary>
    public class SubscriptionNotifyInfo
    {
        /// <summary>
        /// Идентификатор роли
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название роли
        /// </summary>
        public string Name => _roleFromSubscriptionName[this.Id];
        /// <summary>
        /// Подписан ли на уведомление
        /// </summary>
        public bool IsSubscription { get; set; }

        private readonly static Dictionary<int, string> _roleFromSubscriptionName = new()
        {
            [3] = "EveningReport",      // Ежевечерняя рассылка клиентских отчетов
            [4] = "MorningReport",      // Ежеутренняя рассылка клиентских отчетов
            [5] = "AnalyticsDaily",     // Ежедневная аналитика
            [7] = "AnalyticsWeekly",    // Еженедельная аналитика
            [8] = "AnalyticsShares",    // Аналитика по акциям
            [9] = "AnalyticsBonds",     // Аналитика по облигациям
        };
    }
}
