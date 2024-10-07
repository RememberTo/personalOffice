using PersonalOffice.Backend.Domain.Entites.TestCFI;

namespace PersonalOffice.Backend.Domain.Interfaces.Services
{
    /// <summary>
    /// Интерфейс для работы с данными тестирования неквалов 
    /// </summary>
    public interface ITestCFIService
    {
        /// <summary>
        /// Возвращает объект теста
        /// </summary>
        /// <param name="name">Название теста</param>
        /// <returns></returns>
        public Root? GetTestByName(string? name);
        /// <summary>
        /// Возвращает объект теста
        /// </summary>
        /// <param name="id">Идентификатор теста</param>
        /// <returns></returns>
        public Root GetTestById(int? id);
        /// <summary>
        /// Возвращает вопрос из заданной секции с заданной сложностью
        /// </summary>
        /// <param name="section">Название секции теста</param>
        /// <param name="difficulty">Сложность указывается числом от 1 до 3 (2024)</param>
        /// <returns></returns>
        public Question? GetRandomQuestion(Section? section, int difficulty);

        /// <summary>
        /// Возвращает список вопросов из заданной секции с заданной сложностью
        /// </summary>
        /// <param name="section">Название секции теста</param>
        /// <param name="difficulty">Сложность указывается числом от 1 до 3 (2024)</param>
        /// <param name="countElements">Количество вопросов</param>
        /// <returns></returns>
        public IEnumerable<Question> GetRandomQuestions(Section? section, int difficulty, int countElements);

        /// <summary>
        /// Генерирует xml для последующей записи в БД 
        /// </summary>
        /// <param name="test">Объект теста</param>
        /// <param name="answers">список ответов на тест</param>
        /// <returns></returns>
        public Root GenerateTestRootForAnswers(Root test, IEnumerable<string> answers);

        /// <summary>
        /// Сериализует тест в XML формат
        /// </summary>
        /// <param name="root">Объект теста</param>
        /// <param name="testPassed">Статус пройденного теста</param>
        /// <returns>строка xml</returns>
        public string SerializeToXml(Root? root, bool? testPassed = null);
    }
}
