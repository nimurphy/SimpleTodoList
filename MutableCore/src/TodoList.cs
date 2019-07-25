using System.Collections.Generic;
using System;
using System.Linq;

namespace SimpleTodoList.MutableCore {
    public class TodoList {

        public Guid Id { get; private set;}
        public string Name { get; private set; }

        public Dictionary<string, TodoItem> TodoItems {get; private set;}

        public Dictionary<string, TodoEvent> LastEvents { get; private set; }

        public int ItemCount { get => TodoItems.Count; }

        public TodoList(){}
        
        public TodoList(string name){
            Name = name;
            TodoItems = new Dictionary<string,TodoItem>();
            LastEvents = new Dictionary<string,TodoEvent>();
        }   

        public void AddItem(TodoItem item){
            try {
                TodoItems.Add(item.Name, item);
                LastEvents.Add(item.Name, 
                    new TodoEvent(EventType.Create));
            } catch (ArgumentException ex) {
                throw new TodoItemAlreadyExistsException(
                    $"Another TodoItem named {item.Name} already exists", ex);
            }
        }

        public void RemoveItem(string name){
            if (TodoItems.ContainsKey(name)){
                TodoItems.Remove(name);
                LastEvents[name] = new TodoEvent(EventType.Remove);
            } else
                throw new TodoItemNotFoundException($"TodoItem {name} not found");
        }

        public TodoItem GetItem(string name){
            if (TodoItems.ContainsKey(name))
                return TodoItems[name]; 
            else
                throw new TodoItemNotFoundException($"TodoItem {name} not found");
        }

        public IEnumerable<TodoItem> GetAllItems(){   
            return TodoItems.Values; 
        }

        public IEnumerable<(TodoItem item, TodoEvent lastEvent)> GetAllItemsWithLastEvents(){

            return TodoItems.Select(t => (t.Value, LastEvents[t.Key]));
        }
        
        public void UpdateItemName(string old, string neu){
            TodoItem orig;
            if (TodoItems.TryGetValue(old, out orig)){
                TodoItems.Remove(old);
                TodoItems.Add(neu, orig.ChangeName(neu));
                LastEvents.Remove(old);
                LastEvents.Add(neu, new TodoEvent(EventType.ChangeName));
            } else
                throw new TodoItemNotFoundException(
                    $"Cannot change name of TodoItem {neu}. Item not found");
        }

        public void UpdateItemDescription(string name, string desc){
            TodoItem orig;
            if (TodoItems.TryGetValue(name, out orig)){
                TodoItems[name] = orig.ChangeDescription(desc);
                LastEvents[name] = new TodoEvent(EventType.ChangeDescription);
            } else
                throw new TodoItemNotFoundException(
                    $"Cannot change description of TodoItem {name}. Item not found");
        }

        public void CompleteItem(string name){
            TodoItem item;
            if (TodoItems.TryGetValue(name, out item)){
                item.Complete();
                LastEvents[name] = new TodoEvent(EventType.Complete);
            } else
                throw new TodoItemNotFoundException(
                    $"Cannot complete TodoItem {name}. Item not found");
        }
        public TodoEvent GetLastEvent(string name){
            TodoEvent lastEvent;

            if (LastEvents.TryGetValue(name, out lastEvent))
                return lastEvent;
            else
                throw new TodoItemNotFoundException();
        }
    }
}