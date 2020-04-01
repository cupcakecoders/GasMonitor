using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Amazon.SQS;
using Newtonsoft.Json;

namespace GasMonitor
{
    public class SQSQueue
    {
        private readonly IAmazonSQS _sqsClient;
        public SQSQueue(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }
        
        public async Task<string> CreateSqsQueue()
        {
            CreateQueueRequest createQueueRequest = new CreateQueueRequest();
            createQueueRequest.QueueName = "LocationSQSQueue";

            var createQueueResponse = _sqsClient.CreateQueueAsync(createQueueRequest);
            var queueUrl = createQueueResponse.Result.QueueUrl;
            return queueUrl;
        }

        public async Task DeleteSqsQueue(string queueUrl)
        {
            await _sqsClient.DeleteQueueAsync(queueUrl);
        }

        public async Task<IEnumerable<Message>> GetMessagesFromSqsQueue(string queueUrl)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                WaitTimeSeconds = 5,
                MaxNumberOfMessages = 10
            };
            var response = await _sqsClient.ReceiveMessageAsync(request);
            return response.Messages;
        }
        
        public async Task DeleteMessageAsync(string queueUrl, string receiptHandle)
        {
            await _sqsClient.DeleteMessageAsync(queueUrl, receiptHandle);
        }

        public void RecieveMessageFromSQSQueue(Task snsSubscribeResponse, string queryUrl)
        {
            AmazonSQSClient sqsClient = new AmazonSQSClient();
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
            receiveMessageRequest.QueueUrl = queryUrl;
            //ReceiveMessageResponse receiveMessageResponse = 
                sqsClient.ReceiveMessageAsync(receiveMessageRequest);
        }
        
    }
}