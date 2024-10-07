namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetPassThroughData
{
    /// <summary>
    /// Контракт с данными пользователя для бесшовного перехода
    /// </summary>
    public class PassThroughDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public required int СlientId { get; set; }
        /// <summary>
        /// Секретный ключ
        /// </summary>
        public required string Encoded { get; set; }
    }
}
