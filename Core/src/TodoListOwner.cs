namespace SimpleTodoList.Core {
    public class TodoListOwner {
        
        private readonly string _name;
        public string Name { get => _name; }

        private readonly TodoList _todoList;
        public TodoList TodoList { get => _todoList; }
        
        public TodoListOwner(string name){
            _name = name;
            _todoList = new TodoList(name);
        }
    }
}