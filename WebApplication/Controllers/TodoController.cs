using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoRepository;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {

        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager
            ;

         public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
         {
             _repository = repository;
             _userManager = userManager;
         }

         public async Task<IActionResult> Index()
         {
             var user = await _userManager.GetUserAsync(HttpContext.User);

             IndexViewModel indexModel = new IndexViewModel(_repository.GetActive(Guid.Parse(user.Id)));

             return View(indexModel);
         }


         public async Task<IActionResult> MarkAsCompleted(Guid itemId)
         {
            var user = await _userManager.GetUserAsync(HttpContext.User);
             _repository.MarkAsCompleted(itemId, new Guid(user.Id));

             return RedirectToAction("Index");
        }

         public IActionResult Add()
         {
             return View();
         }


         [HttpPost]
         public async Task<IActionResult> Add(AddTodoViewModel model)
         {
             if (!ModelState.IsValid)
             {
                 return View(model);
             }
             else
             {
                 var user = await _userManager.GetUserAsync(HttpContext.User);

                 _repository.Add(new TodoItem(model.Text, new Guid(user.Id))
                 {
                     DateDue = model.DateDue
                 });
                // _repository.AddLabel(model.Label.Trim().ToLower(), new Guid(user.Id));
                 return RedirectToAction("Index");
             }
         }

         public async Task<IActionResult> Completed()
         {
             var user = await _userManager.GetUserAsync(HttpContext.User);
             CompletedViewModel completedModel = new CompletedViewModel(_repository.GetCompleted(new Guid(user.Id)));

             return View(completedModel);
         }
         

         public async Task<IActionResult> RemoveFromCompleted(TodoViewModel item)
         {
             var user = await _userManager.GetUserAsync(HttpContext.User);

             _repository.Remove(item.Id, new Guid(user.Id));

             return RedirectToAction("Completed");
        }
         



       
    }
}