using System.ComponentModel.DataAnnotations;

namespace PersonalOffice.Backend.API.Models.Notify
{
    /// <summary>
    /// Модель на частичное обновление уведомления
    /// </summary>
    public class UpdateMarkNotify
    {
        /// <summary>
        /// Прочитано ли уведомление
        /// </summary>
        [Required]
        public bool IsRead {  get; set; }
    }
}
