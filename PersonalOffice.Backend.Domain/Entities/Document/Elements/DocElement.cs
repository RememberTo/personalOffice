using System.Xml.Serialization;

namespace PersonalOffice.Backend.Domain.Entities.Document.Elements
{
    /// <summary>
    /// Структура сопостовления профилей
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    [XmlRoot("DocElement")]
    public struct DocElement<TKey, TValue>
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public TKey Key { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public TValue Value { get; set; }
    }
}
