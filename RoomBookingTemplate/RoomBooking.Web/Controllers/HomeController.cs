using Microsoft.AspNetCore.Mvc;
using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using RoomBooking.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }


    public async Task<ActionResult> Index(int? id)
    {
      var customers = (await _unitOfWork.Customers.GetAllAsync()).ToList();
      List<Booking> bookings;
      Customer customer;
      int customerId;
      if (id != null)
      {
        customer = customers.Single(c => c.Id == id);
      }
      else
      {
        customer = customers.FirstOrDefault();
      }
      if (customer != null)
      {
        customerId = customer.Id;
        bookings = (await _unitOfWork.Bookings.GetAllWithRoomsAsync(customerId)).ToList();
      }
      else
      {
        customerId = 0;
        bookings = new List<Booking>();
      }
      var homeIndexViewModel = new HomeIndexViewModel(customers, customerId, bookings);
      return View(homeIndexViewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Index(HomeIndexViewModel viewModel)
    {
      int customerId = viewModel.CustomerId;
      var customers = (await _unitOfWork.Customers.GetAllAsync()).ToList();
      var bookings = (await _unitOfWork.Bookings.GetAllWithRoomsAsync(customerId)).ToList();
      var homeIndexViewModel = new HomeIndexViewModel(customers, customerId, bookings);
      return View(homeIndexViewModel);
    }

    public IActionResult Error()
    {
      return View();
    }
  }
}
