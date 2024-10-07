using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Entities.User;

namespace PersonalOffice.Backend.Domain.Entites.User
{
    /// <summary>
    /// Модель пользователя
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользоватлея
        /// </summary>
        [JsonProperty("UserID")]
        public int UserId { get; set; }
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public required string Login { get; set; }
        /// <summary>
        /// Роли пользователя
        /// </summary>
        public ICollection<RoleInfo> Roles { get; set; } = [];
        /// <summary>
        /// Блокировки
        /// </summary>
        public ICollection<BanInfo> Bans { get; set; } = [];
        /// <summary>
        /// Отсутсвуют ли поручения
        /// </summary>
        [JsonProperty("NoOrders")]
        public bool IsNoOrders { get { return Bans.Any(x => x.TypeId == 1); } }
        /// <summary>
        /// Явялется ли пользователь физическим лицом
        /// </summary>
        public bool IsPhysic { get; set; }
        /// <summary>
        /// Явялется ли пользователь резидентом
        /// </summary>
        public bool IsResident { get; set; }
        public string FullName { get; set; }
        public string Telephone { get; set; }
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string Contracts { get; set; }
        [JsonProperty("FirstLogin")]
        public bool IsFirstLogin { get; set; }
        /// <summary>
        /// Подтвержден ли номер телефона
        /// </summary>
        [JsonProperty("PhoneConfirmed")]
        public bool IsPhoneConfirmed { get; set; }
        /// <summary>
        /// Подключена ли двухфакторная аутентификация
        /// </summary>
        [JsonProperty("TwoFactorAuth")]
        public bool IsTwoFactorAuth { get; set; }
        /// <summary>
        /// Составной тип, представляет пути к сертификатам и их идентификаторы
        /// </summary>
        public string? CertPath { get; set; }
        /// <summary>
        /// Список сертификатов
        /// </summary>
        public ICollection<UserCertificateInfo> Certificates { get; set; } = [];
    }
}
