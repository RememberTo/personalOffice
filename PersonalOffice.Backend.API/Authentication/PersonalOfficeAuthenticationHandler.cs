using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.Internal;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Convert = PersonalOffice.Backend.Application.Common.Global.Convert;

namespace PersonalOffice.Backend.API.Authentication
{
    /// <summary>
    /// Обработчик аутентификации
    /// </summary>
    public class PersonalOfficeAuthenticationHandler(
        IOptionsMonitor<PersonalOfficeAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ITransportService transport
        )
        : AuthenticationHandler<PersonalOfficeAuthenticationSchemeOptions>(options, logger, encoder)
    {

        private readonly ITransportService _transportService = transport;
        /// <summary>
        /// Метод обработки аутентификации
        /// </summary>
        /// <returns>Результат аутентификации</returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
#if DEBUG   // Аутентификация в режиме разработки
            var token = Context.Request.Headers.Authorization.ToString();
            token = token.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                var identity = new ClaimsIdentity(PersonalOfficeAuthenticationSchemeOptions.NAME);
                identity.AddClaim(new Claim("UserId", "2339")); //   2339   48092 48206
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return await Task.FromResult(AuthenticateResult.Success(ticket));
            }
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Destination = MicroserviceNames.Authenticator,
                Method = "ValidateJWT",
                Source = MicroserviceNames.Backend,
                Data = token
            }, TimeSpan.FromSeconds(5));

            try
            {
                var result = Convert.DataTo<InternalResult<string>>(msg.Data);
                if (result.Success)
                {
                    var identity = new ClaimsIdentity(PersonalOfficeAuthenticationSchemeOptions.NAME);
                    identity.AddClaim(new Claim("UserId", result.Value ?? throw new InvalidOperationException("UserId is null")));

                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return await Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                {
                    return await Task.FromResult(AuthenticateResult.Fail(result.ErrorMessage ?? "Токен не прошел валидацию"));
                }
            }
            catch (System.Exception)
            {
                return await Task.FromResult(AuthenticateResult.Fail("Authentication failed."));

            }
#else
            var token = Context.Request.Headers.Authorization.ToString();
            token = token.Replace("Bearer ", "");

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Destination = MicroserviceNames.Authenticator,
                Method = "ValidateJWT",
                Source = MicroserviceNames.Backend,
                Data = token
            }, TimeSpan.FromSeconds(5));

            try
            {
                var result = Convert.DataTo<InternalResult<string>>(msg.Data);
                if (result.Success)
                {
                    var identity = new ClaimsIdentity(PersonalOfficeAuthenticationSchemeOptions.NAME);
                    identity.AddClaim(new Claim("UserId", result.Value ?? throw new InvalidOperationException("UserId is null")));

                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return await Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                {
                    return await Task.FromResult(AuthenticateResult.Fail(result.ErrorMessage ?? "Токен не прошел валидацию"));
                }
            }
            catch (System.Exception)
            {
                return await Task.FromResult(AuthenticateResult.Fail("Authentication failed."));

            }
#endif
        }
    }
}
