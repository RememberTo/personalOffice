using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocs
{
    /// <summary>
    /// Модель представления документа
    /// </summary>
    public class DocumentInfoVm
    {
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        public required string Id { get; set; }
        /// <summary>
        /// Идентификатор связанного документа
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public required string ContractId { get; set; }
        /// <summary>
        /// Номер договора
        /// </summary>
        public required string ContractNum { get; set; }
        /// <summary>
        /// Идентификатор статуса
        /// </summary>
        public int StatusId { get; set; }
        /// <summary>
        /// Наименование статуса
        /// </summary>
        public required string Status { get; set; }
        /// <summary>
        /// Исходящий или Входящий документ
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Направление
        /// </summary>
        public byte Direction { get; set; }
        /// <summary>
        /// Есть ли подпись клиента
        /// </summary>
        public bool NeedClientSign { get; set; }
        /// <summary>
        /// Есть ли подпись компании
        /// </summary>
        public bool NeedCompanySign { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Подпись клиента
        /// </summary>
        public int ClientSign { get; set; }
        /// <summary>
        /// Подпись компании
        /// </summary>
        public int CompanySign { get; set; }
        /// <summary>
        /// Количетсво файлов
        /// </summary>
        public int FileCount { get; set; }
    }
}
