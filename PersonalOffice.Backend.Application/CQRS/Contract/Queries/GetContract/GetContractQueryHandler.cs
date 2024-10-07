using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetContract
{
    internal class GetContractQueryHandler(
        ILogger<GetContractQueryHandler> logger,
        IMapper mapper,
        IContractService contractService) : IRequestHandler<GetContractQuery, ContractVm>
    {
        private readonly ILogger<GetContractQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IContractService _contractService = contractService;

        public async Task<ContractVm> Handle(GetContractQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Получение договоров пользователя");
            var contracts = await _contractService.GetContracts(request.UserId, cancellationToken);

            _logger.LogTrace("Договоры получены");

            return _mapper.Map<ContractVm>(contracts.FirstOrDefault(x => x.Id == request.ContractId) 
                ?? throw new NotFoundException("Договор не найден"));
        }
    }
}
