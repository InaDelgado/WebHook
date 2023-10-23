using Microsoft.AspNetCore.Mvc;
using Moq;
using WebHook.Controllers;
using WebHook.Interfaces;

namespace WebHookUnitTest
{
    public class WebHookControllerTest
    {
        [Fact]
        public async void Should_Return_Success_Status_Code_When_Found_Repository_Data()
        {
            #region Arrange

            var receiveMock = new Mock<IReceiveWebhook>();
            var controller = new WebHookController(receiveMock.Object);
            var statusCodeExpected = 200;
            var returnExpected = "Return some string";

            receiveMock
                .Setup(r => r.SendRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("Return some string");

            #endregion

            #region Act

            var result = await controller.GetRepositoryIssues(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            #endregion

            #region Assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(okResult.StatusCode, statusCodeExpected);
            Assert.Equal(okResult.Value, returnExpected);
            Assert.IsAssignableFrom<OkObjectResult>(result);

            #endregion
        }

        [Fact]
        public async void Should_Return_NotFound_Status_Code_When_Repository_Data_NotFound()
        {
            #region Arrange

            var receiveMock = new Mock<IReceiveWebhook>();
            var controller = new WebHookController(receiveMock.Object);
            var statusCodeExpected = 404;
            string? returnMock = null;
            var returnExpected = "GitHub Repository Not Found.";

            receiveMock
                .Setup(r => r.SendRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(returnMock);

            #endregion

            #region Act

            var result = await controller.GetRepositoryIssues(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            #endregion

            #region Assert

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(notFoundResult.StatusCode, statusCodeExpected);
            Assert.Equal(notFoundResult.Value, returnExpected);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);

            #endregion
        }

        [Fact]
        public async void Should_Return_BadRequest_Status_Code_When_Some_Exception_Ocurred()
        {
            #region Arrange

            var receiveMock = new Mock<IReceiveWebhook>();
            var controller = new WebHookController(receiveMock.Object);
            var statusCodeExpected = 400;
            var returnExpected = "Error accessing issues: Value cannot be null. (Parameter 'Some message here')";
            var expection = new ArgumentNullException("Some message here");

            receiveMock
                .Setup(r => r.SendRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(expection);

            #endregion

            #region Act

            var result = await controller.GetRepositoryIssues(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            #endregion

            #region Assert

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badRequestResult.StatusCode, statusCodeExpected);
            Assert.Equal(badRequestResult.Value, returnExpected);
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);

            #endregion
        }
    }
}