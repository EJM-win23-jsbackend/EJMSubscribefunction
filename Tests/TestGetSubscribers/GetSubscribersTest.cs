using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.TestGetSubscribers
{
    //internal class GetSubscribersTest
    //{
    //    [Fact]
    //    public async Task Run_ValidRequest_ReturnsOkResult()
    //    {
    //        // Arrange
    //        var requestBody = "{\"subscribeProperty\":\"value\"}"; // Exempel på giltig begäran
    //        var request = new DefaultHttpContext().Request;
    //        request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody));

    //        var mockSubscribeServices = new Mock<ISubscribeServices>();
    //        mockSubscribeServices.Setup(s => s.CheckIfExsitsAsync(It.IsAny<SubscriberEntity>()))
    //                             .ReturnsAsync(new OkObjectResult(new SubscriberEntity())); // Mocka en befintlig prenumerant

    //        var function = new SubscribeFunction(mockSubscribeServices.Object);

    //        // Act
    //        var response = await function.Run(request);

    //        // Assert
    //        Assert.IsInstanceOf<OkObjectResult>(response);
    //        var result = response as OkObjectResult;
    //        Assert.AreEqual(200, result.StatusCode);
    //        Assert.AreEqual("Subscriber was successfully updated!", result.Value);
    //    }
    //}
}
