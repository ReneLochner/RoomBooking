using Microsoft.EntityFrameworkCore;
using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Persistence
{
  public class BookingRepository : IBookingRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public BookingRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
      => await _dbContext.Bookings.OrderBy(b => b.Room).ThenBy(b => b.From).ToListAsync();

    public async Task AddRangeAsync(IEnumerable<Booking> bookings)
      => await _dbContext.Bookings.AddRangeAsync(bookings);

    public async Task<IEnumerable<Booking>> GetAllWithRoomsAsync(int customerId)
      => await _dbContext.Bookings
          .Include(b => b.Room)
          .Where(b => b.Customer.Id == customerId)
          .OrderBy(b => b.Room.RoomNumber)
          .ThenBy(b => b.From)
          .ToListAsync();

    public async Task<Booking> GetByIdAsync(int id)
      => await _dbContext.Bookings
          .Include(b => b.Customer)
          .Include(b => b.Room)
          .SingleOrDefaultAsync(b => b.Id == id);

    public void Delete(Booking booking)
      => _dbContext.Bookings.Remove(booking);

    public async Task<IEnumerable<Booking>> GetByRoomWithCustomerAsync(int roomId)
      => await _dbContext.Bookings
          .Include(b => b.Customer)
          .Where(b => b.Room.Id == roomId)
          .OrderBy(b => b.From)
          .ToListAsync();
  }
}