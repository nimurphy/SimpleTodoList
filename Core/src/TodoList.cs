using System.Collections.Immutable;
using System.Collections.Generic;
using System;

namespace SimpleTodoList.Core {
    public class TodoList {

        private readonly string _name;
        public string Name { get => _name; }

        private ImmutableDictionary<string, TodoItem> _todoItems;
        public IEnumerable<TodoItem> TodoItems { get => _todoItems.Values; }

        private ImmutableDictionary<string, DoneItem> _doneItems;
        public IEnumerable<DoneItem> DoneItems { get => _doneItems.Values; }

        private ImmutableDictionary<string, TodoEvent> _events;

        public int ItemCount { get => _todoItems.Count; }

        public TodoList(string name){
            _name = name;
            _todoItems = ImmutableDictionary.Create<string,TodoItem>();
            _events = ImmutableDictionary.Create<string,TodoEvent>();
        }   

        public void AddItem(TodoItem item){
            try {
                _todoItems = _todoItems.Add(item.Name, item);
                _events = _events.Add(item.Name, 
                    new TodoEvent(EventType.Create));
            } catch (ArgumentException e) {
                throw new TodoItemAlreadyExistsException(
                    $"Another TodoItem named {item.Name} already exists", e);
            }
        }

        public void RemoveItem(string name){
            if (_todoItems.ContainsKey(name)){
                _todoItems = _todoItems.Remove(name);
                _events = _events.SetItem(name, new TodoEvent(EventType.Remove));
            } else
                throw new TodoItemNotFoundException($"TodoItem {name} not found");
        }

        public TodoItem GetItem(string name){
            if (_todoItems.ContainsKey(name))
                return _todoItems[name]; 
            else
                throw new TodoItemNotFoundException($"TodoItem {name} not found");
        }

        public void UpdateItemName(string old, string neu){
            TodoItem orig;
            if (_todoItems.TryGetValue(old, out orig)){
                _todoItems = _todoItems.Remove(old);
                _todoItems = _todoItems.Add(neu, orig.ChangeName(neu));
                _events = _events.Remove(old);
                _events = _events.Add(neu, new TodoEvent(EventType.ChangeName));
            } else
                throw new TodoItemNotFoundException(
                    $"Cannot change name of TodoItem {neu}. Item not found");
        }

        public void UpdateItemDescription(string old, string neu){
            TodoItem orig;
            if (_todoItems.TryGetValue(old, out orig)){
                _todoItems.Remove(old);
                _todoItems.Add(neu, orig.ChangeDescription(neu));
                _events = _events.Remove(old);
                _events = _events.Add(neu, new TodoEvent(EventType.ChangeDescription));
            } else
                throw new TodoItemNotFoundException(
                    $"Cannot change description of TodoItem {neu}. Item not found");
        }

        public TodoEvent GetLastEvent(string name){
            TodoEvent lastEvent;

            if (_events.TryGetValue(name, out lastEvent))
                return lastEvent;
            else
                throw new TodoItemNotFoundException();
        }
    }
}