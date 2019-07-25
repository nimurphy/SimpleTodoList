using System;

namespace SimpleTodoList.MutableCore
{
    public class TodoListNotFoundException : Exception
    {
        public TodoListNotFoundException() {}

        public TodoListNotFoundException(string message)
            : base(message) {}

        public TodoListNotFoundException(string message, Exception inner)
            : base(message, inner) {}
    }
}