using System;

namespace SimpleTodoList.MutableCore
{
    public class TodoItemNotFoundException : Exception
    {
        public TodoItemNotFoundException() {}

        public TodoItemNotFoundException(string message)
            : base(message) {}
    }
}