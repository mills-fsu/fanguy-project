using Lib.CIS4930.Standard.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.CIS4930.Database
{
    public class TaskEC
    {
        private DataContext db;

        public TaskEC(DataContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<ITask>> Query(string? searchString)
        {
            List<ITask> tasks = new();

            if (searchString == null)
            {
                await db.ToDos.ForEachAsync(t => tasks.Add(t.Into()));
                await db.Appointments.ForEachAsync(a => tasks.Add(a.Into()));
            }
            else
            {
                await db.ToDos
                    .Where(todo => todo.Name.Contains(searchString) || 
                                   todo.Description.Contains(searchString) ||
                                   todo.Id.Equals(searchString))
                    .ForEachAsync(todo => tasks.Add(todo.Into()));

                await db.Appointments
                    .Where(appt => appt.Name.Contains(searchString) ||
                                   appt.Description.Contains(searchString) ||
                                   appt.Id.Equals(searchString))
                    .ForEachAsync(appt => tasks.Add(appt.Into()));
            }

            return tasks;
        }

        public void AddOrUpdate(ITask newTask)
        {
            if (newTask is ToDo todo)
            {
                
                if (db.ToDos.Where(t => t.Id == todo.Id).Any())
                {
                    var toUpdate = db.ToDos.Where(t => t.Id == todo.Id).First();
                    toUpdate.Update(todo);
                }
                else 
                    db.ToDos.Add(new Models.ToDoModel(todo));
            }
            else if (newTask is Appointment appt)
            {
                if (db.Appointments.Where(t => t.Id == appt.Id).Any())
                {
                    var toUpdate = db.Appointments.Where(t => t.Id == appt.Id).First();
                    toUpdate.Update(appt);
                }
                else
                    db.Appointments.Add(new Models.AppointmentModel(appt));
            }

            db.SaveChangesAsync().Wait();
        }

        public void Delete(ITask oldTask)
        {
            if (oldTask is ToDo todo)
            {
                var toDelete = db.ToDos.Where(t => t.Id == todo.Id).FirstOrDefault();

                if (toDelete != null)
                    db.ToDos.Remove(toDelete);
            }
            else if (oldTask is Appointment appt)
            {
                var toDelete = db.Appointments.Where(a => a.Id == appt.Id).FirstOrDefault();

                if (toDelete != null)
                    db.Appointments.Remove(toDelete);
            }

            db.SaveChangesAsync().Wait();
        }
    }
}
