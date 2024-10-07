using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Commands.SignTest
{
    /// <summary>
    /// Контракт на подписание и сохранение пройденного теста
    /// </summary>
    public class SaveTestCommand
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.TestRequest, MessageDataTypes";
        /// <summary>
        /// Название теста
        /// </summary>
        public required string TestName { get; set; }
        /// <summary>
        /// Идентификатор теста
        /// </summary>
        public byte TestTypeID { get; set; }
        /// <summary>
        /// Тест в формате XML
        /// </summary>
        public required string XML { get; set; }
        /// <summary>
        /// Идентфикатор пользователя
        /// </summary>
        public int PersonID { get; set; }
        /// <summary>
        /// Результат тестирования
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Код для подписания
        /// </summary>
        public required string Code { get; set; }
        /// <summary>
        /// Подпись
        /// </summary>
        public required string Sign { get; set; }
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public required string PathFile { get; set; }
    }
}
