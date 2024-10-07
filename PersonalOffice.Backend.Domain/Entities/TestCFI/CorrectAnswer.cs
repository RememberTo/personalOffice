using System.Xml.Serialization;

namespace PersonalOffice.Backend.Domain.Entites.TestCFI
{
    /// <summary>
    /// Правильный ответ
    /// </summary>
    public class CorrectAnswer
    {
        /// <summary>
        /// Идентификатор ответа
        /// </summary>
        [XmlElement("IdAnswer")]
        public required string IdAnswer { get; set; }
        /// <summary>
        /// Содержимое 
        /// </summary>
        [XmlElement("AnswerVariant")]
        public string? Content { get; set; }
    }
}
