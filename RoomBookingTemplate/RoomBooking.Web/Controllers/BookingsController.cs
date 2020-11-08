using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Core.Contracts;
using RoomBooking.Web.DataTransferObjects;
using RoomBooking.Web.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Web.Controllers
{
  public class BookingsController : Controller
  {
    private readonly IUnitOfWork _unitOfWork;

    public BookingsController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
      return RedirectToAction("Index", "Home");
    }


    /// <summary>
    /// Anforderung der Bearbeitung einer Buchung
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult> Edit(int id)
    {
      var booking = await _unitOfWork.Bookings.GetByIdAsync(id);

      if (booking == null)
      {
        return NotFound();
      }

      var viewModel = new BookingsViewModel()
      {
        Id = booking.Id,
        From = booking.From,
        To = booking.To,
        CustomerId = booking.Customer.Id,
        RoomId = booking.Room.Id,
        Customers = (await _unitOfWork.Customers.GetAllAsync()).Select(c => new CustomerDto(c)),
        Rooms = await _unitOfWork.Rooms.GetAllAsync()
      };

      return View("Edit", viewModel);
    }

    /// <summary>
    /// ViewModel auf Korrektheit überprüfen und abspeichern
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns>Ergebnis des Update als Response</returns>
    [HttpPost]
    public async Task<ActionResult> Edit(BookingsViewModel viewModel)
    {
      var customers = (await _unitOfWork.Customers.GetAllAsync()).Select(c => new CustomerDto(c));
      var rooms = await _unitOfWork.Rooms.GetAllAsync();
      viewModel.Customers = customers;
      viewModel.Rooms = rooms;
      if (ModelState.IsValid)
      {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(viewModel.Id);
        if (booking == null)
        {
          return NotFound();
        }
        booking.From = viewModel.From;
        booking.To = viewModel.To;
        booking.CustomerId = viewModel.CustomerId;
        booking.RoomId = viewModel.RoomId;
        try
        {
          await _unitOfWork.SaveAsync();
          return RedirectToAction("Index", "Home", new { id = booking.CustomerId });
        }
        catch (ValidationException validationException)
        {
          // Beim Speichern wurde geprüft, ob die zu speichernde Buchung sich nicht mit
          // anderen Buchungen des Raums überlagert
          ModelState.AddModelError((string)validationException.Value, validationException.Message);
          return View("Edit", viewModel);
        }
        catch (Exception exception)
        {
          ModelState.AddModelError("", exception.GetType() + " " + exception.Message);
          return View("Edit", viewModel);
        }
      }
      return View("Edit", viewModel);

    }


    /// <summary>
    /// Buchung löschen und auf Hauptseite weiter verweisen.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
      if (booking == null)
      {
        return NotFound();
      }
      var customerId = booking.CustomerId;
      _unitOfWork.Bookings.Delete(booking);
      await _unitOfWork.SaveAsync();
      return RedirectToAction("Index", "Home", new { id = customerId });
    }

    [AllowAnonymous]
    [HttpPost]
    public ActionResult CheckTimeBeetween8And18(string from)
    {
      bool ok = DateTime.TryParse(from, out DateTime fromTime);
      if (!ok)
      {
        return Json(false);
      }
      return Json(fromTime >= DateTime.Parse("08:00")
          && fromTime <= DateTime.Parse("18:00"));
    }

  }
}
