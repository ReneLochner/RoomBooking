using System.Collections.Generic;

namespace RoomBooking.Core.Entities
{
    public class Room : EntityObject
    {
        public string RoomNumber { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public Room()
        {
            Bookings = new List<Booking>();
        }

    }
}
