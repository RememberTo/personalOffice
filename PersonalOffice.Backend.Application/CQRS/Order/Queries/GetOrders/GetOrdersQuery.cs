using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetContract;
using PersonalOffice.Backend.Application.Services;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entites.User;
using PersonalOffice.Backend.Domain.Entities.Graph;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalOffice.Backend.Application.CQRS.Order.Queries.GetOrders
{
    internal class GetOrdersQuery : IRequest<IEnumerable<string>>
    {
        public int UsertId { get; set; }
        public int Status { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }
    }
    public class RRRRRRRR
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.OR_Orders4UserRequest, MessageDataTypes";

        public int UserID { get; set; }
        public int Status { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetOrdersQueryHandler(
        ILogger<GetContractQueryHandler> logger,
        IMapper mapper,
        ITransportService contractService) : IRequestHandler<GetOrdersQuery, IEnumerable<string>>
    {
        private readonly ILogger<GetContractQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly ITransportService _transportService = contractService;


        async Task<IEnumerable<string>> IRequestHandler<GetOrdersQuery, IEnumerable<string>>.Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "OR_Orders4User",
                Data = new RRRRRRRR { UserID = request.UsertId, Status = request.Status, Page = request.PageNum, PageSize = request.PageSize },
            }, TimeSpan.FromMinutes(12));

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "OR_Orders4User",
                Data = new RRRRRRRR { UserID = request.UsertId, Status = request.Status, Page = request.PageNum, PageSize = request.PageSize },
            }, TimeSpan.FromMinutes(12));

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "OR_Orders4User",
                Data = new RRRRRRRR { UserID = request.UsertId, Status = request.Status, Page = request.PageNum, PageSize = request.PageSize },
            }, TimeSpan.FromMinutes(12));

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "OR_Orders4User",
                Data = new RRRRRRRR { UserID = request.UsertId, Status = request.Status, Page = request.PageNum, PageSize = request.PageSize },
            }, TimeSpan.FromMinutes(12));

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<IEnumerable<OrderListItem>>>(msg.Data);

            if (sqlResult == null || !sqlResult.Success || sqlResult.ReturnValue is null)
            {
                _logger.LogTrace("Ошибка получения данных: {msg}", sqlResult?.Message);
                return [];
            }
            sqlResult.ReturnValue 

                return new OrdersVm
                {
                    OrderListItem = sqlResult.ReturnValue
                }

            if (int.TryParse(Request["pagenum"], out PageNumber) && int.TryParse(Request["pageitems"], out ItemsOnPage) &&
                               int.TryParse(Request["status"], out int Status) &&
                               Global.SessionData.BackendRequest.GetOrderList4User(Global.SessionData.User.UserID, Status, PageNumber, ItemsOnPage
                                        , out OrderListAndPageCount Orders) == ResponseStatus.Success
                               )
            {
                OrderHandlerResponse OrderResp = new OrderHandlerResponse()
                {
                    Orders = Orders.Orders,
                    TotalCount = Orders.PageCount
                };

                foreach (var el in OrderResp.Orders)
                {
                    el.ID = _symmetricEncrypt.Encrypt(el.ID + _symmetricEncrypt.Salt, password);
                    el.ContractID = _symmetricEncrypt.Encrypt(el.ContractID + _symmetricEncrypt.Salt, password);
                }

                Result = JsonConvert.SerializeObject(OrderResp);
            }


        }
    }

    public class OrdersVm
    {
        OrderListItem 

        }

    public class OrderListItem
    {
        public string ID { get; set; }
        public string ContractID { get; set; }
        public string ContractNum { get; set; }
        public int Num { get; set; }
        public byte DocType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int StatusID { get; set; }
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
