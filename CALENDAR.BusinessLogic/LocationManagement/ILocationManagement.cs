using System;
using CALENDAR.Entity;
using CALENDAR.Data;
using System.Collections.Generic;

namespace CALENDAR.BusinessLogic.LocationManagement
{
    public interface ILocationManagement
    {
        public void New(string Name, string UserId);
        public Location GetLocationById(int id);

        public Location GetLocationByName(string name);

        public List<Location> GetLocations();

        public List<Location> GetUserLocations(string userid);
    }
}
