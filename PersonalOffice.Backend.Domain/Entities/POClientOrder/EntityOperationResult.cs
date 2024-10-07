namespace PersonalOffice.Backend.Domain.Entities.POClientOrder
{
    /// <summary>
    /// Результат выполнения заявки
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityOperationResult<T>
    {
        /// <summary>
        /// Статус выполнения
        /// </summary>
        public required bool IsSuccess { get; set; }
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public required int EntityID { get; set; }
        /// <summary>
        /// Сообщение о результате
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Данные выполнения операции
        /// </summary>
        public T? ReturnValue { get; set; }
    }
}
