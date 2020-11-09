using RoomBooking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace RoomBooking.ImportConsole {
    public static class ImportController {
        /// <summary>
        /// Liest die Buchungen mit ihren Räumen und Kunden aus der
        /// csv-Datei ein.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Booking>> ReadBookingsFromCsvAsync()
        {
            string[][] matrix = await MyFile.ReadStringMatrixFromCsvAsync("bookings.csv", true);

            var customers = matrix
                .Select(customer => new Customer()
                {
                    LastName = customer[0],
                    FirstName = customer[1],
                    Iban = customer[2]
                })
                .GroupBy(entry => entry.LastName + entry.FirstName + entry.Iban)
                .Select(customer => customer.First())
                .ToArray();

            var rooms = matrix
                .GroupBy(entry => entry[3])
                .Select(room => new Room()
                {
                    RoomNumber = room.Key
                })
                .ToArray();

            var bookings = matrix
                .Select(booking => new Booking
                {
                    Customer = customers.Single(customer => customer.LastName == booking[0] && customer.FirstName == booking[1] && customer.Iban == booking[2]
                  ),
                    Room = rooms.Single(room => room.RoomNumber == booking[3]),
                    From = booking[4],
                    To = booking[5]
                })
                .ToArray();

            return bookings;
        }
    }
}
