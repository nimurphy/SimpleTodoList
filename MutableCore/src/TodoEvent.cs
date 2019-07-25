using System;

namespace SimpleTodoList.MutableCore
{
    public enum EventType{
        Create,
        Complete,
        Remove,
        Revert,
        ChangeName,
        ChangeDescription
    }

    public class TodoEvent {
        
        public EventType EventType { get; private set; }

        public string Value { get; private set; }

        public DateTime EventDate { get; private set; }
        
        // Public constructor for ORM
        public TodoEvent(){}
        
        public TodoEvent(EventType eventType){
            EventDate = DateTime.Now;
            EventType = eventType;
        }

        public TodoEvent(EventType eventType, string value): this(eventType){
            Value = value;
        }
    }
}