using System;

namespace SimpleTodoList.MutableCore
{
    public class TodoItemAlreadyExistsException : Exception
    {
        public TodoItemAlreadyExistsException() {}

        public TodoItemAlreadyExistsException(string message)
            : base(message) {}

        public TodoItemAlreadyExistsException(string message, Exception inner)
            : base(message, inner) {}
    }
}