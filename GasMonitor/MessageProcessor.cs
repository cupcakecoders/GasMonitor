using System;

namespace GasMonitor
{
    public class MessageProcessor
    {
        private readonly SQSQueue _sqsQueue;

        public MessageProcessor(SQSQueue sqsQueue)
        {
            _sqsQueue = sqsQueue;
        }

        public void ProcessMessages(string queueUrl)
        {
            var messages = _sqsQueue.GetMessagesFromSqsQueue(queueUrl).Result;
            foreach (var message in messages)
            {
                Console.WriteLine(message.MessageId);
                _sqsQueue.DeleteMessageAsync(queueUrl, message.ReceiptHandle).Wait();
            }
        }
    }
}