namespace GasMonitor
{
    public class Reading
    {
        public string EventId { get; set; }
        public string LocationId { get; set; }
        public double Value { get; set; }
        public long Timestamp { get; set; }

        public override string ToString()
        {
            return $"EventId: {EventId}, LocationId: {LocationId}, Value: {Value}, Timestamp: {Timestamp}\n";
        }
    }

    public class ReadingMessage
    {
        public string ReceiptHandle { get; set; }
        public Reading Reading { get; set; }
    }
    
}