using System;

namespace SimpleTodoList.Core
{
    public class TodoItemNotFoundException : Exception
    {
        public TodoItemNotFoundException() {}

        public TodoItemNotFoundException(string message)
            : base(message) {}
    }
}