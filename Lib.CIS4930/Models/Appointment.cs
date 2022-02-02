namespace Lib.CIS4930.Models
{
    [Serializable]
    public class Appointment : ITask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<string> Attendees { get; set; }

        public Appointment(string name, string description, DateTime start, DateTime end, List<string> attendees)
        {
            Name = name;
            Description = description;
            Start = start;
            End = end;
            Attendees = attendees;
        }

        public override string ToString()
        {
            // full short date format
            var startTime = Start.ToString("g");
            var endTime = End.ToShortTimeString();
            var participants = string.Join(",", Attendees);
            return $"{Name} [{startTime}-{endTime}]\n   notes: {Description}\n   participants: {participants}";
        }
    }
}
