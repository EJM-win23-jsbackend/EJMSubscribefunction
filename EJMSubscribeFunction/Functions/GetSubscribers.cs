using Data.Entities;
using EJMSubscribeFunction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EJMSubscribeFunction.Functions
{
    public class GetSubscribers
    {
        private readonly ILogger<GetSubscribers> _logger;
        private readonly SubscribeServices _subscribeServices;

        public GetSubscribers(ILogger<GetSubscribers> logger, SubscribeServices subscribeServices)
        {
            _logger = logger;
            _subscribeServices = subscribeServices;
        }

        [Function("GetSubscribers")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();

                if (!string.IsNullOrEmpty(body))
                {
                     var subscribeEntity = JsonConvert.DeserializeObject<string>(body);

                     if (subscribeEntity != null)
                     {
                         var subscriberToGet = await _subscribeServices.GetASubscriberAsync(subscribeEntity);

                         if (subscriberToGet != null)
                         {
                             return new OkObjectResult(subscriberToGet);
                         }
                     }
                }

                var result = await _subscribeServices.GetAllSubscribersAsync();

                if (result is OkObjectResult okResult && okResult != null)
                {
                    List<SubscriberEntity> subs = okResult.Value as List<SubscriberEntity>;

                    return new OkObjectResult(subs);
                }

                return new BadRequestObjectResult(new { Status = 400, Message = "Malformed request syntax, invalid request message framing, or deceptive request routing" });
            }
            catch (Exception ex)
            {
                _logger.LogError("GetSubscribersFunction::" + ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
