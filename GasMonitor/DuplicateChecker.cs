using System;
using System.Collections.Generic;
using System.Linq;

namespace GasMonitor
{
    public class DuplicateChecker
    {
        private readonly EventIdStore _previousEventIds = new EventIdStore();

        public bool IsNotDuplicate(ReadingMessage readingMessage)
        {
            var eventId = readingMessage.Reading.EventId;

            if (_previousEventIds.Contains(eventId))
            {
                return false;
            }

            _previousEventIds.Add(eventId);
            return true;
        }
    }

    public class EventIdStore
    {
        private readonly Queue<StoreItem> _storeItems = new Queue<StoreItem>();

        public bool Contains(string eventId)
        {
            RemoveExpiredItems();
            return _storeItems.Any(item => item.Value == eventId);
        }

        private void RemoveExpiredItems()
        {
            while (_storeItems.Any() && _storeItems.Peek().HasExpired())
            {
                _storeItems.Dequeue();
            }
        }

        public void Add(string eventId)
        {
            _storeItems.Append(new StoreItem
            {
                Value = eventId,
                ExpiryTime = DateTime.Now.AddMinutes(6)
            });
        }
    }

    public class StoreItem
    {
        public string Value { get; set; }
        public DateTime ExpiryTime { get; set; }

        public bool HasExpired()
        {
            return DateTime.Now > ExpiryTime;
        }
    }
}
