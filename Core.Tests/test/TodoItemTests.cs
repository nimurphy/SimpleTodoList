using Xunit;

namespace SimpleTodoList.MutableCore.Tests
{
    public class TodoItemTests
    {
        string name;
        string desc;
        string newName;
        string newDesc;

        public TodoItemTests(){
            name = "test todo item";
            desc = "this is a test item";
            newName = "My Test Todo Item";
            newDesc = "this has a new description";
        }

        [Fact]
        public void ChangeName_ShouldHaveOtherValuesSame(){
            var orig = new TodoItem(name, desc);
            var changedName = orig.ChangeName(newName);
            var changedDesc= changedName.ChangeDescription(newDesc);

            Assert.Equal(newName, changedName.Name);
            Assert.Equal(newDesc, changedDesc.Description);
            Assert.Equal(orig.CreatedDate, changedName.CreatedDate);
        }
    }
}
