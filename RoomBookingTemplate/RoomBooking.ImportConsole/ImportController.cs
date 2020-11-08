using RoomBooking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils;

namespace RoomBooking.ImportConsole
{
  public static class ImportController
  {
    /// <summary>
    /// Liest die Buchungen mit ihren Räumen und Kunden aus der
    /// csv-Datei ein.
    /// </summary>
    /// <returns></returns>
    public static async Task<IEnumerable<Booking>> ReadBookingsFromCsvAsync()
    {
      string[][] matrix = await MyFile.ReadStringMatrixFromCsvAsync("bookings.csv", true);
      throw new NotImplementedException();
    }

  }
}
