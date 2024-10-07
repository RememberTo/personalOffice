using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetInvestProfile
{
    /// <summary>
    /// Модель представления инвестиционного профиля
    /// </summary>
    public record InvestProfileVm
    {
        /// <summary>
        /// ФИО клиента 
        /// </summary>
        [JsonProperty("Name")]
        public required string FIO { get; set; }
        /// <summary>
        /// Является ли клент физическим лицом
        /// </summary>
        public bool IsPhysic { get; set; }
        /// <summary>
        /// Название договора
        /// </summary>
        public string Contract { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServstateDate { get; set; }
        /// <summary>
        /// Инн клиента
        /// </summary>
        public required string INN { get; set; }
        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Огрн документа
        /// </summary>
        public string DocOgrn { get; set; }
        /// <summary>
        /// Явялется ли клиент квал инвестором
        /// </summary>
        public bool IsQual { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsQualInLaw { get; set; }
    }
}
