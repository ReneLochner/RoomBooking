using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RoomBooking.Core.Entities;
using RoomBooking.Web.DataTransferObjects;
using RoomBooking.Web.Validations;

namespace RoomBooking.Web.ViewModels
{
    public class BookingsViewModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public IEnumerable<CustomerDto> Customers { get; set; }
        public int RoomId { get; set; }
        public IEnumerable<Room> Rooms { get; set; }

        [RegularExpression("^(0[8-9]|1[0-7]:[0-5][0-9])|18:00$", ErrorMessage = "Zeit muss zwischen 08:00 und 18:00 liegen")]
        [CorrectTimes]
        //[Remote("CheckTimeBeetween8And18", "Bookings", HttpMethod = "POST", ErrorMessage = "Time has to be beetween 08:00 and 18:00")]
        [Required]
        public String From { get; set; }

        [RegularExpression("^(0[8-9]|1[0-7]:[0-5][0-9])|18:00$", ErrorMessage = "Zeit muss zwischen 08:00 und 18:00 liegen")]
        [CorrectTimes]
        [Required]
        public String To { get; set; }

    }
}
