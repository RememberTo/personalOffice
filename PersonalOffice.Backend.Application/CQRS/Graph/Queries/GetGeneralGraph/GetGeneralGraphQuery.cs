using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.General;
using PersonalOffice.Backend.Domain.Entities.Graph;

namespace PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetGeneralGraph
{
    /// <summary>
    /// Контракт на получение данных графика по всем договорам
    /// </summary>
    public class GetGeneralGraphQuery : AnalyticsDataQuery, IRequest<IEnumerable<PointGraphVm>>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.SF_ContractDataRequest, MessageDataTypes";
    }
}
