using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.Payment.Commands.TopUpAccount;
using PersonalOffice.Backend.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalOffice.Backend.API.Models.Payment
{
    /// <summary>
    /// Модель для создания формы пополнения
    /// </summary>
    public class TopUpModel : IMapWith<TopUpAccountCommand>
    {
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        [Required]
        public int ContractId { get; set; }
        /// <summary>
        /// Идентификатор портфеля
        /// </summary>
        [Required]
        public int PortfolioId { get; set; }
        /// <summary>
        /// Валюта
        /// </summary>
        public Currency Currency { get; set; } = Currency.RoubleRF;
        /// <summary>
        /// Метод пополнения
        /// </summary>
        [Required]
        public PaymentOptions PaymentOptions { get; set; }
        /// <summary>
        /// Сумма пополнения
        /// </summary>
        [Required]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Оплата с мобильного
        /// </summary>
        public bool IsMobile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TopUpModel,  TopUpAccountCommand>();
        }
    }
}
