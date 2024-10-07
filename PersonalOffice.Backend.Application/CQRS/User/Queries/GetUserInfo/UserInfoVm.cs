using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetUserInfo
{
    /// <summary>
    /// Модель представления информации о Персоне клиента для фронта
    /// PS: Будет дополняться по мере необходимости
    /// </summary>
    public class UserInfoVm
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("UserId")]
        public int UserId { get; set; }
        /// <summary>
        /// Доступность пункта меню "Связаться с инвестиционным консультантом"
        /// </summary>
        public bool CanContactInvestmentConsultant { get; set; }
        /// <summary>
        /// Общая стоимость всех портфелей, (ЦБ и денег)
        /// </summary>
        public decimal TotalPortfolioValue { get; set; }

    }
}

