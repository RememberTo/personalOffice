using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PersonalOffice.Backend.API.Models.Auth
{
    /// <summary>
    /// Представляет контракт, который содержит email пользователя
    /// </summary>
    public class EmailDto
    {
        /// <summary>
        /// Адресс электронной почты пользователя
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле Email обязательное")]
        [EmailAddress]
        [NotNull]
        public required string Email { get; set; }
    }
}
