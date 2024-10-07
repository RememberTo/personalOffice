using PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.General;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTestById.Vm
{
    /// <summary>
    /// Модель преставления вопроса
    /// </summary>
    public class QuestionVm
    {
        /// <summary>
        /// Номер вопроса
        /// </summary>
        public int NumberQuest { get; set; }
        /// <summary>
        /// Текст вопроса
        /// </summary>
        public string? TextQuest { get; set; }
        /// <summary>
        /// Тип контрола HTML
        /// </summary>
        public string? TypeControl { get; set; }
        /// <summary>
        /// Список вариантов ответа
        /// </summary>
        public required IEnumerable<AnswerVariantVm> AnswerVariants { get; set; }
    }
}