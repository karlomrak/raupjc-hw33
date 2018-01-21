using System.Data.Entity;

namespace ToDoRepository
{
    public class TodoDbContext: DbContext
    {

        public IDbSet<TodoItem> TodoItems { get; set; }

        public IDbSet<TodoItemLabel> Labels { get; set; }

        public TodoDbContext(string cnnstr) : base(cnnstr)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>().HasKey(i => i.Id);
            modelBuilder.Entity<TodoItem>().Property(i => i.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.IsCompleted).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.UserId).IsRequired();
           

            modelBuilder.Entity<TodoItemLabel>().HasKey(l => l.Id);
            modelBuilder.Entity<TodoItemLabel>().Property(l => l.Value).IsRequired();

            modelBuilder.Entity<TodoItem>().HasMany(i => i.Labels).WithMany(l => l.LabelTodoItems);


        }
    }
}