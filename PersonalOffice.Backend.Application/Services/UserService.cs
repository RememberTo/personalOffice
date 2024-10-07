using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotify;
using PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopics;
using PersonalOffice.Backend.Application.Services.Base;
using PersonalOffice.Backend.Domain.Entites.File;
using PersonalOffice.Backend.Domain.Entites.Pagination;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entites.User;
using PersonalOffice.Backend.Domain.Entities.SQL;
using PersonalOffice.Backend.Domain.Entities.User;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Security.Cryptography.X509Certificates;
using Convert = PersonalOffice.Backend.Application.Common.Global.Convert;

namespace PersonalOffice.Backend.Application.Services
{
    internal class UserService(
        IService<UserService> service,
        IDistributedCache cache,
        IMediator mediator,
        IMapper mapper) : IUserService
    {
        private readonly ILogger<UserService> _logger = service.Logger;
        private readonly IDistributedCache _cache = cache;
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ITransportService _transportService = service.TransportService;

        public async Task<string> GetPhoneByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_GetPhoneByUserID",
                Data = userId
            }, cancellationToken).ConfigureAwait(false); ;

            var sqlResult = Convert.DataTo<SQLOperationResult<string>>(msg.Data);

            if (!sqlResult.Success || sqlResult.ReturnValue is null)
            {
                string errorMessage = "Ошибка в UserService.GetPhoneByUserIdAsync : " + sqlResult.Message ?? "Номер телефона не найден";
                throw new NotFoundException(errorMessage);
            }
            return sqlResult.ReturnValue;
        }


        public async Task<decimal> GetTotalPortfolioValueAsync(int userId, CancellationToken cancellationToken)
        {
            var sqlResult = new SQLOperationResult<decimal> { Success = false };
            try
            {
                var msg = await _transportService.RPCServiceAsync(
                    new Message
                    {
                        Source = MicroserviceNames.Backend,
                        Destination = MicroserviceNames.DbConnector,
                        Method = "GetTotalPortfolioValue",
                        Data = userId
                    }
                    , cancellationToken);

                sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<decimal>>(msg.Data);
            }
            catch (Exception ex)
            {
                sqlResult.Message = ex.Message;
            }

            if (!sqlResult.Success || sqlResult.Message is null)
            {
                string errorMessage = "Ошибка в UserService.GetTotalPortfolioValue : "
                    + sqlResult.Message ?? "Неопределенная ошибка";
                throw new InvalidOperationException(errorMessage);
            }
            return sqlResult.ReturnValue;
        }


        //to-do: перенести обработку Exception на MidleWare
        public async Task<bool> CanContactInvestmentConsultantAsync(int userId, CancellationToken cancellationToken)
        {
            var sqlResult = new SQLOperationResult<bool> { Success = false };
            try
            {
                var msg = await _transportService.RPCServiceAsync(
                    new Message
                    {
                        Source = MicroserviceNames.Backend,
                        Destination = MicroserviceNames.DbConnector,
                        Method = "CanContactInvestmentConsultant",
                        Data = userId
                    }
                    , cancellationToken);

                sqlResult = Convert.DataTo<SQLOperationResult<bool>>(msg.Data);
            }
            catch (Exception ex)
            {
                sqlResult.Message = ex.Message;
            }

            if (!sqlResult.Success || sqlResult.Message is null)
            {
                string errorMessage = "Ошибка в UserService.CanContactInvestmentConsultant : "
                    + sqlResult.Message ?? "Неопределенная ошибка";
                throw new InvalidOperationException(errorMessage);
            }
            return sqlResult.ReturnValue;
        }


        public async Task CheckUserTopicAccessAsync(int userId, int topicId, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Проверка доступа к топику: {tid} для пользователя: {uid}", topicId, userId);

            var topics = await _mediator.Send(new GetTopicsQuery { UserId = userId }, cancellationToken);

            if (!topics.Any(x => x.TopicID == topicId))
                throw new UnauthorizedAccessException($"Нет доступа к топику {topicId}");

            _logger.LogTrace("Доступ к топику: {tid} для пользователя: {uid} разрешен", topicId, userId);
        }

        public async Task CheckUserNotifyFilesAccessAsync(int userId, int notifyId, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Проверка доступа к файлам уведомления: {tid} для пользователя: {uid}", notifyId, userId);

            var notifies = await _mediator.Send(new GetNotifyQuery
            {
                UserId = userId,
                PageConfig = new PageConfiguration { PageNumber = 0, PageSize = 0 }
            }, cancellationToken);

            if (!notifies.Notifications.Any(x => x.NotifyID == notifyId))
                throw new UnauthorizedAccessException($"Нет доступа к файлам для уведомления {notifyId}");

            _logger.LogTrace("Доступ к файлам уведомления: {tid} для пользователя: {uid} разрешен", notifyId, userId);
        }

        public async Task<string> GetDocNumAsync(int contractId, int userId, CancellationToken cancellationToken = default)
        {
            var cachedDocNum = await _cache.GetStringAsync(Convert.CachedKey("docnum", contractId, userId), cancellationToken);
            if (!string.IsNullOrEmpty(cachedDocNum)) return cachedDocNum;

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "SF_GetDocNumByContractID",
                Data = new IdRequest { UserId = userId, UniqueId = contractId },
            }, TimeSpan.FromSeconds(5)).ConfigureAwait(false); ;

            var sqlResult = Convert.DataTo<SQLOperationResult<string>>(msg.Data);

            if (!sqlResult.Success)
            {
                _logger.LogError("contractId {id} Ошибка: {sqlerr}", contractId, sqlResult.Message ?? "Номер договора не найден");
                throw new NotFoundException("Номер договора не найден");
            }
            
            var docnum = sqlResult.ReturnValue ?? throw new NotFoundException("Номер договора не найден");

            await _cache.SetStringAsync(Convert.CachedKey("docnum", contractId, userId), docnum, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            }, cancellationToken);

            return docnum;
        }

        public async Task<User> GetGeneralUserInfoAsync(int userId, CancellationToken cancellationToken = default)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "SF_GetUserInfoById",
                Data = userId,
            }, cancellationToken).ConfigureAwait(false);

            var sqlResult = Convert.DataTo<SQLOperationResult<IEnumerable<User>>>(msg.Data);

            if (!sqlResult.Success)
            {
                _logger.LogError("Ошибка: {sqlerr}", sqlResult.Message ?? "Информация по пользователю не найдена");
                throw new NotFoundException("Информация по пользователю не найдена");
            }

            var users = sqlResult.ReturnValue ?? throw new NotFoundException("Информация по пользователю не найдена");

            return users.First(u => u.UserId == userId);
        }

        public async Task AddCertificatesAsync(User user, CancellationToken cancellationToken = default)
        {
            if (user.CertPath is null) return;

            foreach (string info in user.CertPath.Split(';'))
            {
                var cinfo = info.Trim();
                var certId = int.Parse(cinfo.Split('|').First());
                var cpath = cinfo.Split('|').Last();
                var fileResult = Convert.DataTo<FileOperationResult>((await _transportService.RPCServiceAsync(new Message
                {
                    Source = MicroserviceNames.Backend,
                    Destination = MicroserviceNames.FileManager,
                    Method = "ReadFile",
                    Data = new FileOperationParameters() { Name = cpath },
                }, cancellationToken).ConfigureAwait(false)).Data);

                if (fileResult.Success)
                {
                    user.Certificates.Add(new UserCertificateInfo()
                    {
                        Id = certId,
                        Certificate = new X509Certificate2(fileResult.Content)
                    });
                }
            }
        }

        public async Task AddBansAsync(User user, CancellationToken cancellationToken = default)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_GetUserBanInfo",
                Data = new IntRequest { Value = user.UserId },
            }, cancellationToken).ConfigureAwait(false);

            var sqlResult = Convert.DataTo<SQLOperationResult<ICollection<BanInfo>>>(msg.Data);

            if (!sqlResult.Success) throw new NotFoundException(sqlResult.Message ?? "Информация по блокировкам пользователя не найдена");

            user.Bans = sqlResult.ReturnValue ?? [];
        }

        public async Task AddRolesAsync(User user, CancellationToken cancellationToken = default)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_UserRoles",
                Data = new IntRequest { Value = user.UserId },
            }, cancellationToken).ConfigureAwait(false); ;

            var sqlResult = Convert.DataTo<SQLOperationResult<ICollection<RoleInfo>>>(msg.Data);

            if (!sqlResult.Success) throw new NotFoundException(sqlResult.Message ?? "Информация по ролям пользователя не найдена");

            user.Roles = sqlResult.ReturnValue ?? [];
        }

        public async Task<string> GetUserLoginAsync(int userId, CancellationToken cancellationToken = default)
        {
            var cachedLogin = await _cache.GetStringAsync(Convert.CachedKey("user-login", userId), cancellationToken);
            if (!string.IsNullOrEmpty(cachedLogin)) return cachedLogin;

            var user = await GetGeneralUserInfoAsync(userId, cancellationToken);

            await _cache.SetStringAsync(Convert.CachedKey("user-login", userId), user.Login, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
            }, cancellationToken);

            return user.Login;
        }
        
        public async Task ChangeRoleAsync(ChangeRoleRequest changeRoleRequest, CancellationToken cancellationToken = default)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_ChangeRole",
                Data = changeRoleRequest,
            }, cancellationToken).ConfigureAwait(false);

            var sqlResult = Convert.DataTo<SQLOperationResult<object>>(msg.Data);

            if (!sqlResult.Success) throw new InvalidOperationException("Ошибка при изменении роли пользователя:" + sqlResult.Message);
        }

        public async Task<UserProfile> GetUserProfileAsync(int userId, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Начало получения информации о пользователе");
            var user = await GetGeneralUserInfoAsync(userId, cancellationToken);

            _logger.LogTrace("Информация о пользователе получена, начало получения ролей пользователя");

            await AddRolesAsync(user, cancellationToken);
            _logger.LogTrace("Информация о ролях пользователя получена");

            return _mapper.Map<UserProfile>(user);
        }

        public async Task<string> GetPersonCodeAsync(int contractId, CancellationToken cancellationToken = default)
        {
            var cachedPersonCode = await _cache.GetStringAsync(Convert.CachedKey("personCode", contractId), cancellationToken);
            if (!string.IsNullOrEmpty(cachedPersonCode)) return cachedPersonCode;

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "SF_GetPersonCode",
                Data = contractId,
            }, TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            var sqlResult = Convert.DataTo<SQLOperationResult<string>>(msg.Data);

            if (!sqlResult.Success)
            {
                _logger.LogError("Ошибка: {sqlerr}", sqlResult.Message ?? "Код пользователя не найден");
                throw new NotFoundException("Код пользователя не найден");
            }

            var personCode = sqlResult.ReturnValue ?? throw new NotFoundException("Код пользователя не найден");

            await _cache.SetStringAsync(Convert.CachedKey("personCode", contractId), personCode, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(10)
            }, cancellationToken);

            return personCode;
        }
    }
}
