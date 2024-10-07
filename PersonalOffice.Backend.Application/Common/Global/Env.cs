namespace PersonalOffice.Backend.Application.Common.Global
{
    /// <summary>
    /// Класс предоставляет удобное использование переменных окружения
    /// </summary>
    public static class Env
    {
        /// <summary>
        /// Текущее окружение
        /// </summary>
        public static readonly string? Current = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        /// <summary>
        /// 
        /// </summary>
        public const string Development = "Development";
        /// <summary>
        /// 
        /// </summary>
        public const string Production = "Production";
        /// <summary>
        /// 
        /// </summary>
        public const string Test = "Test";
    }
}
