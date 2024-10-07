using AutoMapper;

namespace PersonalOffice.Backend.Application.Common.Mappings
{
    /// <summary>
    /// Интерфейс для реализации мапинга
    /// </summary>
    /// <typeparam name="T">тип</typeparam>
    public interface IMapWith<T>
    {
        /// <summary>
        /// Метод который выполняет мапинг
        /// </summary>
        /// <param name="profile">профиль мапинга</param>
        void Mapping(Profile profile);
    }
}
