using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Domain.Entities.Contract;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetContract
{
    /// <summary>
    /// Модель представления договора
    /// </summary>
    public class ContractVm : IMapWith<ContractInfo>
    {
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public required string DocNum { get; set; }

        /// <summary>
        /// Дата заключения договора
        /// </summary>
        public required string DocDate { get; set; }

        /// <summary>
        /// ФИО пользователя в сокращенном вариант Фамилия И.О.
        /// </summary>
        public required string PersonShortName { get; set; }

        /// <summary>
        /// Код пользователя
        /// </summary>
        public required string PersonCode { get; set; }

        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public required string PersonName { get; set; }

        /// <summary>
        /// Дата окончания договора
        /// </summary>
        public required string EndDate { get; set; }

        /// <summary>
        /// Тариф договора
        /// </summary>
        public required string Tariff { get; set; }

        /// <summary>
        /// Явялется ли этот договора действующим
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Идентификатор стратегии
        /// </summary>
        public int DuStrategyId { get; set; }

        /// <summary>
        /// Название стратегии
        /// </summary>
        public string? DuStrategyName { get; set; }

        /// <summary>
        /// Является ли договора ИИС
        /// </summary>
        public bool IsIis { get; set; }

        /// <summary>
        /// Является ли договора типа ДУ
        /// </summary>
        public bool IsDu { get; set; }

        /// <summary>
        /// Является ли договор типа ДП
        /// </summary>
        public bool IsDP { get; set; }

        /// <summary>
        /// Дата последнего изменения досье
        /// </summary>
        public DateTime DossierLastChangeDate { get; set; }

        /// <summary>
        /// Идентификатор персонального брокера
        /// </summary>
        public int PersonIDBroker { get; set; }
        /// <summary>
        /// Является ли единым брокерским счетом(ЕБС)
        /// </summary>
        public bool IsEbs { get; set; }
        /// <summary>
        /// Сумма на договоре
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Результат
        /// </summary>
        public decimal? ProfitLoss { get; set; }
        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="profile"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ContractInfo, ContractVm>();
        }
    }
}
