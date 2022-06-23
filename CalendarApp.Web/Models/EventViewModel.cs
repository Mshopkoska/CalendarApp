using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using CALENDAR.Entity;

namespace CalendarApp.Web.Models
{
    public class EventViewModel
    {
        public Event Event { get; set; }
        public List<SelectListItem> Location = new List<SelectListItem>();
        public string LocationName { get; set; }
        public string UserId { get; set; }

        public ReminderFrequency ReminderFrequency { get; set; }

        public EventViewModel(Event myevent, List<Location> locations, string userid)
        {
            Event = myevent;
            LocationName = myevent.Location.Name;
            UserId = userid;
            foreach (var loc in locations)
            {
                Location.Add(new SelectListItem() { Text = loc.Name });
            }

            ReminderFrequency = myevent.reminderFrequency;
        }

        public EventViewModel(List<Location> locations, string userid)
        {
            UserId = userid;
            foreach (var loc in locations)
            {
                Location.Add(new SelectListItem() { Text = loc.Name });
            }
        }

        public EventViewModel()
        {

        }

    }
}
