using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Question.Commands.SendMessage;
using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Entites.File;
using PersonalOffice.Backend.Domain.Entites.Notify;
using PersonalOffice.Backend.Domain.Entites.OneTimePass;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Text;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Commands.SignTest
{
    internal class SignTestCommandHandler(
        ILogger<SendMessageCommandHandler> logger,
        ITransportService transportService,
        ITestCFIService testCFIService,
        IUserService userService,
        IFileService fileService,
        INotificationService notificationService)
         : IRequestHandler<SignTestCommand, IResult>
    {
        private readonly ILogger<SendMessageCommandHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly ITestCFIService _testCFIService = testCFIService;
        private readonly IUserService _userService = userService;
        private readonly IFileService _fileService = fileService;
        private readonly INotificationService _notificationService = notificationService;

        public async Task<IResult> Handle(SignTestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Отправка запроса на получение номера телефона UserId {uId}", request.UserId);

            var otpResult = await CodeVerefication(await _userService.GetPhoneByUserIdAsync(request.UserId), request.Code);
            
            if (!otpResult.IsSuccess) return new Result(InternalStatus.Error, otpResult.Message);
            _logger.LogInformation("Код верефицирован для пользователя: {uid} код:{cod}", request.UserId, otpResult.Message);

            var result = await SignTestWithCode(request);

            await _notificationService.SendNotificationMessageAsync(new NotificationParameters
            { 
                UserID = request.UserId, 
                Subject = "Результаты тестирования", 
                Message = result.Message 
            });

            return result;
        }

        private async Task<IResult> SignTestWithCode(SignTestCommand request)
        {
            var test = _testCFIService.GetTestById(request.TestId);

            try
            {
                var testObj = _testCFIService.GenerateTestRootForAnswers(test, request.Answers);

                var testPassed = testObj.Knowledge.Questions
                    .Where(question => question.AnswerVariants
                        .First().IdAnswer != question.CorrectAnswer.IdAnswer).Count() != 0
                ? false : true;
                _logger.LogInformation("Тест {} пользователь: {uid}, Название теста: {ntest}", testPassed?"Пройден":"Не пройден", request.UserId, test.FileName);
                
                var xml = _testCFIService.SerializeToXml(testObj, testPassed);

                DateTime testDate = DateTime.Now;
                byte[] content = Encoding.UTF8.GetBytes(xml);
                string testFilePath = $@"i\testCFI\{testDate.Year:0000}_{testDate.Month:00}\{request.UserId}\{testObj.FileName}_"
                                    + $@"{testDate.Year:0000}{testDate.Month:00}{testDate.Day:00}{testDate.Hour:00}{testDate.Minute:00}{testDate.Second:00}.xml";

                await _fileService.WriteFileAsync(new FileOperationParameters { Overwrite = false, Content = content, Name = testFilePath });
                string signTestFilePath = testFilePath.Replace(".xml", ".sig");

                byte[] signature = Crypto.HmacSignature(request.Code, content);
                await _fileService.WriteFileAsync(new FileOperationParameters { Overwrite = false, Content = signature, Name = signTestFilePath });

                await SaveTestToDB(new SaveTestCommand
                {
                    IsSuccess = testPassed,
                    TestTypeID = test.TestId,
                    Sign = Encoding.UTF8.GetString(signature),
                    PathFile = testFilePath,
                    XML = xml,
                    Code = request.Code,
                    PersonID = request.UserId,
                    TestName = test.FileName
                });

                return new Result(testPassed ? InternalStatus.TestPassed :
                    InternalStatus.TestFailed, GenerateMessage(testPassed, test.NameGenitiveCase ?? test.FileName));

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task SaveTestToDB(SaveTestCommand saveTestCommand)
        {
            _logger.LogTrace("Сохранение теста {pthfile} в БД ", saveTestCommand.PathFile);

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_WriteTest",
                Data = saveTestCommand
            }, TimeSpan.FromSeconds(15));

            var SqlResult = Common.Global.Convert.DataTo<SQLOperationResult<string>>(msg.Data);
            
            if(!SqlResult.Success) { throw new InvalidOperationException(SqlResult.Message); }

            _logger.LogTrace("Tест {pthfile} сохранен в БД ", saveTestCommand.PathFile);
        }

        private async Task<OtpOperationResult> CodeVerefication(string phone, string code)
        {
            var otpResult = Common.Global.Convert.DataTo<OtpOperationResult>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.Otp,
                Method = "CheckOtp",
                Data = new CodeVerificationCommand { ID = phone, Otp = code, MarkUsed = false }
            }, TimeSpan.FromSeconds(5))).Data);


            return otpResult ?? throw new OneTimePassException("Не удалось верефицировать код подтверждения");
        }

        private string GenerateMessage(bool testPasssed, string testNameGenetiveCase)
        {
            if (testPasssed)
                return "<p>Настоящим АО ИФК «Солид» уведомляет Вас о положительной оценке результата Вашего тестирования, " +
                       $"проведенного в отношении {testNameGenetiveCase}.</p>" +
                       "<p>Доступ к торговым инструментам выбранной категории будет Вам предоставлен со следующего торгового дня.</p>" +
                       $"<p>{DateTime.Now:dd.MM.yyyy HH:mm:ss}</p>";
            else
                return "<p>Настоящим АО ИФК «Солид» уведомляет Вас об отрицательной оценке результата Вашего тестирования, " +
                       $"проведенного в отношении {testNameGenetiveCase}.</p>" +
                       "<p>Для получения доступа к торговым инструментам выбранной категории пройдите тест повторно. Количество попыток не ограничено.</p>" +
                       $"<p>{DateTime.Now:dd.MM.yyyy HH:mm:ss}</p>";
        }
    }
}