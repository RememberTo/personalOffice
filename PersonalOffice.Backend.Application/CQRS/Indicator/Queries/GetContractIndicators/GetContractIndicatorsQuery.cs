using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Entities.Graph;

namespace PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetContractIndicators
{
    /// <summary>
    /// Контракт на получение индикаторов по договору
    /// </summary>
    public class GetContractIndicatorsQuery : AnalyticsDataQuery, IRequest<IndicatorsVm>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.SF_ContractDataRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public int ContractId { get; set; }
    }
}
