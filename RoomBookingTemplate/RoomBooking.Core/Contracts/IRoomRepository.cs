using System.Collections.Generic;
using System.Threading.Tasks;
using RoomBooking.Core.Entities;

namespace RoomBooking.Core.Contracts
{
  public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync();
    }
}
