using LiteDB;
using SimpleTodoList.MutableCore;
using Xunit;

namespace SimpleTodoList.Service.Tests {

    public class LiteDBTests{
        
        LiteDatabase db;
        BsonMapper mapper = BsonMapper.Global;
        LiteCollection<TodoItem> items;
        LiteCollection<TodoList> lists;

        public LiteDBTests(){
            db = new LiteDatabase(".\\SimpleTodoList.data");
            db.DropCollection("items");
            db.DropCollection("lists");
            
            lists = db.GetCollection<TodoList>("lists");
        }

        [Fact]
        public void TodoItems_ShouldBeRetained(){
            var list = new TodoList("test list");
            list.AddItem(new TodoItem("test item 1"));
            list.AddItem(new TodoItem("test item 2"));
            lists.Insert(list);

            var retrieved = lists.FindOne(x => x.Name == "test list");
            Assert.Equal(2, retrieved.ItemCount);
        }
    }
}
