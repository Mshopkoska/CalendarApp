using System;
using CALENDAR.Entity;
using CALENDAR.Data;
using System.Collections.Generic;

namespace CALENDAR.BusinessLogic.EventManagement
{
    public class EventManagement : IEventManagement
    {
        private readonly IDAL _dal;

        public EventManagement(IDAL dal)
        {
            _dal = dal;
        }

        public void New(string Name, string Description, DateTime startTime, DateTime endTime,
            Location location, ReminderFrequency reminderFrequency, int NTimesFrequency, string UserId, List<string> emails)
        {
            Event e = new Event(Name, Description, startTime, endTime, location, reminderFrequency, NTimesFrequency, UserId, emails);
            e.eventReminderDate = CalculateEventReminderDate(reminderFrequency, NTimesFrequency);

            _dal.CreateEvent(e);
        }

        public void Update(string Name, string Description, DateTime startTime, DateTime endTime,
            Location location, ReminderFrequency reminderFrequency, int NTimesFrequency, string UserId, List<string> emails)
        {
            Event e = new Event(Name, Description, startTime, endTime, location, reminderFrequency, NTimesFrequency, UserId, emails);
            e.eventReminderDate = CalculateEventReminderDate(reminderFrequency, NTimesFrequency);

            _dal.UpdateEvent(e);
        }

        public void Delete(int id)
        {
            _dal.DeleteEvent(id);
        }

        public Event GetEvent(int id)
        {
            return _dal.GetEvent(id);
        }

        public List<Event> GetEvents()
        {
            return _dal.GetEvents();
        }

        public List<Event> GetUserEvents(string userid)
        {
            return _dal.GetMyEvents(userid);
        }

        public List<Event> GetEventsFromDate(DateTime date, string type)
        {
            return _dal.GetEventsFromDate(date, type);
        }

        public static DateTime CalculateEventReminderDate(ReminderFrequency reminderFrequency, int NTimesFrequency)
        {
            DateTime reminderDate = DateTime.Now;
            if (reminderFrequency.Equals(ReminderFrequency.Daily)) return reminderDate.AddDays(NTimesFrequency * 1);
            else if (reminderFrequency.Equals(ReminderFrequency.Weekly)) return reminderDate.AddDays(NTimesFrequency * 7);
            else if (reminderFrequency.Equals(ReminderFrequency.Monthly)) return reminderDate.AddMonths(NTimesFrequency * 1);
            else return reminderDate.AddYears(NTimesFrequency * 1);
        }
    }
}
