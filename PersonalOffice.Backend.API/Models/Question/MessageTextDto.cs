using System.ComponentModel.DataAnnotations;

namespace PersonalOffice.Backend.API.Models.Question
{
    /// <summary>
    /// Модель для передачи текста сообщения
    /// </summary>
    public class MessageTextDto
    {
        /// <summary>
        /// Текст сообщения
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public required string Text {  get; set; }
    }
}
