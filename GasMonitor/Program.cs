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
            var sqsClient = new AmazonSQSClient(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"), Environment.GetEnvironmentVariable("AWS_SECRET_KEY_ID"), RegionEndpoint.EUWest2);
            var snsClient = new AmazonSimpleNotificationServiceClient(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"), Environment.GetEnvironmentVariable("AWS_SECRET_KEY_ID"), RegionEndpoint.EUWest2);
            var sqsQueue = new SQSQueue(sqsClient);
            var snsService = new SnsService(snsClient, sqsClient);
            var messageParser = new MessageParser();
            
            var locationService = LocationService.ReadObjectDataAsync().Result;
            
            var locationChecker = new LocationChecker(locationService);
            var duplicateChecker = new DuplicateChecker();

            var processor = new MessageProcessor(sqsQueue, messageParser, locationChecker, duplicateChecker);
            
            
            foreach (var location in locationService)
            {
                Console.WriteLine($"Id:{location.Id}, X:{location.X}, Y:{location.X}");
            }

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