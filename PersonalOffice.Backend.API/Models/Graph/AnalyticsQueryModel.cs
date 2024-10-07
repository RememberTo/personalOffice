using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractAllGraphs;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractGraph;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetGeneralGraph;
using PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetContractIndicators;
using PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetGeneralIndicators;
using PersonalOffice.Backend.Domain.Entities.Graph;
using PersonalOffice.Backend.Domain.Enums;

namespace PersonalOffice.Backend.API.Models.Graph
{
    /// <summary>
    /// Модель запроса для построения графика
    /// </summary>
    public class AnalyticsQueryModel : IMapWith<AnalyticsDataQuery>
    {
        /// <summary>
        /// Идентфиикатор валюты
        /// По умолчанию 1 - рубль РФ
        /// </summary>
        public int currencyId { get; set; } = 1;
        /// <summary>
        /// Начальная дата графика
        /// По умолчанию текущая дата - 1 месяц
        /// </summary>
        public DateTime beginDate { get; set; } = DateTime.Now.AddMonths(-1);
        /// <summary>
        /// Конечная дата графика
        /// По умолчанию текущая дата
        /// </summary>
        public DateTime endDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AnalyticsQueryModel, GetContractGraphQuery>()
                .ForMember(x => x.CurrencyId, opt => opt.MapFrom(y => MapCurrency(y.currencyId)));
            profile.CreateMap<AnalyticsQueryModel, GetContractAllGraphsQuery>()
                .ForMember(x => x.CurrencyId, opt => opt.MapFrom(y => MapCurrency(y.currencyId)));
            profile.CreateMap<AnalyticsQueryModel, GetGeneralGraphQuery>()
                .ForMember(x => x.CurrencyId, opt => opt.MapFrom(y => MapCurrency(y.currencyId)));
            profile.CreateMap<AnalyticsQueryModel, GetContractIndicatorsQuery>()
                .ForMember(x => x.CurrencyId, opt => opt.MapFrom(y => MapCurrency(y.currencyId)));
            profile.CreateMap<AnalyticsQueryModel, GetGeneralIndicatorsQuery>()
                .ForMember(x => x.CurrencyId, opt => opt.MapFrom(y => MapCurrency(y.currencyId)));
        }

        private static Currency MapCurrency(int currencyId)
        {
            if (Enum.IsDefined(typeof(Currency), currencyId))
            {
                return (Currency)currencyId;
            }
            else
            {
                return Currency.RoubleRF;
            }
        }
    }
}
