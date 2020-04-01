using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Amazon.Runtime.SharedInterfaces;
using Amazon.SimpleNotificationService;
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

        public static Task<SubscribeResponse> SubscribeToSnsTopic(string queryUrl)
        {
            string topicArn = "arn:aws:sns:eu-west-2:099421490492:GasMonitoring-snsTopicSensorDataPart1-1YOM46HA51FB";
            AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient();
            SubscribeRequest subscribeRequest = new SubscribeRequest(topicArn, "https", queryUrl); 
            var snsSubscribeResponse = snsClient.SubscribeAsync(subscribeRequest);
            return snsSubscribeResponse;
        }
        
        //delete the queue
    }
}