namespace PersonalOffice.Backend.Application.Common.Global
{
    /// <summary>
    /// Статический класс для форматирования
    /// </summary>
    public static class Format
    {
        /// <summary>
        /// Получение CN по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetCNFromName(string name)
        {
            return string.Join(" ", name.Split(',').Where(x => x.Contains("CN=")).Select(x => x.Replace("CN=", "")));
        }
    }
}
