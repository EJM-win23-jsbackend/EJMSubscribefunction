using EJMSubscribeFunction.Services;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EJMSubscribeFunction.Functions
{
    public class Unsubscribe
    {
        private readonly ILogger<Unsubscribe> _logger;
        private readonly SubscribeServices _subscribeServices;

        public Unsubscribe(ILogger<Unsubscribe> logger, SubscribeServices subscribeServices)
        {
            _logger = logger;
            _subscribeServices = subscribeServices;
        }

        [Function("Unsubscribe")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();

                if (!string.IsNullOrEmpty(body))
                {
                    var subscribeEntity = JsonConvert.DeserializeObject<SubscriberEntity>(body);

                    if (subscribeEntity != null)
                    {
                        var checkIfExist = await _subscribeServices.CheckIfExsitsAsync(subscribeEntity);

                        if (checkIfExist is OkObjectResult okResult)
                        {
                            var entityToDelete = okResult.Value as SubscriberEntity;

                            if (entityToDelete != null) 
                            {
                                var result = await _subscribeServices.DeleteSubscriberAsync(entityToDelete);

                                if(result is OkResult)
                                {
                                    return new OkObjectResult(new { Status = 200, Message = "Subscription was successfully deleted!" });
                                }
                            }
                        }
                        else
                        {
                                return new NotFoundObjectResult(new { Status = 404, Message = "No subscription found" });
                        }
                    }
                }

                return new BadRequestObjectResult( new { Status = 400, Message = "Unable to unsubscribe"  });
            }
            catch (Exception ex)
            {
                _logger.LogError("UnSubscribeFunction::" + ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
