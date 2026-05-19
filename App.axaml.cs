using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReservationSystem.Services;
using ReservationSystem.ViewModels;
using ReservationSystem.Views;

namespace ReservationSystem;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var appDataService = new AppDataService();
            var fileService = new FileService(appDataService);
            var reservationService = new ReservationService(fileService);

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(reservationService)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
