using System.Xml.Serialization;

namespace PersonalOffice.Backend.Domain.Entities.Document.Info.Base
{
    /// <summary>
    /// Базовое представление информации о документе
    /// </summary>
    [Serializable]
    [XmlRoot("DocBaseInfo")]
    public class DocumentBaseInfo
    {
        /// <summary>
        /// Название договора
        /// </summary>
        public string? Contract { get; set; }
        /// <summary>
        /// Тип документа
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// Название типа документа
        /// </summary>
        public string? TypeName { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        public string? Date { get; set; }
        /// <summary>
        /// Пользователь является физ лицом
        /// </summary>
        public bool IsPhysic { get; set; }
        /// <summary>
        /// Состояние документа
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Дата состояния документа 
        /// </summary>
        public string? StatusDate { get; set; }
        /// <summary>
        /// Название состояния дкумента
        /// </summary>
        public string? StatusName { get; set; }
        /// <summary>
        /// Комментарий к статусу документа
        /// </summary>
        public string? DocStatusComment { get; set; }
        /// <summary>
        /// Подписан ли документ клиентом
        /// </summary>
        public bool NeedClientSign { get; set; }
    }
}
