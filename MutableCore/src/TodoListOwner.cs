namespace SimpleTodoList.MutableCore {

    public class TodoListOwner {
        
        public string Name { get; private set; }

        public TodoList TodoList { get; private set; }
        
        public TodoListOwner(string name){
            Name = name;
            TodoList = new TodoList(name);
        }
    }
}