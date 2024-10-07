using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.API.Models.Auth;
using PersonalOffice.Backend.API.Models.User;
using PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocs;
using PersonalOffice.Backend.Application.CQRS.Security.Queries.GetSignType;
using PersonalOffice.Backend.Application.CQRS.User.Commands.ChangePassword;
using PersonalOffice.Backend.Application.CQRS.User.Commands.UpdateProfile;
using PersonalOffice.Backend.Application.CQRS.User.Queries.GetCertificates;
using PersonalOffice.Backend.Application.CQRS.User.Queries.GetPassThroughData;
using PersonalOffice.Backend.Application.CQRS.User.Queries.GetProfile;
using PersonalOffice.Backend.Application.CQRS.User.Queries.GetUserBranches;
using PersonalOffice.Backend.Application.CQRS.User.Queries.GetUserInfo;
using PersonalOffice.Backend.Domain.Entites.User;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с тестами для неквалифицированных инвесторов
    /// </summary>
    public class UserController(IMapper mapper) : BaseController
    {
        private readonly IMapper _mapper = mapper;


        /// <summary>
        /// Получение информации о пользователе
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/users/me/profile
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Параметры персоны для Front</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(UserInfoVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("users/me/info")]
        public async Task<ActionResult<UserInfoVm>> GetUsersInfo(CancellationToken cancellationToken)
        {
            var person = await Mediator.Send(new GetUserInfoQuery { UserID = base.UserId }, cancellationToken);
            return Ok(person);
        }

        /// <summary>
        /// Получение информации о профиле пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/users/me/profile
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Информация о профиле пользователя</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(UserProfileVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("users/me/profile")]
        public async Task<ActionResult<UserProfileVm>> GetProfile(CancellationToken cancellationToken)
        {
            var person = await Mediator.Send(new GetUserProfileQuery { UserId = base.UserId }, cancellationToken);
            
            return Ok(person);
        }

        /// <summary>
        /// Получение списка сертификатов
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/users/me/certificates
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Список сертификатов пользователя</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<CertificateVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("users/me/certificates")]
        public async Task<ActionResult<IEnumerable<CertificateVm>>> GetCertificateList(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetCertificatesQuery { UserId = base.UserId}, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Получение веток пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/users/me/branches
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Информация о ветках пользователя</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(UserProfile), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("users/me/branches")]
        public async Task<ActionResult<UserProfile>> GetBranches(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetUserBranchesQuery { UserId = base.UserId }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Получение данных для бесшовного перехода
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/users/me/pass-data
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Данные для бесшомного перехода</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(PassThroughDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("users/me/pass-data")]
        public async Task<ActionResult<PassThroughDto>> GetPassThroughData(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetPassThroughDataQuery { UserId = base.UserId }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Получение типа подписи
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/users/me/sign-type
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Тип подписания для пользователя</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(SignTypeVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("users/me/sign-type")]
        public async Task<ActionResult<SignTypeVm>> GetSignType(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetSignTypeQuery { UserId = base.UserId }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Изменение пароля пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST api/users/me/change-password
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Информация о профиле пользователя</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(Domain.Interfaces.Objects.IResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("users/me/change-password")]
        public async Task<ActionResult<UserInfoVm>> ChangePassword(ChangePasswordDto passwordDto, CancellationToken cancellationToken)
        {
            var mappedDto = _mapper.Map<ChangePasswordCommand>(passwordDto);
            mappedDto.UserId = base.UserId;

            var result = await Mediator.Send(mappedDto, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Подключение\отключение двухфакторной аутентификации
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PUT api/users/me/change-password
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Информация о профиле пользователя</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(UserProfile), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPut("users/me/profile")]
        public async Task<ActionResult<UserProfile>> ChangePassword(UserProfileDto profileDto, CancellationToken cancellationToken)
        {
            var mappedDto = _mapper.Map<UserProfile>(profileDto);

            var result = await Mediator.Send(new UpdateUserProfileCommand { UserId = base.UserId, UpdateProfile = mappedDto }, cancellationToken);

            return Ok(result);
        }
    }
}