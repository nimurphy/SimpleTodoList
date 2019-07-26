using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;
using SimpleTodoList.MutableCore;

namespace SimpleTodoList.Service {

    public class TodoListService: ITodoListService {
        
        LiteDatabase db;
        BsonMapper mapper = BsonMapper.Global;
        LiteCollection<TodoList> lists;
        
        public TodoListService(){
            InitialiseLiteDB(".\\SimpleTodoList.data");
        }

        public TodoListService(string dbLocation){
            InitialiseLiteDB(dbLocation);
        }

        private void InitialiseLiteDB(string dbLocation){
            var dir = System.IO.Path.GetDirectoryName(dbLocation);
            if (!Directory.Exists(dir))
                throw new DirectoryNotFoundException(
                    $"Database directory {dir} does not exist");
            
            db = new LiteDatabase(dbLocation);
            lists = db.GetCollection<TodoList>("lists");
            lists.EnsureIndex(l => l.Name);
        }

        public void CreateList(string name){
            if (lists.FindOne(Query.EQ("Name", name)) is null)
                lists.Insert(new TodoList(name));
            else
                throw new TodoListAlreadyExistsException(
                    $"List with name {name} already exists");            
        }

        public void RemoveList(string name){
            lists.Delete(Query.EQ("Name", name));
        }

        public void AddItem(string listName, string itemName){
            var list = lists.FindOne(Query.EQ("Name", listName));
            if (list is null)
                throw new TodoListNotFoundException($"TodoList {listName} does not exist");
            list.AddItem(new TodoItem(itemName));
            lists.Update(list);
        }
        
        public void AddItem(string listName, string itemName, string itemDesc){
            var list = lists.FindOne(Query.EQ("Name", listName));
            if (list is null)
                throw new TodoListNotFoundException($"TodoList {listName} does not exist");
            list.AddItem(new TodoItem(itemName, itemDesc));
            lists.Update(list);
        }

        public void RemoveItem(string listName, string itemName){
            var list = lists.FindOne(Query.EQ("Name", listName));
            if (list is null)
                throw new TodoListNotFoundException($"TodoList {listName} does not exist");
            list.RemoveItem(itemName);
            lists.Update(list);
        }
        
        public void CompleteItem(string listName, string itemName){
            var list = lists.FindOne(Query.EQ("Name", listName));
            if (list is null)
                throw new TodoListNotFoundException($"TodoList {listName} does not exist");
            list.CompleteItem(itemName);
            lists.Update(list);
        }

        public void ChangeItemName(string listName, string old, string neu){
            var list = lists.FindOne(Query.EQ("Name", listName));
            if (list is null)
                throw new TodoListNotFoundException($"TodoList {listName} does not exist");
            list.UpdateItemName(old, neu);
            lists.Update(list);
        }

        public void ChangeItemDescription(string listName, string old, string neu){
            var list = lists.FindOne(Query.EQ("Name", listName));
            if (list is null)
                throw new TodoListNotFoundException($"TodoList {listName} does not exist");
            list.UpdateItemDescription(old, neu);
            lists.Update(list);
        }

        public IEnumerable<(string name, string description, 
                bool complete, DateTime lastEventDate, string lastEventDesc)>
                GetListItems(string name){

            var list = lists.FindOne(Query.EQ("Name", name));
            if (list is null)
                throw new TodoListNotFoundException($"TodoList {name} does not exist");
            
            return list.GetAllItemsWithLastEvents()
                .Select(l => (l.item.Name, l.item.Description, 
                    l.item.IsComplete, l.lastEvent.EventDate, l.lastEvent.EventType.ToString()));
        }
    }
}

