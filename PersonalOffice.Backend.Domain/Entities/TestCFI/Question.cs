using System.Xml.Serialization;

namespace PersonalOffice.Backend.Domain.Entites.TestCFI
{
    /// <summary>
    /// Вопрос
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Сложность
        /// </summary>
        [XmlAttribute("difficulty")]
        public int Difficulty { get; set; }
        /// <summary>
        /// Номер варианта
        /// </summary>
        [XmlAttribute("variant")]
        public int Variant { get; set; }
        /// <summary>
        /// Содержимое вопроса
        /// </summary>
        [XmlText]
        public string? Content { get; set; }
        /// <summary>
        /// Тип вопроса
        /// </summary>
        [XmlElement("Type")]
        public string? Type { get; set; }
        /// <summary>
        /// Варианты ответов
        /// </summary>
        [XmlElement("AnswerVariant")]
        public required List<AnswerVariant> AnswerVariants { get; set; }
        /// <summary>
        /// Правильный вариант ответа
        /// </summary>
        [XmlElement("CorrectAnswer")]
        public required CorrectAnswer CorrectAnswer { get; set; }
    }
}
