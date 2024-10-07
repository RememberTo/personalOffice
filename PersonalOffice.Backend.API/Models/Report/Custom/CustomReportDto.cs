using AutoMapper;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.Report.Commands.CreateCustomReport;
using System.ComponentModel.DataAnnotations;

namespace PersonalOffice.Backend.API.Models.Report.Custom
{
    /// <summary>
    /// Контракт для создания отчета c заданными параметрами
    /// </summary>
    public class CustomReportDto : IMapWith<CreateCustomReportCommand>
    {
        /// <summary>
        /// Идентификатор контракта пользователя
        /// </summary>
        [Required]
        public int ContractID { get; set; }
        /// <summary>
        /// Начальная дата отчета
        /// </summary>
        [Required]
        public required DateTime BeginDate { get; set; }
        /// <summary>
        /// Конечная дата отчета
        /// </summary>
        [Required]
        public required DateTime EndDate { get; set; }
        /// <summary>
        /// Тип отчета цифрой
        /// </summary>
        [RegularExpression(@"^\d+$")]
        [MaxLength(5)]
        [Required]
        public required string ReportType { get; set; }
        /// <summary>
        /// Наименование типа отчета
        /// </summary>
        [Required]
        public required string ReportTypeName { get; set; }
        /// <summary>
        /// Тип отчета, указывается формат
        /// </summary>
        [Required]
        public required string ReportFormat { get; set; }
        /// <summary>
        /// Язык отчета цифрой
        /// </summary>
        [RegularExpression(@"^\d+$")]
        [MaxLength(5)]
        [Required]
        public required string Language { get; set; }
        /// <summary>
        /// Валюта отчета
        /// </summary>
        [RegularExpression(@"^\d+$")]
        [MaxLength(5)]
        [Required]
        public required string Currency { get; set; }
        /// <summary>
        /// Тип цены для отчета (рыночная или по последним сделкам)
        /// </summary>
        [RegularExpression(@"^\d+$")]
        [MaxLength(5)]
        [Required]
        public required string PriceType { get; set; }

        /// <summary>
        /// Выполнение маппинга
        /// </summary>
        /// <param name="profile">профиль мапинга</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CustomReportDto, CreateCustomReportCommand>();
        }
    }
}
