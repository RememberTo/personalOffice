using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.General;
using PersonalOffice.Backend.Domain.Entities.Graph;

namespace PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractGraph
{
    /// <summary>
    /// Контракт на получение данных графика по договору
    /// </summary>
    public class GetContractGraphQuery : AnalyticsDataQuery, IRequest<IEnumerable<PointGraphVm>>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.SF_ContractDataRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        [JsonIgnore]
        public int ContractId { get; set; }
    }
}
