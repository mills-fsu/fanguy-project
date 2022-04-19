using Lib.CIS4930.Standard.Models;

namespace Server.CIS4930.Database.Models
{
    public class ToDoModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Priority { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }

        public ToDoModel()
        {
            Id = Guid.NewGuid();
        }

        public ToDoModel(ToDo todo)
        {
            Update(todo);
        }

        public ToDo Into()
        {
            return new ToDo
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Priority = Priority,
                Deadline = Deadline,
                IsCompleted = IsCompleted
            };
        }

        public void Update(ToDo todo)
        {
            Id = todo.Id;
            Name = todo.Name;
            Description = todo.Description;
            Priority = todo.Priority;
            Deadline = todo.Deadline;
            IsCompleted = todo.IsCompleted;
        }
    }
}
