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
            Event e = new Event(Name, Description, startTime, endTime, location, reminderFrequency, NTimesFrequency, UserId, emails)
            {
                eventReminderDate = CalculateEventReminderDate(reminderFrequency, NTimesFrequency)
            };

            _dal.CreateEvent(e);
        }

        public void Update(string Name, string Description, DateTime startTime, DateTime endTime,
            Location location, ReminderFrequency reminderFrequency, int NTimesFrequency, string UserId, List<string> emails)
        {
            Event e = new Event(Name, Description, startTime, endTime, location, reminderFrequency, NTimesFrequency, UserId, emails)
            {
                eventReminderDate = CalculateEventReminderDate(reminderFrequency, NTimesFrequency)
            };

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

        public List<Event> GetEventsFromDate(DateTime date, EmailType type)
        {
            return _dal.GetEventsFromDate(date, type);
        }

        /// <summary>
        /// This method calculates the next reminder date based on the event reminder frequency
        /// </summary>
        /// <param name="reminderFrequency"></param>
        /// <param name="NTimesFrequency"></param>
        /// <returns></returns>
        public static DateTime CalculateEventReminderDate(ReminderFrequency reminderFrequency, int nTimesFrequency)
        {
            DateTime reminderDate = DateTime.Now;

            switch (reminderFrequency)
            {
                case ReminderFrequency.Daily:
                    reminderDate = reminderDate.AddDays(nTimesFrequency);
                    break;
                case ReminderFrequency.Weekly:
                    reminderDate = reminderDate.AddDays(nTimesFrequency * 7);
                    break;
                case ReminderFrequency.Monthly:
                    reminderDate = reminderDate.AddMonths(nTimesFrequency);
                    break;
                case ReminderFrequency.Yearly:
                    reminderDate = reminderDate.AddYears(nTimesFrequency);
                    break;
            }

            return reminderDate;
        }
    }
}
