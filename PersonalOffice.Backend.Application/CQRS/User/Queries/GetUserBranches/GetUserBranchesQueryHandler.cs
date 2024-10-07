using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Contract;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetUserBranches
{
    internal class GetUserBranchesQueryHandler(
        ILogger<GetUserBranchesQueryHandler> logger,
        ITransportService transportService,
        IMapper mapper,
        IContractService contractService) : IRequestHandler<GetUserBranchesQuery, UserBranchesVm>
    {
        private readonly ILogger<GetUserBranchesQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IMapper _mapper = mapper;
        private readonly IContractService _contractService = contractService;

        public async Task<UserBranchesVm> Handle(GetUserBranchesQuery request, CancellationToken cancellationToken)
        {
            var contracts = await _contractService.GetContracts(request.UserId, cancellationToken);

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_UserBranchs",
                Data = request.UserId
            }, cancellationToken);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<IEnumerable<BranchInfo>>>(msg.Data);

            return new UserBranchesVm
            {
                IsPrivilegiesIIS = contracts.Any(x => x.IsIis
                            && DateTime.TryParse(x.EndDate, out DateTime endDate)
                            && (endDate == DateTime.Parse("01.01.2079") || x.DocDate == x.EndDate)),
                Branches = _mapper.Map<IEnumerable<BranchVm>>(sqlResult.ReturnValue)
            };
        }
    }
}
