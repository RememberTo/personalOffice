using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.User.Commands;

namespace PersonalOffice.Backend.API.Models.Auth
{
    /// <summary>
    /// Контракт для обмена токеном доступа
    /// </summary>
    public class TokenDto : IMapWith<RefreshAccessTokenCommand>
    {
        /// <summary>
        /// Токен аутентификаиции
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле AccessToken обязательное")]
        [NotNull]
        public required string AccessToken { get; set; }

        /// <summary>
        /// Создает мап для типов TokenDto и RefreshAccessTokenCommand
        /// </summary>
        /// <param name="profile">профиль мапинга</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TokenDto, RefreshAccessTokenCommand>().ReverseMap();
        }
    }
}