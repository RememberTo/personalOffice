using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Domain.Entites.TestCFI;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Xml.Serialization;

namespace PersonalOffice.Backend.Application.Services
{
    /// <summary>
    /// Реализация получения данных о тестах из XML файлов
    /// </summary>
    public class TestCFIServiceXML : ITestCFIService
    {
        private readonly List<Root> roots;
        private readonly ILogger<TestCFIServiceXML> _logger;

        /// <summary>
        /// Создается список тестов на основе пути к файлам XML
        /// </summary>
        /// <param name="pathToXMLFiles">путь к xml файлам, задается именно директория ОБЯЗАТЕЛЬНО со / в конце, либо через Path.Combine (советую)</param>
        /// <param name="logger"></param>
        public TestCFIServiceXML(string pathToXMLFiles, ILogger<TestCFIServiceXML> logger)
        {
            roots = [];
            _logger = logger;
            var files = Directory.EnumerateFiles(pathToXMLFiles).ToList();
            var serializer = new XmlSerializer(typeof(Root));

            foreach (var file in files)
            {
                using var fs = new FileStream(file, FileMode.Open);

                if (serializer.Deserialize(fs) is Root root)
                {
                    root.FileName = Path.GetFileName(file).Replace(".xml", "");

                    roots.Add(root);
                    _logger.LogInformation("Сount: {ct} | Filename: {fr}", roots.Count, root.FileName);
                }
            }
        }

        /// <summary>
        /// Возвращает объект теста
        /// </summary>
        /// <param name="name">Название теста</param>
        /// <returns></returns>
        public Root GetTestByName(string? name)
        {
            name ??= string.Empty;

            return roots.FirstOrDefault(x => x.FileName == name) ?? throw new NotFoundException($"Тест: {name} не найден");
        }

        /// <summary>
        /// Возвращает объект теста
        /// </summary>
        /// <param name="id">Идентификатор теста</param>
        /// <returns></returns>
        public Root GetTestById(int? id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            return roots.FirstOrDefault(x => x.TestId == id) ?? throw new NotFoundException($"Тест с id: {id} не найден");
        }

        /// <summary>
        /// Возвращает вопрос из заданной секции с заданной сложностью
        /// </summary>
        /// <param name="section">Название секции теста</param>
        /// <param name="difficulty">Сложность указывается числом от 1 до 3 (2024)</param>
        /// <returns></returns>
        public Question? GetRandomQuestion(Section? section, int difficulty)
        {
            ArgumentNullException.ThrowIfNull(nameof(section));

            return section?.Questions?
               .Where(q => q.Difficulty == difficulty)
               .OrderBy(x => Guid.NewGuid()) //рандомная сортировка
               .FirstOrDefault(q => q.Difficulty == difficulty);
        }

        /// <summary>
        /// Возвращает список вопросов из заданной секции с заданной сложностью
        /// </summary>
        /// <param name="section">Название секции теста</param>
        /// <param name="difficulty">Сложность указывается числом от 1 до 3 (2024)</param>
        /// <param name="countElements">Количество вопросов</param>
        /// <returns></returns>
        public IEnumerable<Question> GetRandomQuestions(Section? section, int difficulty, int countElements)
        {
            ArgumentNullException.ThrowIfNull(nameof(section));

            return section?.Questions?
               .Where(q => q.Difficulty == difficulty)
               .OrderBy(x => Guid.NewGuid()) //рандомная сортировка
               .Take(countElements) ?? [];
        }

        /// <summary>
        /// Генерирует новый объект теста, только с выбранными ответами пользователя
        /// </summary>
        /// <param name="test">Объект теста который проходил пользователь</param>
        /// <param name="answers">Варианты ответов пользователя</param>
        /// <returns></returns>
        public Root GenerateTestRootForAnswers(Root test, IEnumerable<string> answers)
        {
            _logger.LogTrace("Начало генерации объекта теста testId: {tid} Название теста: {ntest}", test.TestId, test.FileName);
            var testObj = GetRootTestForAnswers(test, answers);

            if (testObj.Knowledge.Questions.Count != testObj.Knowledge.Count ||
                testObj.SelfEsteem.Questions.Count != testObj.SelfEsteem.Count)
                _logger.LogInformation("Пользователь ответил не на все вопросы");

            //_logger.LogTrace("XML сгенерирован для теста testId: {tid} XML:\n{xml}", test.TestId, xml);
            // Возвращаем XML-представление измененного объекта testClone
            return testObj;
        }

        /// <summary>
        /// Сериализация теста в xml формат
        /// </summary>
        /// <param name="test">Объект теста</param>
        /// <param name="testPassed">Статус пройденного теста</param>
        /// <returns></returns>
        public string SerializeToXml(Root? test, bool? testPassed = null)
        {
            var serializer = new XmlSerializer(typeof(Root));
            using var writer = new StringWriter();

            serializer.Serialize(writer, test);

            var xml = writer.ToString() ?? string.Empty;

            if (testPassed is not null)
                xml = EmbedResult(xml, testPassed.Value);

            xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

            return xml;
        }

        private Root GetRootTestForAnswers(Root test, IEnumerable<string> answers)
        {
            var testClone = test.Clone();

            foreach (var section in new[] { testClone.SelfEsteem, testClone.Knowledge })
            {
                section.Questions.RemoveAll(question =>
                {
                    var answerVariantsToRemove = question.AnswerVariants.Where(answer => !answers.Contains(answer.IdAnswer)).ToList();
                    answerVariantsToRemove.ForEach(answer => question.AnswerVariants.Remove(answer));
                    return answerVariantsToRemove.Count == question.AnswerVariants.Count;
                });

                section.Questions.RemoveAll(question => question.AnswerVariants.Count == 0);
            }

            return testClone;
        }

        private string EmbedResult(string xml, bool testPassed)
        {
            return xml.Insert(xml.IndexOf("</Knowledge>"), $"<TestPassed>{testPassed}</TestPassed>");
        }
    }
}
