using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.User.Commands.UpdateProfile;
using PersonalOffice.Backend.Domain.Entites.User;

namespace PersonalOffice.Backend.API.Models.User
{
    /// <summary>
    /// Контракт настроек профиля пользователя
    /// </summary>
    public class UserProfileDto : IMapWith<UserProfile>
    {
        /// <summary>
        /// Номер телефона
        /// </summary>
        public required string Phone { get; set; }
        /// <summary>
        /// Привязан ли номер телефона
        /// </summary>
        public bool HasPhone { get; set; }
        /// <summary>
        /// Доступ к двухфакторной аутентификации
        /// </summary>
        public bool IsTwoFactor { get; set; }
        /// <summary>
        /// Доступ к двухфакторной аутентификации
        /// </summary>
        public bool CanTwoFactor { get; set; }
        /// <summary>
        /// Номер телефона подтвержден
        /// </summary>
        public bool IsPhoneConfirmed { get; set; }
        /// <summary>
        /// Бесшовный переход между приложениями Солид Брокер и Инвестор
        /// </summary>
        public bool IsPassTroughAuth { get; set; }
        /// <summary>
        /// Подписки на рассылки
        /// </summary>
        public required ICollection<SubscriptionNotifyInfo> Subscriptions { get; set; }
        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="profile">Профиль мапинга</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserProfileDto, UserProfile>().ReverseMap();
        }
    }
}
