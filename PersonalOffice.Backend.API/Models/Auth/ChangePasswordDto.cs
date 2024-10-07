using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.User.Commands.ChangePassword;
using System.ComponentModel.DataAnnotations;

namespace PersonalOffice.Backend.API.Models.Auth
{
    /// <summary>
    /// Контракт на смену пароля
    /// </summary>
    public class ChangePasswordDto : IMapWith<ChangePasswordCommand>
    {
        /// <summary>
        /// Хэш старого пароля
        /// </summary>
        [Required]
        public required string OldPasswordHash { get; set; }
        /// <summary>
        /// Хэш нового пароля
        /// </summary>
        [Required]
        public required string NewPasswordHash { get; set; }
        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="profile">профиль мапинга</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChangePasswordDto, ChangePasswordCommand>();
        }
    }
}