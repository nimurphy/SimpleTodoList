using System.Collections.Generic;
using System.Linq;
using SimpleTodoList.MutableCore;
using Xunit;

namespace SimpleTodoList.Service.Tests {

    public class ServiceTests{
        
        ITodoListService Service;

        public ServiceTests(){
            Service = new TodoListService(".\\MockTodoList.data");
        }

        [Fact]
        public void GetListItems_HasAccurateTuples(){
            var listName = "test list";
            Service.CreateList(listName);
            
            var items = new List<(string name, string desc, bool complete)>() {
                ("item 1", "", false),
                ("item 2", "Second item", true)
            };
            
            items.ForEach(item => { 
                Service.AddItem(listName, item.name, item.desc);
                if(item.complete)
                    Service.CompleteItem(listName, item.name);
            });
            
            var retrieved = Service.GetListItems("test list").ToList();
            
            for (int i = 0; i < retrieved.Count; i++){
                var expected = items[i];
                var found = retrieved[i];
                Assert.Equal(expected.name, found.name);
                Assert.Equal(expected.desc, found.description);
                Assert.Equal(expected.complete, found.complete);
            }
                
            Service.RemoveList(listName);
        }

        [Fact]
        public void AddDupicateLists_ThrowsException(){
            var name = "dup_list";
            Service.CreateList(name);
            Assert.Throws<TodoListAlreadyExistsException>(() =>
                Service.CreateList(name));
            Service.RemoveList(name);
        }
    }
}
