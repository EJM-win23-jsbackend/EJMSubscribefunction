using Data.Entities;
using EJMSubscribeFunction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EJMSubscribeFunction.Functions
{
    public class GetASubscriber
    {
        private readonly ILogger<GetASubscriber> _logger;
        private readonly SubscribeServices _subscribeServices;

        public GetASubscriber(ILogger<GetASubscriber> logger, SubscribeServices subscribeServices)
        {
            _logger = logger;
            _subscribeServices = subscribeServices;
        }

        [Function("GetASubscriber")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();

                if (!string.IsNullOrEmpty(body))
                {
                    var userId = JsonConvert.DeserializeObject<string>(body);

                    if (userId != null)
                    {
                        var subscriberToGet = await _subscribeServices.GetASubscriberAsync(userId);

                        if (subscriberToGet is OkObjectResult okObject)
                        {
                            return new OkObjectResult(okObject.Value);
                        }

                        return new NotFoundResult();
                    }
                }

                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                _logger.LogError("GetSubscribersFunction::" + ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
