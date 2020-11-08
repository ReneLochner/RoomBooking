using RoomBooking.Wpf.Common;
using RoomBooking.Wpf.ViewModels;
using System.Windows;

namespace RoomBooking.Wpf
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public App()
    {
    }

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
      WindowController controller = new WindowController();
      var mainViewModel = await MainViewModel.CreateAsync(controller);
      controller.ShowWindow(mainViewModel, false);
    }
  }
}
