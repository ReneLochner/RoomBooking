using System.ComponentModel.DataAnnotations.Schema;

namespace RoomBooking.Core.Entities
{
    public class Booking : EntityObject
    {
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; }
        public int RoomId { get; set; }

        public string From { get; set; }
        public string To { get; set; }

    }
}
