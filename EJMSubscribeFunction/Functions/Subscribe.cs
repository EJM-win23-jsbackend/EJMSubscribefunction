using EJMSubscribeFunction.Services;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EJMSubscribeFunction.Functions
{
    public class Subscribe
    {
        private readonly ILogger<Subscribe> _logger;
        private readonly SubscribeServices _subscribeServices;

        public Subscribe(ILogger<Subscribe> logger, SubscribeServices subscribeServices)
        {
            _logger = logger;
            _subscribeServices = subscribeServices;
        }

        [Function("Subscribe")]
        public async Task <IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();

                if(!string.IsNullOrEmpty(body))
                {
                    var subscribeEntity = JsonConvert.DeserializeObject<SubscriberEntity>(body);

                    if(string.IsNullOrEmpty(subscribeEntity.Id))
                    {
                        subscribeEntity.Id = Guid.NewGuid().ToString();
                    }

                    if(subscribeEntity != null)
                    {
                        var existingEntity = await _subscribeServices.CheckIfExsitsAsync(subscribeEntity);
                        
                        if(existingEntity is ConflictResult conflict)
                        {
                            return new ConflictObjectResult(new { Status = 409, Message = "This email is already up for subscription" });
                        }

                        if(existingEntity is OkObjectResult okResult)
                        {
                            var entityToUpdate = okResult.Value as SubscriberEntity;
                            var result = _subscribeServices.UpdateAsync(subscribeEntity, entityToUpdate!);
                            return new OkObjectResult(new { Status = 200, Message = "Subscriber was successfully updated!" });
                        }
                        else
                        {
                            var result = await _subscribeServices.CreateSubscriberAsync(subscribeEntity);

                            if (result is OkResult)
                            {
                                return new OkObjectResult(new { Status = 200, Message = "Subscriber was successfully added for subscription!" });
                            }
                        }                   
                    }
                }

                return new BadRequestObjectResult(new { Status = 400, Message = "Malformed request syntax, invalid request message framing, or deceptive request routing" });
            }
            catch (Exception ex)
            {
                _logger.LogError("SubscribeFunction::" + ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
