using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using RoomBooking.Persistence;
using RoomBooking.Wpf.Common;
using RoomBooking.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RoomBooking.Wpf.ViewModels {
    public class MainViewModel : BaseViewModel {
        public ObservableCollection<Booking> _bookings;
        public ObservableCollection<Room> _rooms;
        public Booking _selectedBooking;
        public Room _selectedRoom;

        public ObservableCollection<Booking> Bookings {
            get => _bookings;
            set {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
            }
        }

        public ObservableCollection<Room> Rooms {
            get => _rooms;
            set {
                _rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }

        public Booking SelectedBooking {
            get => _selectedBooking;
            set {
                _selectedBooking = value;
                OnPropertyChanged(nameof(SelectedBooking));
            }
        }

        public Room SelectedRoom {
            get => _selectedRoom;
            set {
                _selectedRoom = value;
                OnPropertyChanged(nameof(Booking));
                _ = LoadDataAsync();
            }
        }

        public MainViewModel(IWindowController windowController) : base(windowController)
        {
        }

        private ICommand _cmdEditCustomer;
        public ICommand CmdEditCustomer {
            get {
                if (_cmdEditCustomer == null)
                {
                    _cmdEditCustomer = new RelayCommand(
                        execute: _ =>
                        {
                            Controller.ShowWindow(new EditCustomerViewModel(Controller, SelectedBooking.Customer), true);
                            _ = LoadDataAsync();
                        },
                        canExecute: _ => SelectedBooking != null
                    );
                }

                return _cmdEditCustomer;
            }
        }

        private async Task LoadDataAsync()
        {
            using IUnitOfWork uow = new UnitOfWork();

            if(Rooms == null)
            {
                var rooms = (await uow.Rooms
                    .GetAllAsync())
                    .OrderBy(room => room.RoomNumber)
                    .ToList();

                Rooms = new ObservableCollection<Room>(rooms);
                SelectedRoom = Rooms.First();
            }

            var bookings = (await uow.Bookings
                .GetByRoomWithCustomerAsync(SelectedRoom.Id))
                .OrderBy(booking => booking.From)
                .ThenBy(booking => booking.To)
                .ToList();

            Bookings = new ObservableCollection<Booking>(bookings);
        }

        public static async Task<MainViewModel> CreateAsync(IWindowController windowController)
        {
            var viewModel = new MainViewModel(windowController);
            await viewModel.LoadDataAsync();
            return viewModel;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
