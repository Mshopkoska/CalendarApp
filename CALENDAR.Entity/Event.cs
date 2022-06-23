using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CALENDAR.Entity
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public ReminderFrequency reminderFrequency { get; set; }

        [Required]
        public int NTimesFrequency { get; set; }

        public DateTime eventReminderDate { get; set; }

        [NotMapped]
        public List<String> Emails { get; set; }

        //Relational data
        [Required]
        public virtual Location Location { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Event(string Name, string Description, DateTime StartTime, DateTime EndTime, Location location, ReminderFrequency reminderFrequency, int NTimesFrequency, string UserId, List<String> emails)
        {
            this.UserId = UserId;
            this.Name = Name;
            this.Description = Description;
            this.StartTime = StartTime;
            this.EndTime = EndTime;

            this.reminderFrequency = reminderFrequency;
            this.NTimesFrequency = NTimesFrequency;

            Location = location;

            this.eventReminderDate = new DateTime();
            this.Emails = emails;
        }

        public void UpdateEvent(string Name, string Description, DateTime StartTime, DateTime EndTime, Location location, ReminderFrequency reminderFrequency, int NTimesFrequency, ApplicationUser user, List<String> emails)
        {
            this.UserId = UserId;
            this.Name = Name;
            this.Description = Description;
            this.StartTime = StartTime;
            this.EndTime = EndTime;

            this.reminderFrequency = reminderFrequency;
            this.NTimesFrequency = NTimesFrequency;

            Location = location;
            this.eventReminderDate = new DateTime();
            this.Emails = emails;
        }

        public Event()
        {
        }
    }

    public enum ReminderFrequency
    {
        Daily = 0,
        Weekly = 1,
        Monthly = 2,
        Yearly = 3
    }
}
