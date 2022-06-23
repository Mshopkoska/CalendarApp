using System;
using CALENDAR.Entity;
using CALENDAR.Data;
using System.Collections.Generic;

namespace CALENDAR.BusinessLogic
{
    public class LocationManagement
    {
        private readonly IDAL _dal;

        public LocationManagement(IDAL dal)
        {
            _dal = dal;
        }

        public void New(string Name, string UserId)
        {
            _dal.CreateLocation(new Location(Name, UserId));
        }

        public Location GetLocationById(int id)
        {
           return _dal.GetLocation(id);
        }

        public Location GetLocationByName(string name)
        {
            return _dal.GetLocation(name);
        }

        public List<Location> GetLocations()
        {
           return _dal.GetLocations();
        }

        public List<Location> GetUserLocations(string userid)
        {
            return _dal.GetMyLocations(userid);
        }
    }
}
