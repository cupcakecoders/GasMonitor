using System;
using System.Linq;

namespace GasMonitor
{
    public class MessageProcessor
    {
        private readonly SQSQueue _sqsQueue;
        private readonly MessageParser _messageParser;
        private readonly LocationChecker _locationChecker;
        private readonly DuplicateChecker _duplicateChecker;

        public MessageProcessor(SQSQueue sqsQueue, MessageParser messageParser, LocationChecker locationChecker, DuplicateChecker duplicateChecker)
        {
            _sqsQueue = sqsQueue;
            _messageParser = messageParser;
            _locationChecker = locationChecker;
            _duplicateChecker = duplicateChecker;
        }

        public void ProcessMessages(string queueUrl)
        {
            var readingMessages = _sqsQueue
                .GetMessagesFromSqsQueue(queueUrl).Result
                .Select(_messageParser.ParseMessage)
                .Where(_locationChecker.FromValidLocation)
                .Where(_duplicateChecker.IsNotDuplicate);

            foreach (var message in readingMessages)
            {
                Console.WriteLine(message.Reading);
                _sqsQueue.DeleteMessageAsync(queueUrl, message.ReceiptHandle).Wait();
            }
        }
    }
}