using System;
using System.Collections.Generic;
using System.Linq;
using CALENDAR.Entity;
using CALENDAR.Migrations;

namespace CALENDAR.Data
{
    public interface IDAL
    {
        public List<Event> GetEvents();
        public List<Event> GetMyEvents(string userid);
        public List<Event> GetEventsFromDate(DateTime date, string type);
        public Event GetEvent(int id);
        public void CreateEvent(Event newevent);
        public void UpdateEvent(Event newevent);
        public void DeleteEvent(int id);
        public List<Location> GetLocations();
        public List<Location> GetMyLocations(string userid);
        public Location GetLocation(int id);

        public Location GetLocation(string Name);
        public void CreateLocation(Location location);
    }

    public class DAL : IDAL
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Event> GetEvents()
        {
            return db.Events.ToList();
        }

        public List<Event> GetMyEvents(string userid)
        {
            return db.Events.Where(x => x.UserId == userid).ToList();
        }

        public Event GetEvent(int id)
        {
            return db.Events.FirstOrDefault(x => x.Id == id);
        }

        public void CreateEvent(Event newevent)
        {
            db.Events.Add(newevent);
            db.SaveChanges();
        }

        public void UpdateEvent(Event newevent)
        {
            db.Entry(newevent).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteEvent(int id)
        {
            var myevent = db.Events.Find(id);
            db.Events.Remove(myevent);
            db.SaveChanges();
        }

        public List<Location> GetMyLocations(string userid)
        {

            List<Location> list = db.Locations.Where(x => x.UserId == userid).ToList();
            return list;
        }

        public List<Location> GetLocations()
        {
            return db.Locations.ToList();
        }

        public Location GetLocation(int id)
        {
            return db.Locations.Find(id);
        }

        public void CreateLocation(Location location)
        {
            db.Locations.Add(location);
            db.SaveChanges();
        }

        public Location GetLocation(string Name)
        {
            return db.Locations.Where(x => x.Name == Name).FirstOrDefault();
        }

        public List<Event> GetEventsFromDate(DateTime date, string type)
        {
            if (type == "reminder")
            {
                return db.Events.Where(x => x.eventReminderDate == date).ToList();
            }
            else return db.Events.Where(x => x.StartTime == date).ToList();
        }

    }
}
