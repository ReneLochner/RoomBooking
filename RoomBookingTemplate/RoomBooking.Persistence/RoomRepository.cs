using Microsoft.EntityFrameworkCore;
using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Persistence
{
  public class RoomRepository : IRoomRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public RoomRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
      => await _dbContext
            .Rooms
            .OrderBy(r => r.RoomNumber)
            .ToListAsync();

  }
}
