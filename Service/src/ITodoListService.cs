using System.Collections.Generic;
using System;

namespace SimpleTodoList.Service {

    public interface ITodoListService {

        void CreateList(string name);

        void RemoveList(string name);

        void AddItem(string listName, string itemName);

        void AddItem(string listName, string itemName, string itemDesc);

        void RemoveItem(string listName, string name);

        void CompleteItem(string owner, string item);

        void ChangeItemName(string owner, string old, string neu);

        void ChangeItemDescription(string owner, string old, string neu);
        
        IEnumerable<(string name, string description, bool complete, DateTime lastEventDate, string lastEventDesc)> 
                GetListItems(string owner);
    }
}

