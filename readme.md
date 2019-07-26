## Scope of Work

### Functional Requirements
 - Online todo list
 - View a task list
 - Add or remove tasks
	- Allow empty task list
 - Task has description
 -   Task has last modified date
 - Task has created date
 - Task has “checked” state
	- Checked state means task is done
### Non-functional requirements
 - Simple - Implemented in 3 days
 - Implemented in .NET, 3rd party libraries allowed
 - Users must sign in using unique user id & password
 - Changes must be persisted between user sessions
 - Performance must be considered
 - Database must be in-memory
## Approach
### Observations & Assumptions
 - Attention given to certain application characteristics   
	- Architecture
	- Code structure & clarity
	- Performance
	- Security
	- Testing
 - Database is not required to be SQL based
 - Development time is very short
 - Relative to the functional requirements, the non-functional requirements are non-trivial
 - There is no requirement for concurrent access
 - In-memory database should be serverless, i.e. it should not require the installation of a database server
 -   Todo items in single list cannot have same name
 -   Descriptions can be blank
### Initial Approach
 -   Business logic: NET Core library
 -   UI: <span>ASP.NET</span> Core Razor Pages
 -   Persistence: LiteDB serverless database
 -   Testing: XUnit, Powershell, Selenium
##### Bonus objectives
 -   Security: <span>ASP.NET</span> Identity
The approach taken is to use a .NET Core application using Razor Pages. Persistence will be implemeted using LiteDB, a serverless, in-memory document database.
#### Other Approaches
 - <span>ASP.NET</span> MVC. As a UI framework, this would be overly complex for this applicaiton. 
 - Entity Framework. This introduces a level of complexity that is unwarranted. The performance is also poor compared to other ORMs such as Dapper. There is also no reason to use an SQL database.
 - Domain driven design (business logic). The domain complexity does not warrant use of DDD.
 - SPA. Given the scale, development time, and requirement to use .NET, an SPA implementation would not be appropriate.
### Implementation
The application is implemented as a set of 3 libraries, with associated test libraries. 
 - MutableCore. Core business model
 - Service. Repository/persistence. Depends on MutableCore.
 - Web. Web UI. Depends on Service and MutableCore.
#### Model
The model was initially implemented using fully immutable state. Due to difficulty with LiteDB, it was modified to use mutable state. Some design decisions remain.
``` c#
// Create a list and add/remove/complete items
var myList = new TodoList("myList")
myList.AddItem(new TodoItem("item 1"));
myList.AddItem(new TodoItem("item 2", "this is item 2"));
myList.RemoveItem("item 2");
myList.CompleteItem("item 2");

// Changing item details returns a new object reference
var item = new TodoItem("my item");
var newItem = item.ChangeName("another item");
Assert.Equal(item, newItem);	// Fails
// Handled internally in TodoList
myList.ChangeItemName("item", "another Item");

// TodoList internally maintains a separate index of events
var event = new TodoEvent(EventType.Create);
myList.AddItem(new TodoItem("buy fish"));
myList.CompleteItem("buy fish");
Assert.Equal(EventType.Complete, myList.GetLastEvent("buy fish"));	// Passes

// There are two methods for viewing all TodoItems on a TodoList
IEnumerable<TodoItem> items = myList.GetAllItems();
IEnumerable<(TodoItem,TodoEvent)> items = myList.GetAllItemsWithLastEvents();
```
#### Service
The service library provides a repository interface `ITodoListService` to the user interface and to the data store used to persist user data. It also includes an implementation `TodoListService` using LiteDB.
``` c#
// Create a list and add, remove or complete items
var listName = "My list";
var service = new TodoListService(".\MockTodoList.data");
service.CreateList(listName);
service.AddItem(listName, "Item 1");
service.AddItem(listName, "Item 2", "this is item 2");
service.RemoveItem(listName, "Item 1");
service.CompleteItem("Item 2");
service.AddItem(listName, "item 3","Third item");

// Retrieve all items on a list. This returns a tuple 
var items = service.GetAllItems(listName);
foreach (var item in items){
	Console.WriteLine($"Name: {item.name}");
	Console.WriteLine($"Description: {item.description}");
	Console.WriteLine($"Copmlete: {item.complete}");
	Console.WriteLine($"Last event: {item.lastEventDesc} ({item.lastEventDate})\n");
}

/*
Output:
Name: Item 2
Desc: this is item 2
Complete: True
LastEvent: Complete (25/07/2019 13:31:53)

Name: Item 3
Desc: Third item
Complete: False
LastEvent: Complete (25/07/2019 13:33:53)
*/
```
#### Web
This is an ASP.NET Razor Page. There is a single page, `Index`, backed by a PageModel. Along with this there are the scaffolded pages scaffolded by the `aspnet new` utility. All actions are handled within the same page
#### Security
Authentication is done using ASP.NET Identity. Due to time constraints, it was not possible to integrate it with LiteDB. While this was not strictly necessary, it somewhat defeats the point of avoiding the use of a SQL database for persisting model data.
Users can use the application without logging in. However their data will be saved under a common _Unregistered User_ account.
The application requires use of HTTPS. At present it uses a self-signed certificate.
## Installation
The application can be run natively on the dotnet runtime (netcoreapp2.2). 
`dotnet run web.dll`
It can be hosted in IIS using the ASP.NET Core module using in-process hosting. See [here](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-2.2) for instructions on configuring IIS.
Note that the IIS website must be configured with a https binding. As the applicaiton uses a self-signed certificate, all browsers will present a security warning which must be bypassed to use the application.
To build from source, clone the repository and build the Web.csproj project. 
`dotnet build --project .\Web\Web.csproj `
## Review
Overall the application was successful. All requirements were met or addressed. 
The use of LiteDB for data persistence should perform at least as well as SQLite and in theory better than an Entity Framework based implementation. 
The user interface has been tested in Firefox, Chrome and Edge, but not in Internet Explorer.
Below is a brief list of improvements that might be made.
- Exceptions in the model are exposed to the UI.
- Revisit the decision to use tuples instead of data transfer objects. 
- Use of immutable state should be possible using LiteDB and a Builder pattern. This may require writing custom mappings based on LiteDB's `BSonMapper`.
	- The service implementation using LiteDB should be reviewed in any case.
- Incomplete test coverage of `Service` library. No testing of UI.
- More effective use use of XUnit.
- No build / test automation.
- No performance testing.
- More extensive implementation of ASP.NET Identity, ideally using LiteDB.
- Use of self signed certificate.
- Razor Page code could be improved. Ideally based on feedback from somebody more familiar with the framework.
	- Client-side validation.
- More effective use of git. The initial repository was discarded when the excercise was cancelled, and a new one created when it was complete.
- Internal dependencies should be packaged, and ideally published.
- Use of configuration file (using ASP.NET Options library)
 - Configurable data locations
### Code Quality
As much as is possible, the code is idiomatic. It is written to be read. However with the limited, and fragmented time available and the issues with LiteDB, code clarity has suffered. Ideally a more substantial code review would be conducted. This would undoubtedly lead to a certain amount of refactoring for quality and clarity.