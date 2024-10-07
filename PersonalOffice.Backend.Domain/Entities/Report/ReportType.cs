namespace PersonalOffice.Backend.Domain.Entities.Report
{
    /// <summary>
    /// Типы отчетов
    /// </summary>
    public static class ReportType
    {
        /// <summary>
        /// Названите типов отчетов
        /// </summary>
        public static readonly Dictionary<string, string> Names = new Dictionary<string, string>()
        {
            ["9"] = "Отчет клиенту с приложениями",
            ["5"] = "Отчет клиенту",
            ["6"] = "Приложение 1. (Сделки)",
            ["10"] = "Приложение 1. (Займы)",
            ["11"] = "Приложение 1. (Расшифровка движений)",
            ["1"] = "Приложение 2. (Срочный рынок)",
            ["13"] = "Приложение 4 (Валютный рынок)",
            ["14"] = "Клиентский лимит",
            ["12"] = "Выгрузка данных в бухгалтерию"
        };
    }
}
