namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.General
{
    /// <summary>
    /// Модель представляющая вопрос
    /// </summary>
    /// <param name="IdAnswer">Идентификатор вопроса</param>
    /// <param name="TextAnswerVariant">Текст вопроса</param>
    public record AnswerVariantVm(string IdAnswer, string TextAnswerVariant);
}