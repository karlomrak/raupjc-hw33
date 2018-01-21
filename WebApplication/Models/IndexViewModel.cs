using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoRepository;

namespace WebApplication.Models
{
    public class IndexViewModel
    {
       public List<TodoViewModel> TodoViewModels {get; set; }


        public IndexViewModel(List<TodoItem> todoItemList)
        {
            TodoViewModels=new List<TodoViewModel>();

            foreach(var temp in todoItemList)
            {
             TodoViewModels.Add(new TodoViewModel(temp) );   
            }
        }

       
    }
}
