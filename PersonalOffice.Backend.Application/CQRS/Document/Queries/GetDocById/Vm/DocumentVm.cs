namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocById.Vm
{
    /// <summary>
    /// Модель представления документа
    /// </summary>
    public class DocumentBaseVm
    {
        /// <summary>
        /// Название договора
        /// </summary>
        public required string Contract { get; set; }
        /// <summary>
        /// Тип документа
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// Название типа документа
        /// </summary>
        public required string TypeName { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        public required string Date { get; set; }
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
        public required string StatusDate { get; set; }
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
