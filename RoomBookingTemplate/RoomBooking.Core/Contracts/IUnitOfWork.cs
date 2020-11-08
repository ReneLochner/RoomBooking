using System;
using System.Threading.Tasks;

namespace RoomBooking.Core.Contracts
{
  public interface IUnitOfWork : IDisposable
  {
    IBookingRepository Bookings { get; }
    ICustomerRepository Customers { get; }
    IRoomRepository Rooms { get; }

    Task SaveAsync();

    Task DeleteDatabaseAsync();
    Task MigrateDatabaseAsync();
  }
}
