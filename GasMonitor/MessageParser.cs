using Newtonsoft.Json;
using Amazon.SQS.Model;
using SnsMessage = Amazon.SimpleNotificationService.Util.Message;

namespace GasMonitor
{
    public class MessageParser
    {
        public ReadingMessage ParseMessage(Message message)
        {
            var snsMessage = SnsMessage.ParseMessage(message.Body);
            var reading = JsonConvert.DeserializeObject<Reading>(snsMessage.MessageText);
            return new ReadingMessage
            {
                ReceiptHandle = message.ReceiptHandle,
                Reading = reading
            };
        }
    }
}