using System;
using System.Collections.Generic;
using Amazon.Runtime.Internal;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS.Model;
using Amazon.SQS;
using Newtonsoft.Json;

namespace GasMonitor
{
    public class SQSQueue
    {
        public string sqsitems;

        public string CreateSQSQueue()
        {
            AmazonSQSClient sqsClient = new AmazonSQSClient();
            CreateQueueRequest createQueueRequest = new CreateQueueRequest();
            createQueueRequest.QueueName = "LocationSQSQueue";
            var createQueueResponse = sqsClient.CreateQueueAsync(createQueueRequest);
            var queryResponse = createQueueResponse.Result.QueueUrl;
            return queryResponse;
        }

        public static void ReadSqsItems()
        {
            string topicArn = "arn:aws:sns:eu-west-2:099421490492:GasMonitoring-snsTopicSensorDataPart1-1YOM46HA51FB";
            SubscribeRequest subscribeRequest = new SubscribeRequest(topicArn, "https", "name@example.com");
        }
    }
}