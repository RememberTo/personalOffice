using System.Data;

namespace PersonalOffice.Backend.Domain.Entites.TestCFI
{
    /// <summary>
    /// Модель представления результата БД
    /// </summary>
    /// <param name="Tests">Таблица данных</param>
    /// <param name="PageCount">Номер страницы</param>
    public record DbTestsCFI(DataTable Tests, int PageCount);
}
