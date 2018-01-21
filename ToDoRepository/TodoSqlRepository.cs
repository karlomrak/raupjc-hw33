using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoRepository
{
    public class TodoSqlRepository : ITodoRepository
    {

        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            
            using (var db =_context)
            {
                TodoItem element = db.TodoItems.Include(i => i.Labels).FirstOrDefault(i => i.Id == todoId);

                if (element == null) return null;

                if (element.UserId != userId) throw new TodoAccessDeniedException();

                return element;
            }
        }

        public void Add(TodoItem todoItem)
        {
            using (var db = _context)
            {
                if (db.TodoItems.FirstOrDefault(i => i.Id==todoItem.Id) != null)
                {
                    throw new DuplicateTodoItemException("Duplicate id : " + todoItem.Id );
                }

                db.TodoItems.Add(todoItem);
                db.SaveChanges();
            }
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            using (var db =_context)
            {
                return db.TodoItems.Include(i=>i.Labels).Where(i=>!i.IsCompleted).Where(i=>i.UserId==userId).ToList();
            }
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            using (var db=_context)
            {
                return db.TodoItems.Include(i => i.Labels).Where(i=>i.UserId==userId).OrderByDescending(i=>i.DateCreated).ToList();
            }
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            using (var db = _context)
            {
                return db.TodoItems.Include(i => i.Labels).Where(i => i.IsCompleted).Where(i=>i.UserId==userId).ToList();
            }
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            using (var db = _context)
            {
                return db.TodoItems.Include(i => i.Labels).AsEnumerable().Where(filterFunction).ToList();
            }
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            using (var db=_context)
            {
                TodoItem element = db.TodoItems.FirstOrDefault(i => i.Id == todoId);

                if (element == null) return false;

                if(element.UserId != userId) throw new TodoAccessDeniedException();

                element.MarkAsCompleted();

                db.Entry(element).State = EntityState.Modified;
                db.SaveChanges();
                
            }
            return true;
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            using (var db=_context)
            {
                TodoItem temp = db.TodoItems.FirstOrDefault(i => i.Id == todoId);

                if (temp == null) return false;

                if(temp.UserId!=userId) throw  new TodoAccessDeniedException();

                db.TodoItems.Remove(temp);
                db.SaveChanges();
            }

            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            using (var db=_context)
            {
                TodoItem temp = db.TodoItems.FirstOrDefault(i => i.Id == todoItem.Id);

                if (temp == null)
                {
                    db.TodoItems.Add(todoItem);
                }

                else
                {
                    if (temp.UserId != userId)
                    {
                        throw new TodoAccessDeniedException();
                    }

                    temp.DateDue = todoItem.DateDue;
                    temp.Text = todoItem.Text;
                    db.Entry(todoItem).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
        }

        public bool AddLabel(string labelText, Guid itemId)
        {
            using (var db = _context)
            {
                TodoItem temp = db.TodoItems.FirstOrDefault(i => i.Id == itemId);

                if (temp == null)
                {
                    return false;
                }

                if (db.Labels.FirstOrDefault(l => l.Value.Equals(labelText.Trim().ToLower())) == null)
                {
                    temp.Labels.Add(new TodoItemLabel(labelText.Trim().ToLower()));
                }

                else
                {
                    temp.Labels.Add(new TodoItemLabel(labelText.Trim().ToLower()));
                    db.Labels.Add(new TodoItemLabel(labelText.Trim().ToLower()));
                }

                db.Entry(temp).State = EntityState.Modified;

                db.SaveChanges();
                return true;
            }
        }
    }

    public class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException(string message) : base(message)
        {
            
        }

        public DuplicateTodoItemException()
        {
            
        }
            
        
    }

    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException(string message) : base(message)
        {

        }

        public TodoAccessDeniedException()
        {

        }
    }
}
