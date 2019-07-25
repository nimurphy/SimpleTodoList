using Xunit;
using System;

namespace SimpleTodoList.MutableCore.Tests
{
    public class TodoListTests
    {
        string name;
        string desc;
        string newName;
        string newDesc;
        TodoList list;
        
        public TodoListTests(){
            name = "test todo item";
            desc = "this is a test item";
            newName = "My Test Todo Item";
            newDesc = "this has a new description";
            
            list = new TodoList("test");
        }

        [Fact]
        public void AddDuplicateItem_ShouldThrowAlreadyExistsException(){
            var item = new TodoItem(name);
            list.AddItem(item);
            var sameItem = new TodoItem(name);
            Assert.Throws<TodoListAlreadyExistsException>(() => list.AddItem(sameItem));
        }

        [Fact]
        public void RemoveNonExistentItem_ShouldThrowNotFoundException(){
            Assert.Throws<TodoItemNotFoundException>(() => list.RemoveItem("No item"));
        }

        [Fact]
        public void ChangeName_ShouldReplaceOriginal(){
            var item = new TodoItem(name);
            list.AddItem(item);
            list.UpdateItemName(name, newName);

            Assert.Equal(1, list.ItemCount);
            Assert.Equal(newName, list.GetItem(newName).Name);
        }

        [Fact]
        public void ChangeDescription_ShouldReplaceOriginal(){
            var item = new TodoItem(name, desc);
            list.AddItem(item);
            list.UpdateItemDescription(name, newDesc);

            Assert.Equal(1, list.ItemCount);
            Assert.Equal(newDesc, list.GetItem(name).Description);
        }

        [Fact]
        public void ChangeName_ShouldCreateNewTodoEvent(){
            var now = DateTime.Now;
            var item = new TodoItem(name);
            list.AddItem(item);
            list.UpdateItemName(name, newName);
            
            Assert.Equal(EventType.ChangeName, list.GetLastEvent(newName).EventType);
            Assert.True(list.GetLastEvent(newName).EventDate > now);
        }

        [Fact]
        void CompleteItem_ShouldBeComplete(){
            var item = new TodoItem(name);
            list.AddItem(item);
            list.CompleteItem(item.Name);

            Assert.True(list.GetItem(name).IsComplete);
        }
    }
}
