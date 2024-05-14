using Data.Entities;
using Moq;
using EJMSubscribeFunction.Services;
using Microsoft.AspNetCore.Mvc;
using Data.Contexts;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Tests.ServiceTests
{
    public class ServiceTests
    {
        [Fact]
        public async Task GetAllSubscribersAsync_ReturnsOkResult_WithSubscribers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Skapa en faktisk instans av DbContext med minnesdatabasalternativ
            using (var dbContext = new DataContext(options))
            {
                dbContext.Subscribers.AddRange(new List<SubscriberEntity>
                {
                    new SubscriberEntity { Email = "Subscriber 1" },
                    new SubscriberEntity { Email = "Subscriber 2" }
                });
                dbContext.SaveChanges();
            }

            // Skapa en faktisk instans av SubscribeServices med DbContext och ILogger
            using (var dbContext = new DataContext(options))
            {
                var service = new SubscribeServices(dbContext);

                // Act
                var result = await service.GetAllSubscribersAsync();

                // Assert
                var okResult = result as OkObjectResult;
                Assert.Equal(200, okResult.StatusCode);
                Assert.NotNull(okResult.Value);
                var subscribers = okResult.Value as List<SubscriberEntity>;
                Assert.Equal(2, subscribers.Count);
            }
        }
    }
}
