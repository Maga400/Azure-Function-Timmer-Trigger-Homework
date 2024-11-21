using Azure.Storage.Queues;
using FunctionApp3.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp3.Services.Concretes
{
    public class QueueService : IQueueService
    {
        private readonly QueueClient _queueClient;
        public QueueService(string connectionString, string queueName)
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }
        public async Task<string> ReceiveMessageAsync()
        {
            var messageResponse = await _queueClient.ReceiveMessageAsync();
            if (messageResponse.Value != null)
            {
                var message = messageResponse.Value;
                await _queueClient.DeleteMessageAsync(messageResponse.Value.MessageId, messageResponse.Value.PopReceipt);
                return message.Body.ToString();
            }
            return null;
        }

        public async Task SendMessageAsync(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await _queueClient.SendMessageAsync(message, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(10));
            }
        }
    }
}
