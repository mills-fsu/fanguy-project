using Lib.CIS4930.Standard.Models;

namespace Server.CIS4930.Database
{
    public static class FakeDB
    {
        public static List<ToDo> Todos = new List<ToDo>
        {
            new ToDo
            {
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
                Name = "appt1",
                Description = "my second desc",
                Start = DateTime.Now.AddHours(2),
                End = DateTime.Now.AddHours(3),
                Attendees = new List<string> { "person1", "person2", "person3" },
            },
        };
    }
}
