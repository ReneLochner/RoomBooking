using System.Threading.Tasks;

namespace RoomBooking.Wpf.Common.Contracts
{
  public interface IWindowController
  {
    void ShowWindow(BaseViewModel viewModel, bool showAsDialog);
    void CloseWindow(BaseViewModel viewModel);
  }
}
