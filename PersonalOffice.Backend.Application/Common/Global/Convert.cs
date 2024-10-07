using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Entities.Document.Elements;
using System.Data;
using System.Text;

namespace PersonalOffice.Backend.Application.Common.Global
{
    /// <summary>
    /// Класс для конвертации различных данных
    /// </summary>
    public class Convert
    {
        /// <summary>
        /// Конвертирует из DataTable в список определенного типа T
        /// </summary>
        /// <typeparam name="T">Возвращаемый тип</typeparam>
        /// <param name="dataTable">Таблица из БД</param>
        /// <returns>Пустой список или IEnumerableT</returns>
        public static IEnumerable<T> DataTableToEnumerable<T>(DataTable dataTable)
        {
            ArgumentNullException.ThrowIfNull(dataTable);

            var jsonTable = JsonConvert.SerializeObject(dataTable);

            if (string.IsNullOrEmpty(jsonTable)) return [];

            return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonTable) ?? [];
        }

        /// <summary>
        /// Конвертирует json в тип T
        /// </summary>
        /// <typeparam name="T">Возвращаемый тип</typeparam>
        /// <param name="data">объект который можно преобразовать в JSON</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Если возвращаемый тип равен null</exception>
        public static T DataTo<T>(object data)
        {
            ArgumentNullException.ThrowIfNull(data);

            return JsonConvert.DeserializeObject<T>(data.ToString() ?? string.Empty)
                ?? throw new InvalidOperationException($"Не удалось десериализовать объект {data} в {typeof(T).FullName}");
        }

        /// <summary>
        /// Конвертирует json в тип T
        /// </summary>
        /// <typeparam name="T">Возвращаемый тип</typeparam>
        /// <param name="json">строка json</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Если возвращаемый тип равен null</exception>
        public static T DataTo<T>(string json)
        {
            ArgumentNullException.ThrowIfNull(json);

            return JsonConvert.DeserializeObject<T>(json ?? string.Empty)
                ?? throw new InvalidOperationException($"Не удалось десериализовать объект {json} в {typeof(T).FullName}");
        }

        /// <summary>
        /// Преобразует набор парметров в ключ для кеширования
        /// </summary>
        /// <param name="title">Название раздела</param>
        /// <param name="uniqueKeys">Уникальные параметры</param>
        /// <returns></returns>
        public static string CachedKey(string title, params object[] uniqueKeys) => $"{title}#{string.Join("-", uniqueKeys)}";

        /// <summary>
        /// Конвертирует кастомный тип DocElement в KeyValuePair
        /// </summary>
        /// <param name="filesHash"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> MapKeyValues(ICollection<DocElement<string, string>> filesHash)
            => filesHash.Select(x => new KeyValuePair<string, string>(x.Key, x.Value));
    }
}
