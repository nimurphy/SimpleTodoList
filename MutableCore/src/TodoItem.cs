using System;

namespace SimpleTodoList.MutableCore
{
    public class TodoItem {
        
        public string Name { get; private set; }

        public string Description { get; private set; } = String.Empty;

        public DateTime CreatedDate { get; private set; }

        public DateTime? CompletedDate { get; private set; }

        public bool IsComplete { get => CompletedDate.HasValue; }

        public TodoItem(){}
        
        public TodoItem(string name){

            if (name is null)
                throw new ArgumentNullException("Todo Name cannot be null");
                
            Name = name;
            CreatedDate = DateTime.Now;
        }

        public TodoItem(string name, string desc): this(name) {
            Description = desc;
        }

        private TodoItem(string name, DateTime created){
            Name = name;
            CreatedDate = created;
        }

        private TodoItem(string name, string desc, DateTime created): this(name, created){
            Description = desc;
        }

        public TodoItem ChangeName(string name){
            return new TodoItem(name, Description, CreatedDate);
        }

        public TodoItem ChangeDescription(string desc){
            return new TodoItem(Name, desc, CreatedDate);
        }

        public void Complete(){
            CompletedDate = DateTime.Now;
        }
    }
}