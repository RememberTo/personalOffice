using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Entities.Graph;

namespace PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractAllGraphs
{
    /// <summary>
    /// Контракт на получение данных графика по всем договорам
    /// </summary>
    public class GetContractAllGraphsQuery : AnalyticsDataQuery, IRequest<IEnumerable<AllGraphVm>>
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
