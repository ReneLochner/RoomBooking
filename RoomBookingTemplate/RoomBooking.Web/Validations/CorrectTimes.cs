using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RoomBooking.Web.ViewModels;

namespace RoomBooking.Web.Validations
{
    public class CorrectTimes : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            BookingsViewModel viewModel = (BookingsViewModel) validationContext.ObjectInstance;
            DateTime fromTime;
            DateTime toTime;
            if (viewModel.From == null || !DateTime.TryParse(viewModel.From, out fromTime))
            {
                return new ValidationResult("Kein gültiges Zeitformat HH:MM", new List<string> { "From" });
            }
            if (viewModel.To == null || !DateTime.TryParse(viewModel.To, out toTime))
            {
                return new ValidationResult("Kein gültiges Zeitformat HH:MM", new List<string> { "To" });
            }
            if (toTime <= fromTime)
            {
                return new ValidationResult("Biszeit muss hinter Vonzeit liegen", new List<string>{ "To" });
            }
            return ValidationResult.Success;
        }
    }
}
