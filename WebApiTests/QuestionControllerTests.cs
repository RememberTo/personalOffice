using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalOffice.Backend.API.Controllers;
using PersonalOffice.Backend.API.Models.Question;
using PersonalOffice.Backend.Application.CQRS.Question.Commands.SendMessage;
using PersonalOffice.Backend.Application.CQRS.Question.Queries.GetMessagesFromTopic;
using PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopicById;
using PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopics;
using System.Reflection;
using System.Security.Claims;

namespace WebApiTests
{
    public class QuestionControllerTests
    {
        #region GetTopics

        [Fact]
        public async Task GetTopics_ВозвратOK200()
        {
            // Arrange
            var controller = GetInitializeController("3779");

            // Act
            var result = await controller.GetTopics(10,CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var topics = Assert.IsAssignableFrom<IEnumerable<TopicVm>>(okResult.Value);
            Assert.NotNull(topics);
        }

        [Fact]
        public async Task GetTopics_НекорректныйUserID_Ошибка()
        {
            // Arrange
            var controller = GetInitializeController("test");

            // Act
            try
            {
                var result = await controller.GetTopics(10,CancellationToken.None);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<FormatException>(ex);
            }
        }

        #endregion

        #region PostTopic

        [Fact]
        public async Task CreateTopic_ВалидныеДанные_ВозвратТопикСоздан201()
        {
            // Arrange
            var controller = GetInitializeController("3779");
            var newTopic = new NewTopicDto { Subject = "test0", Text = "test1", TopicTypeCode = "ChatWithManager" };

            // Act
            var result = await controller.CreateTopic(newTopic);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(nameof(NewTopicDto), createdResult.Location);
        }

        #endregion

        [Fact]
        public async Task GetTopicById_ВозвратOK200()
        {
            // Arrange
            
            var topicId = 5315;
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetTopicByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TopicInfoVm());

            var controller = GetInitializeController("3779", mediatorMock);

            // Act
            var result = await controller.GetTopicById(topicId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var topicInfo = Assert.IsAssignableFrom<TopicInfoVm>(okResult.Value);
            Assert.NotNull(topicInfo);
        }

        [Fact]
        public async Task GetMessagesFromTopic_ВозвратOK200()
        {
            // Arrange
            var controller = GetInitializeController("3779");
            var topicId = 5315;

            // Act
            var result = await controller.GetMessagesFromTopic(topicId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var topicMessage = Assert.IsAssignableFrom<IEnumerable<TopicMessageVm>>(okResult.Value);
            Assert.NotNull(topicMessage);
        }

        [Fact]
        public async Task SendMessageForTopic_ВозвратСоздан201()
        {
            // Arrange
            var controller = GetInitializeController("3779");
            var topicId = 5315;
            var longString = "test message";
            var textDto = new MessageTextDto { Text = longString };

            // Act
            var result = await controller.SendMessageForTopic(topicId, textDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(nameof(SendMessageCommand), createdResult.Location);
        }


        private static QuestionController GetInitializeController(string UserId, Mock<IMediator>? mediator = null)
        {
            var loggerMock = new Mock<ILogger<QuestionController>>();
            var mapperMock = new Mock<IMapper>();
            var mediatorMock = mediator ?? new Mock<IMediator>();

            var controller = new QuestionController(loggerMock.Object, mapperMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                        new Claim("UserId", UserId)
                    }, "test"))
                    }
                }
            };

            // через рефлексию
            var mediatorField = controller?.GetType()?.BaseType?.GetField("_mediator", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mediatorField);
            mediatorField.SetValue(controller, mediatorMock.Object);

            ArgumentNullException.ThrowIfNull(controller);

            return controller;
        }
    }
}
