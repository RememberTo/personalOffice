using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.API.Models.Auth;
using PersonalOffice.Backend.Application.CQRS.User.Commands;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthController(IMapper mapper) : BaseController
    {
        private readonly IMapper _mapper = mapper;

        //[HttpPost("auth/login")]
        //[AllowAnonymous]
        //public Task<ActionResult<string>> Login()
        //{
        //    //(1)обычная аутентификация, возвращаем токен
        //    //(2)двухфакторная аутентификация, возвращаем просто sucess без токена и сообщение о том что ждем дополнительные данные
        //    //(3)?Создавать специальный токен на 2? минуты который позволяет отправить запрос на подтверждение 
        //    return Ok();
        //}

        //[HttpPost("auth/verify")]
        //[AllowAnonymous]
        //public Task<ActionResult<string>> Confirm2FactorAuthentication()
        //{
        ////    (1)обычная аутентификация,
        ////    (2)двухфакторная аутентификация, возвращаем просто sucess без токена
        //    return Ok();
        //}

        //[HttpPost("auth/access-recovery")]
        //[AllowAnonymous]
        //public Task<IActionResult> AccessRecovery([FromBody] EmailDto emailDto)
        //{
        // Восстановление доступа к аккаунту личного кабинета
        //    var a = emailDto.Email;
        //    return Redirect("http://localhost:5240/swagger/index.html111");
        //}

        /// <summary>
        /// Проверка авторизации
        /// </summary>
        /// <returns></returns>
        [HttpGet("auth/check")]
        public IActionResult Check()
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("auth/refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshAccessToken([FromBody] TokenDto tokenDto)
        {
            var token = _mapper.Map<RefreshAccessTokenCommand>(tokenDto);
            var result = _mapper.Map<TokenDto>(await Mediator.Send(token));
            if(string.IsNullOrEmpty(result.AccessToken)){
                return Unauthorized();
            }
            return Ok(result);
        }

  
    }
}
