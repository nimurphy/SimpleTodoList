using System;

namespace SimpleTodoList.Core
{
    public class TodoItem {
        
        private readonly string _name;
        public string Name { get => _name; }

        private readonly string _description = String.Empty;
        public string Description { get => _description; }

        private DateTime _createdDate;
        public DateTime CreatedDate { get => _createdDate; }

        public TodoItem(){
            _name = "test";
        }

        public TodoItem(string name){
            _name = name;
            _createdDate = DateTime.Now;
        }

        public TodoItem(string name, string desc): this(name) {
            _description = desc;
        }

        private TodoItem(string name, DateTime created){
            _name = name;
            _createdDate = created;
        }

        private TodoItem(string name, string desc, DateTime created): this(name, created){
            _description = desc;
        }

        public TodoItem ChangeName(string name){
            return new TodoItem(name, _description, _createdDate);
        }

        public TodoItem ChangeDescription(string desc){
            return new TodoItem(_name, desc, _createdDate);
        }
    }
}