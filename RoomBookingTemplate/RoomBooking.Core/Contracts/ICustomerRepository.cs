using RoomBooking.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomBooking.Core.Contracts
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();

        Task<IEnumerable<Customer>> GetAllWithBookingsAndRoomsAsync();

        Task<Customer> GetByIdAsync(int id);

        void Update(Customer customer);
    }
}
