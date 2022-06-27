using System;
using CALENDAR.Entity;
using CALENDAR.Data;
using System.Collections.Generic;


namespace CALENDAR.BusinessLogic.EventManagement
{
    public interface IEventManagement
    {

        public void New(string Name, string Description, DateTime startTime, DateTime endTime,
        Location location, ReminderFrequency reminderFrequency, int NTimesFrequency, string UserId, List<string> emails);

        public void Update(string Name, string Description, DateTime startTime, DateTime endTime,
           Location location, ReminderFrequency reminderFrequency, int NTimesFrequency, string UserId, List<string> emails);

        public void Delete(int id);

        public Event GetEvent(int id);

        public List<Event> GetEvents();

        public List<Event> GetUserEvents(string userid);

        public List<Event> GetEventsFromDate(DateTime date, string type);
    }
}
