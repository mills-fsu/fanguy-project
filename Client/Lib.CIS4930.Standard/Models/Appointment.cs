using Lib.CIS4930.Standard.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.CIS4930.Standard.Models
{
    [JsonConverter(typeof(TaskJsonConverter))]
    [Serializable]
    public class Appointment : ITask
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<string> Attendees { get; set; }

        public Appointment()
        {
            Id = Guid.NewGuid();
            Name = "";
            Description = "";
            Start = DateTime.Now;
            End = DateTime.Now;
            Attendees = new List<string>();
        }

        public Appointment(string name, string description, DateTime start, DateTime end, List<string> attendees)
        {
            Id = Guid.NewGuid();
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

            StringBuilder stars = new StringBuilder();
            for (int i = 0; i < Priority && i < 5; i++)
            {
                stars.Append("⭐");
            }

            return $"{Name} [{startTime}-{endTime}] {stars}\n   notes: {Description}\n   participants: {participants}";
        }
    }
}
