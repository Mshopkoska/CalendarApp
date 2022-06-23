using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CALENDAR.Entity
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string UserId { get; set; }

        //Relational data
        public virtual ICollection<Event> Events { get; set; }

        public Location() { }

        public Location(string Name, string UserId)
        {
            this.Name = Name;
            this.UserId = UserId;
        }
    }
}
