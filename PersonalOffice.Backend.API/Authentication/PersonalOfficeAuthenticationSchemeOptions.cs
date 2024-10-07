using Microsoft.AspNetCore.Authentication;

namespace PersonalOffice.Backend.API.Authentication
{
    /// <summary>
    /// Класс предоставляющий опции для схемы аутентификации
    /// </summary>
    public class PersonalOfficeAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// Наименование схемы аутентификации
        /// </summary>
        public const string NAME = "PersonalOfficeAuthenticationScheme";
    }
}
