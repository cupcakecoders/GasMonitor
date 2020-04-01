using System;
using System.Threading.Tasks;
using Amazon.S3;
using System.Runtime;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SQS;

namespace GasMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var s3Client = new AmazonS3Client(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"), Environment.GetEnvironmentVariable("AWS_SECRET_KEY_ID"), RegionEndpoint.EUWest2);
            var sqsClient = new AmazonSQSClient(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"), Environment.GetEnvironmentVariable("AWS_SECRET_KEY_ID"), RegionEndpoint.EUWest2);
            var snsClient = new AmazonSimpleNotificationServiceClient(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"), Environment.GetEnvironmentVariable("AWS_SECRET_KEY_ID"), RegionEndpoint.EUWest2);
            var sqsQueue = new SQSQueue(sqsClient);
            var snsService = new SnsService(snsClient, sqsClient);
            var processor = new MessageProcessor(sqsQueue);
            
            var locationService = LocationService.ReadObjectDataAsync().Result;

            using (var queue = new SubscribedQueue(sqsQueue, snsService))
            {
                var endTime = DateTime.Now.AddMinutes(1);
                while (DateTime.Now < endTime)
                {
                    processor.ProcessMessages(queue.QueueUrl);
                }
            }
            Console.WriteLine("finished.");
        }
    }
}  