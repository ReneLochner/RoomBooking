using RoomBooking.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomBooking.Core.Contracts
{
  public interface IBookingRepository
  {
    Task<IEnumerable<Booking>> GetAllAsync();

    Task AddRangeAsync(IEnumerable<Booking> bookings);

    Task<IEnumerable<Booking>> GetAllWithRoomsAsync(int id);

    Task<Booking> GetByIdAsync(int id);

    void Delete(Booking booking);

    Task<IEnumerable<Booking>> GetByRoomWithCustomerAsync(int roomId);
  }
}