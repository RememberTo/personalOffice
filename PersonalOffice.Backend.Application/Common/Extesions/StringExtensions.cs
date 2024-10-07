namespace PersonalOffice.Backend.Application.Common.Extesions
{
    /// <summary>
    /// Класс расширения для форматирования строк
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Замена сразу нескольких символов
        /// </summary>
        /// <param name="s">заменяемая строка</param>
        /// <param name="replacementCharacters">символы под замену</param>
        /// <param name="replacementCharacter">на какой символ заменить</param>
        /// <returns></returns>
        public static string Replace(this string s, char[] replacementCharacters, char replacementCharacter)
        {
            return replacementCharacters.Aggregate(s, (str, cItem) => str.Replace(cItem, replacementCharacter));
        }
    }
}
