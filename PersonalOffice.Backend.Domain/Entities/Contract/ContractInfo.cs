using Newtonsoft.Json;
using System.Xml.Serialization;

namespace PersonalOffice.Backend.Domain.Entities.Contract
{
    /// <summary>
    /// Информация о договоре пользователя
    /// </summary>
    [Serializable]
    public class ContractInfo
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.ContractInfo, MessageDataTypes";
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        [JsonProperty("ID")]
        [XmlElement(ElementName = "ID")]
        public int Id { get; set; }
        /// <summary>
        /// Номер договора
        /// </summary>
        [XmlElement(ElementName = "Name")]
        public required string DocNum { get; set; }
        /// <summary>
        /// Дата заключения договора
        /// </summary>
        [XmlElement(ElementName = "Date")]
        public required string DocDate { get; set; }
        /// <summary>
        /// ФИО пользователя в сокращенном вариант Фамилия И.О.
        /// </summary>
        public required string PersonShortName { get; set; }
        /// <summary>
        /// Код пользователя
        /// </summary>
        public required string PersonCode { get; set; }
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public required string PersonName { get; set; }
        /// <summary>
        /// Дата окончания договора
        /// </summary>
        [XmlIgnore]
        public required string EndDate { get; set; }
        /// <summary>
        /// Тариф договора
        /// </summary>
        [XmlIgnore]
        public required string Tariff { get; set; }
        /// <summary>
        /// Явялется ли этот договора действующим
        /// </summary>
        [XmlIgnore]
        public bool IsActive { get; set; }
        /// <summary>
        /// Идентификатор стратегии
        /// </summary>
        [JsonProperty("DuStrategyID")]
        [XmlIgnore]
        public int DuStrategyId { get; set; }
        /// <summary>
        /// Название стратегии
        /// </summary>
        [XmlIgnore]
        public string? DuStrategyName { get; set; }
        /// <summary>
        /// Является ли договора ИИС
        /// </summary>
        [XmlIgnore]
        public bool IsIis { get; set; }
        /// <summary>
        /// Является ли договора типа ДУ
        /// </summary>
        [XmlIgnore]
        public bool IsDu { get; set; }
        /// <summary>
        /// Является ли договор типа ДП
        /// </summary>
        [XmlIgnore]
        public bool IsDP { get; set; }
        /// <summary>
        /// Дата последнего изменения досье
        /// </summary>
        [XmlIgnore]
        public DateTime DossierLastChangeDate { get; set; }
        /// <summary>
        /// Идентификатор персонального брокера
        /// </summary>
        public int PersonIDBroker { get; set; }
        /// <summary>
        /// Является ли единым брокерским счетом(ЕБС)
        /// </summary>
        public bool IsEbs { get; set; }
    }
}
