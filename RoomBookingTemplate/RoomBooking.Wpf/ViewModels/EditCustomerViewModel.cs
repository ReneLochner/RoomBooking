using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using RoomBooking.Persistence;
using RoomBooking.Wpf.Common;
using RoomBooking.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Input;

namespace RoomBooking.Wpf.ViewModels {
    public class EditCustomerViewModel : BaseViewModel {
        public Customer _customer;
        public Customer _undo;
        public string _firstName;
        public string _lastName;
        public string _iban;

        public Customer Customer {
            get => _customer;
            set {
                _customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }

        public String FirstName {
            get => _firstName;
            set {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        [MinLength(2, ErrorMessage = "Der Nachname muss mindestens 2 Zeichen lang sein!")]
        [MaxLength(50, ErrorMessage = "Der Nachname darf maximal 50 Zeichen lang sein!")]
        public String LastName {
            get => _lastName;
            set {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Iban {
            get => _iban;
            set {
                _iban = value;
                OnPropertyChanged(nameof(Iban));
            }
        }

        public EditCustomerViewModel(IWindowController windowController, Customer customer) : base(windowController)
        {
            this.Customer = customer;
            this.FirstName = customer.FirstName;
            this.LastName = customer.LastName;
            this.Iban = customer.Iban;

            _undo = new Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Iban = customer.Iban
            };
        }

        private ICommand _cmdUndo;
        public ICommand CmdUndo {
            get {
                if (_cmdUndo == null)
                {
                    _cmdUndo = new RelayCommand(
                        execute: _ =>
                        {
                            FirstName = _undo.FirstName;
                            LastName = _undo.LastName;
                            Iban = _undo.Iban;

                        },
                        canExecute: _ => Customer != _undo);
                }

                return _cmdUndo;
            }
            set => _cmdUndo= value;
        }

        private ICommand _cmdSave;
        public ICommand CmdSave {
            get {
                if(_cmdSave == null)
                {
                    _cmdSave = new RelayCommand(
                        execute: async _ =>
                        {
                            using IUnitOfWork uow = new UnitOfWork();
                            Customer.FirstName = FirstName;
                            Customer.LastName = LastName;
                            Customer.Iban = Iban;
                            await uow.SaveAsync();
                            uow.Customers.Update(Customer);
                            Controller.CloseWindow(this);
                        },
                        canExecute: _ => Customer != null);
                }

                return _cmdSave;
            }
            set => _cmdSave = value;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
