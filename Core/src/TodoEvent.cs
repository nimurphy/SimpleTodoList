using System;

namespace SimpleTodoList.Core
{
    public enum EventType{
        Create,
        Complete,
        Remove,
        Revert,
        ChangeName,
        ChangeDescription
    }

    public class TodoEvent{
        
        private readonly EventType _eventType;
        public EventType EventType { get => _eventType; }

        private readonly string _value = String.Empty;
        public string Value { get => _value; }

        private readonly DateTime _eventDate;
        public DateTime EventDate { get => _eventDate; }

        public TodoEvent(EventType eventType){
            _eventDate = DateTime.Now;
            _eventType = eventType;
        }

        public TodoEvent(EventType eventType, string value): this(eventType){
            _value = value;
        }
    }
}