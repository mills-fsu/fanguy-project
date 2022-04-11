using Lib.CIS4930.Standard.Models;

namespace Server.CIS4930.Database
{
    public class TaskEC
    {
        public void AddOrUpdate(ITask newTask)
        {
            if (newTask is ToDo todo)
            {
                if (ExistsOn(todo, FakeDB.Todos))
                {
                    // update
                    for (int i = 0; i < FakeDB.Todos.Count; ++i)
                    {
                        if (FakeDB.Todos[i].Id.Equals(todo.Id))
                        {
                            FakeDB.Todos[i] = todo;
                            break;
                        }
                    }
                }
                else
                {
                    // add
                    FakeDB.Todos.Add(todo);
                }
            }
            else if (newTask is Appointment appt)
            {
                if (ExistsOn(appt, FakeDB.Appointments))
                {
                    // update
                    for (int i = 0; i < FakeDB.Appointments.Count; ++i)
                    {
                        if (FakeDB.Appointments[i].Id.Equals(appt.Id))
                        {
                            FakeDB.Appointments[i] = appt;
                            break;
                        }
                    }
                }
                else
                {
                    // add
                    FakeDB.Appointments.Add(appt);
                }
            }
        }

        public void Delete(ITask oldTask)
        {
            if (oldTask is ToDo todo)
            {
                for (int i = 0; i < FakeDB.Todos.Count; ++i)
                {
                    if (FakeDB.Todos[i].Id.Equals(todo.Id))
                    {
                        FakeDB.Todos.RemoveAt(i);
                        break;
                    }
                }
            }
            else if (oldTask is Appointment appt)
            {
                for (int i = 0; i < FakeDB.Appointments.Count; ++i)
                {
                    if (FakeDB.Appointments[i].Id.Equals(appt.Id))
                    {
                        FakeDB.Appointments.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private bool ExistsOn(ITask check, IEnumerable<ITask> container)
        {
            return container
                    .Where(item => item.Id.Equals(check.Id))
                    .Any();
        }
    }
}
