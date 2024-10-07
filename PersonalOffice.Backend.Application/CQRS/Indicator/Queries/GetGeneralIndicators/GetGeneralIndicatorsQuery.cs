using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetContractIndicators;
using PersonalOffice.Backend.Domain.Entities.Graph;

namespace PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetGeneralIndicators
{
    /// <summary>
    /// Контракт на получение общих индикаторов
    /// </summary>
    public class GetGeneralIndicatorsQuery : AnalyticsDataQuery, IRequest<IndicatorsVm>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.SF_ContractDataRequest, MessageDataTypes";
    }
}
