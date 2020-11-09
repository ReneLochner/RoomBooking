using Microsoft.EntityFrameworkCore;
using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using RoomBooking.Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Persistence {
    public class UnitOfWork : IUnitOfWork {
        readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// ConnectionString kommt aus den appsettings.json
        /// </summary>
        public UnitOfWork()
        {
            _dbContext = new ApplicationDbContext();
            Rooms = new RoomRepository(_dbContext);
            Customers = new CustomerRepository(_dbContext);
            Bookings = new BookingRepository(_dbContext);
        }

        public ICustomerRepository Customers { get; }
        public IRoomRepository Rooms { get; }
        public IBookingRepository Bookings { get; }


        public async Task DeleteDatabaseAsync() => await _dbContext.Database.EnsureDeletedAsync();

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task MigrateDatabaseAsync() => await _dbContext.Database.MigrateAsync();

        /// <summary>
        /// Prüfung, ob für den zu buchenden Zeitraum des Raums bereits eine andere Buchung vorliegt.
        /// </summary>
        /// <param name="entity"></param>
        private async Task ValidateEntityAsync(object entity)
        {
            if (entity is Booking booking)
            {
                var bookingsForRoom = await _dbContext.Bookings.Include(b => b.Customer).Where(b => b.RoomId == booking.RoomId).ToArrayAsync();
                var bookingFrom = DateTime.Parse(booking.From);
                var bookingTo = DateTime.Parse(booking.To);
                foreach (var bookingForRoom in bookingsForRoom)
                {
                    var bookingForRoomFrom = DateTime.Parse(bookingForRoom.From);
                    var bookingForRoomTo = DateTime.Parse(bookingForRoom.To);
                    if (bookingFrom >= bookingForRoomFrom && bookingFrom <= bookingForRoomTo)
                    {
                        throw new ValidationException($"Es gibt schon eine Buchung von {bookingForRoom.Customer.LastName} von {bookingForRoom.From} bis {bookingForRoom.To} ", null, new List<string> { "From" });
                    }
                    if (bookingTo >= bookingForRoomFrom && bookingTo <= bookingForRoomTo)
                    {
                        throw new ValidationException($"Es gibt schon eine Buchung von {bookingForRoom.Customer.LastName} von {bookingForRoom.From} bis {bookingForRoom.To} ", null, new List<string> { "To" });
                    }
                }
            }
            if (entity is Customer customer)  // WPF-Anwendung
            {
                var dbCustomer = await _dbContext.Customers.Where(c => c.Id != customer.Id && c.FirstName.Equals(customer.FirstName) && customer.LastName.Equals(customer.LastName))
                    .ToArrayAsync();

                if (string.IsNullOrWhiteSpace(customer.LastName))
                {
                    throw new ValidationException("Es ist ein Nachname erforderlich!");
                }
                if (customer.LastName.Length < 2)
                {
                    throw new ValidationException("Der Nachname muss mindestens zwei Zeichen lang sein!");
                }
                if (dbCustomer != null)
                {
                    throw new ValidationException($"{customer.LastName} {customer.FirstName} existiert bereits!");
                }
                if (!IbanChecker.CheckIban(customer.Iban))
                {
                    throw new ValidationException("Kein gültiger IBAN!");
                }
            }
        }

        /// <summary>
        /// Geänderte Entities extra validieren
        /// </summary>
        public async Task SaveAsync()
        {
            var entities = _dbContext.ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added
                                 || entity.State == EntityState.Modified)
                .Select(e => e.Entity);
            foreach (var entity in entities)
            {
                await ValidateEntityAsync(entity);
            }
            await _dbContext.SaveChangesAsync();
        }

    }
}
