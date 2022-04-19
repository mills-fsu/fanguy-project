using Lib.CIS4930.Standard.Models;

namespace Server.CIS4930.Database.Models
{
    public class AppointmentModel
    {    
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Priority { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Attendees { get; set; }

        public AppointmentModel()
        {
            Id = Guid.NewGuid();
        }

        public AppointmentModel(Appointment appt)
        {
            Update(appt);
        }

        public Appointment Into()
        {
            return new Appointment
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Priority = Priority,
                Start = Start,
                End = End,
                Attendees = new List<string>(Attendees.Split(',')),
            };
        }

        public void Update(Appointment appt)
        {
            Id = appt.Id;
            Name = appt.Name;
            Description = appt.Description;
            Priority = appt.Priority;
            Start = appt.Start;
            End = appt.End;
            Attendees = string.Join(",", appt.Attendees);
        }
    }
}
