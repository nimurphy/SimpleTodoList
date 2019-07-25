using System;

namespace SimpleTodoList.MutableCore
{
    public class TodoListAlreadyExistsException : Exception
    {
        public TodoListAlreadyExistsException() {}

        public TodoListAlreadyExistsException(string message)
            : base(message) {}

        public TodoListAlreadyExistsException(string message, Exception inner)
            : base(message, inner) {}
    }
}