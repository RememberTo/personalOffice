namespace PersonalOffice.Backend.Domain.Interfaces.Services
{
    /// <summary>
    /// Менеджер кеширования
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Получение закешированного объекта
        /// </summary>
        /// <typeparam name="T">Тип закешированного объекта</typeparam>
        /// <param name="key">Ключ для получения объекта</param>
        /// <returns></returns>
        public Task<T?> Get<T>(string key) where T : class;

        /// <summary>
        /// Кеишрвоание объекта
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="key">По какому ключу будет записан объект</param>
        /// <param name="value">Объект</param>
        /// <param name="expiry">Время удаления объекта</param>
        /// <returns></returns>
        public Task Set<T>(string key, T value, TimeSpan expiry = default) where T : class;
    }
}
