using System.Collections.Generic;
using RoomBooking.Core.Entities;
using RoomBooking.Web.DataTransferObjects;

namespace RoomBooking.Web.ViewModels
{
    public class HomeIndexViewModel
    {
        public HomeIndexViewModel()
        {
        }

        public HomeIndexViewModel(List<Customer> customers, int customerId, List<Booking> bookings) 
        {
            Customers = new List<CustomerDto>();
            foreach (var customer in customers)
            {
                Customers.Add(new CustomerDto(customer));
            }
            CustomerId = customerId;
            Bookings = bookings;
        }

        public List<CustomerDto> Customers { get; set; }  // Customers in der Combobox
        public IEnumerable<Booking> Bookings { get; set; }    // Buchungen des ausgewählten Customers

        public int CustomerId { get; set; }

    }
}
