using RoomBooking.Core.Entities;

namespace RoomBooking.Web.DataTransferObjects
{
    public class CustomerDto
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public CustomerDto(Customer customer)
        {
            Id = customer.Id;
            Name = $"{customer.LastName} {customer.FirstName}";
        }
    }
}
