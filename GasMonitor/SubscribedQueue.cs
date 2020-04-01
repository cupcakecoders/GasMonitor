using System;

namespace GasMonitor
{
    public class SubscribedQueue : IDisposable
    {
        private readonly SQSQueue _sqsQueue;
        private readonly SnsService _snsService;

        public string QueueUrl { get; }
        private readonly string _subscriptionArn;
        
        public SubscribedQueue(SQSQueue sqsQueue, SnsService snsService)
        {
            _sqsQueue = sqsQueue;
            _snsService = snsService;

            QueueUrl = sqsQueue.CreateSqsQueue().Result;
            _subscriptionArn = _snsService.SubscribeToSnsTopic(QueueUrl).Result;
        }
        
        public void Dispose()
        {
            _snsService.UnsubscribeToSnsTopic(_subscriptionArn).Wait();
            _sqsQueue.DeleteSqsQueue(QueueUrl).Wait();
        }
    }
}

