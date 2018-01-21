using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoRepository;

namespace WebApplication.Models
{
    public class CompletedViewModel
    {
        public List<TodoViewModel> CompletedModels { get; set; }

       public CompletedViewModel(List<TodoItem> todoItemList)
        {
           CompletedModels = new List<TodoViewModel>();

            foreach (var temp in todoItemList)
            {
                CompletedModels.Add(new TodoViewModel(temp));
            }
        }

      
    }
}
