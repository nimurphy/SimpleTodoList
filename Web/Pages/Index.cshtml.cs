using SimpleTodoList.Service;
using SimpleTodoList.MutableCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace Web.Pages {

    public class IndexModel : PageModel {

        private ITodoListService _service;

        [BindProperty]
        public IEnumerable<(string name, string description, bool complete, DateTime lastEventDate, string lastEventType)> 
                Items { get; private set; }

        public string UserName { get; set; }
        
        [TempData]
        public string DisplayMessage { get; set; }

        public IndexModel(ITodoListService service, IHttpContextAccessor httpContextAccessor){
            _service = service;        
            UserName =  httpContextAccessor.HttpContext.User.Identity.IsAuthenticated ? 
                httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value :
                "Unregistered User";
        }

        public IActionResult OnGet(){
            
            try {
                Items = _service.GetListItems(UserName);
            } catch (TodoListNotFoundException){
                _service.CreateList(UserName);
                Items = _service.GetListItems(UserName);
                DisplayMessage = $"Created new list for user {UserName}";
            }
            return Page();
        }

        public IActionResult OnPostCreate(string name, string description){

            try { 
                _service.AddItem(UserName, name, description);
                DisplayMessage = $"Added Todo Item: {name}";
            } catch (Exception ex) when (
                    ex is TodoItemAlreadyExistsException || 
                    ex is ArgumentNullException) {
                DisplayMessage = ex.Message;
            }

            return RedirectToPage();
        }

        public IActionResult OnPostEdit(string id, string name, string description){
            
            try {
                if (id != name)
                    _service.ChangeItemName(UserName, id, name);
                else 
                    _service.ChangeItemDescription(UserName, id, description);
                DisplayMessage = $"Modified TodoItem {id}";
            }  catch (Exception ex) when (
                    ex is TodoItemAlreadyExistsException || 
                    ex is ArgumentNullException) {
                DisplayMessage = ex.Message;
            }
            
            return RedirectToPage();
        }

        public IActionResult OnPostRemove(string id){

            _service.RemoveItem(UserName, id);
            return RedirectToPage();
        }

        public IActionResult OnPostComplete(string id){
        
            _service.CompleteItem(UserName, id);
            return RedirectToPage();
        }
    }
}
