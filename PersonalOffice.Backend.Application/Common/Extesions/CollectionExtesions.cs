using Newtonsoft.Json;
using PersonalOffice.Backend.Application.Common.Global;
using System;
using System.Drawing;

namespace PersonalOffice.Backend.Application.Common.Extesions
{
    /// <summary>
    /// Класс расширения для коллекций
    /// </summary>
    public static class CollectionExtesions
    {
        /// <summary>
        /// Содержится ли в коллекции элемент
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool In<T>(this T item, params T[] list) => list.Contains(item);

        /// <summary>
        /// Преобразовывает цвет в массив байт
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static byte[] ColorToByteArray(this Color color)
        {
            return [color.R, color.G, color.B, color.A];
        }

        /// <summary>
        /// Преобразует ответ в Exception если не удалось возвращает null
        /// </summary>
        /// <param name="data"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static bool IsException(this object data, out Exception exception)
        {
            var result = GetException(data);

            if(result == null)
            {
                exception = new Exception();
                return false;
            }
            else
            {
                exception = result;
                return true;
            }
        }

        /// <summary>
        /// Преобразует ответ в Exception если не удалось возвращает null
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsException(this object data)
        {
            var result = GetException(data);

            return result != null;
        }

        private static Exception? GetException(object data)
        {
            if (data == null)
            {
                return null;
            }
            try
            {
                var strData = data.ToString();

                if(strData is null)
                    return null;

                return JsonConvert.DeserializeObject<Exception>(strData) ?? throw new Exception();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
