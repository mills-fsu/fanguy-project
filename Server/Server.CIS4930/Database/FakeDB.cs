using Lib.CIS4930.Standard.Models;

namespace Server.CIS4930.Database
{
    public static class FakeDB
    {
        public static List<ITask> Tasks
        {
            get
            {
                var l = new List<ITask>();
                Todos.ForEach(t => l.Add(t));
                Appointments.ForEach(t => l.Add(t));
                return l;
            }
        }

        public static List<ToDo> Todos = new List<ToDo>
        {
            new ToDo
            {
                Id = new Guid("04769df5-bcc1-42b3-b758-54398512cec5"),
                Name = "todo1",
                Description = "my first desc",
                Deadline = DateTime.Now.AddDays(1),
                IsCompleted = false,
            },
        };

        public static List<Appointment> Appointments = new List<Appointment>
        {
            new Appointment
            {
                Id = new Guid("bfd73143-afda-4a61-aeaf-d004625d6b31"),
                Name = "appt1",
                Description = "my second desc",
                Start = DateTime.Now.AddHours(2),
                End = DateTime.Now.AddHours(3),
                Attendees = new List<string> { "person1", "person2", "person3" },
            },
        };

        /*public static void Add(ITask task)
        {
            if (task is ToDo todo)
                Todos.Add(todo);
            else if (task is Appointment appt)
                Appointments.Add(appt);
        }

        public static void Update(ITask task)
        {
            if (task is ToDo todo)
            {
                for (int i = 0; i < Todos.Count; ++i)
                {
                    if (Todos[i].Id.Equals(todo.Id))
                    {
                        Todos[i] = todo;
                        break;
                    }
                }
            }
            else if (task is Appointment appt)
            {
                for (int i = 0; i < Appointments.Count; ++i)
                {
                    if (Appointments[i].Id.Equals(appt.Id))
                    {
                        Appointments[i] = appt;
                        break;
                    }
                }
            }
        }*/
    }
}
