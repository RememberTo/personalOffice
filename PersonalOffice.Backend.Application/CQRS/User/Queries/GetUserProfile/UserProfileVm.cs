using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Domain.Entites.User;
using PersonalOffice.Backend.Domain.Entities.User;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetProfile
{
    /// <summary>
    /// Контракт настроек профиля пользователя
    /// </summary>
    public class UserProfileVm : IMapWith<UserProfile>
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
        /// <param name="profile">Профиль маппинга</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserProfileVm, UserProfile>().ReverseMap();

            profile.CreateMap<Domain.Entites.User.User, UserProfile>()
                .ForMember(up => up.Phone, opt => opt.MapFrom(u => u.Telephone))
                .ForMember(up => up.IsTwoFactor, opt => opt.MapFrom(u => u.IsTwoFactorAuth))
                .ForMember(up => up.CanTwoFactor, opt => opt.MapFrom(u => u.IsPhysic || !u.IsResident))
                .ForMember(up => up.IsPhoneConfirmed, opt => opt.MapFrom(u => u.IsPhoneConfirmed))
                .ForMember(up => up.HasPhone, opt => opt.MapFrom(u => !string.IsNullOrEmpty(u.Telephone)))
                .ForMember(up => up.IsPassTroughAuth, opt => opt.MapFrom(u => u.Roles.Any(x => x.Id == 10)))
                .ForMember(up => up.Subscriptions, opt => opt.MapFrom(u => ToSubs(u.Roles)));
        }

        private ICollection<SubscriptionNotifyInfo> ToSubs(ICollection<RoleInfo> roles)
        {
            return new List<SubscriptionNotifyInfo>()
                {
                    new() {Id = 3, IsSubscription = roles.Any(x => x.Id == 3)},
                    new() {Id = 4, IsSubscription = roles.Any(x => x.Id == 4)},
                    new() {Id = 5, IsSubscription = roles.Any(x => x.Id == 5)},
                    new() {Id = 7, IsSubscription = roles.Any(x => x.Id == 7)},
                    new() {Id = 8, IsSubscription = roles.Any(x => x.Id == 8)},
                    new() {Id = 9, IsSubscription = roles.Any(x => x.Id == 9)},
                };
        }
    }
}
