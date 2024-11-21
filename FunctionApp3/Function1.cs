using System;
using FunctionApp3.Services.Abstracts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp3
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IQueueService _queueService;

        public Function1(ILoggerFactory loggerFactory, IQueueService queueService)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _queueService = queueService;
        }

        [Function("Function1")]
        public async Task Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer)
        {
            string message = Guid.NewGuid().ToString();
            await _queueService.SendMessageAsync(message);
            _logger.LogInformation($"Timer Queue Send Value: {message} Time => {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }

        [Function("Function2")]
        public async Task Run2([TimerTrigger("*/15 * * * * *")] TimerInfo myTimer)
        {
            var message = await _queueService.ReceiveMessageAsync();
            _logger.LogInformation($"Timer Queue Receive Value => {message} Time => {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
