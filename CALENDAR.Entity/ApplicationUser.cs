using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CALENDAR.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}
