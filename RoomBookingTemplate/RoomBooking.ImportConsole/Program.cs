using RoomBooking.Core.Contracts;
using RoomBooking.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.ImportConsole
{
  class Program
  {
    static async Task Main()
    {
      Console.WriteLine("Import der Buchungen in die Datenbank");
      using IUnitOfWork unitOfWork = new UnitOfWork();
      Console.WriteLine("Datenbank löschen");
      await unitOfWork.DeleteDatabaseAsync();
      Console.WriteLine("Datenbank migrieren");
      await unitOfWork.MigrateDatabaseAsync();
      Console.WriteLine("Daten werden von bookings.csv eingelesen");
      var bookings = (await ImportController.ReadBookingsFromCsvAsync()).ToArray();
      if (bookings.Length == 0)
      {
        Console.WriteLine("!!! Es wurden keine Buchungen eingelesen");
        return;
      }
      Console.WriteLine($"  Es wurden {bookings.Count()} Buchungen eingelesen!");
      await unitOfWork.Bookings.AddRangeAsync(bookings);
      Console.WriteLine("Buchungen werden in Datenbank gespeichert (persistiert)");
      await unitOfWork.SaveAsync();
      Console.Write("Beenden mit Eingabetaste ...");
      Console.ReadLine();
    }
  }
}
