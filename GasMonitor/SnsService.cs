using System;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;

namespace GasMonitor
{
    public class SnsService
    {
        private const string TopicArn = "arn:aws:sns:eu-west-2:099421490492:GasMonitoring-snsTopicSensorDataPart1-1YOM46HA51FB";
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly IAmazonSQS _sqsClient;
        
        public SnsService(IAmazonSimpleNotificationService snsClient, IAmazonSQS sqsClient)
        {
            _snsClient = snsClient;
            _sqsClient = sqsClient;
        }

        public async Task<string> SubscribeToSnsTopic(string queueUrl)
        {
            return await _snsClient.SubscribeQueueAsync(TopicArn, _sqsClient, queueUrl);
        }

        public async Task UnsubscribeToSnsTopic(string subscriptionArn)
        {
            await _snsClient.UnsubscribeAsync(subscriptionArn);

        }
    }
}