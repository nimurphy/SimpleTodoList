using System;

namespace SimpleTodoList.Core
{
    public class DoneItem {
        
        private readonly TodoItem _item;
        public TodoItem Item { get => _item; }

        private readonly DateTime _doneDate;
        public DateTime DoneDate { get => _doneDate; }
           
        public DoneItem(TodoItem item){
            _item = item;
        }
    }
}