using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalOffice.Backend.Application.CQRS.Question.General
{
    /// <summary>
    /// тип топика (темы чата)
    /// </summary>
    public enum QuestionTopicType
    {
        /// <summary>
        /// Лбюбой тип
        /// </summary>
        AllType = 0,
        /// <summary>
        /// Чат с менеджером
        /// </summary>
        ChatWithManager = 1,
        /// <summary>
        /// Чат с инвестиционным консультантом
        /// </summary>
        ChatWithInvestmentAdvisor = 2
    }
}
