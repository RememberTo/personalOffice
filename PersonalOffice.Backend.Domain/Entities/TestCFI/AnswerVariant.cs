using System.Xml.Serialization;

namespace PersonalOffice.Backend.Domain.Entites.TestCFI
{
    /// <summary>
    /// Вариант ответа
    /// </summary>
    public class AnswerVariant
    {
        /// <summary>
        /// Идентификатор варианта ответа
        /// </summary>
        [XmlElement("IdAnswer")]
        public required string IdAnswer { get; set; }
        /// <summary>
        /// Содержимое
        /// </summary>
        [XmlText]
        public string? Content { get; set; }
    }
}
