using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReservationSystem.Models;
using ReservationSystem.Services;

namespace ReservationSystem.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ReservationService _reservationService;

    [ObservableProperty]
    private ViewModelBase _currentView = null!;

    public ReservationListViewModel ReservationListVM { get; }
    public CalendarViewModel CalendarVM { get; }
    private SettingsViewModel? _settingsVM;

    public MainWindowViewModel(ReservationService reservationService)
    {
        _reservationService = reservationService;
        ReservationListVM = new ReservationListViewModel(reservationService, this);
        CalendarVM = new CalendarViewModel(reservationService, this);
        CurrentView = ReservationListVM;
    }

    [RelayCommand]
    private void ShowReservationList() => CurrentView = ReservationListVM;

    [RelayCommand]
    private void ShowCalendar()
    {
        CalendarVM.Refresh();
        CurrentView = CalendarVM;
    }

    [RelayCommand]
    private void ShowSettings()
    {
        _settingsVM ??= new SettingsViewModel(_reservationService, this);
        CurrentView = _settingsVM;
    }

    public void ShowAddReservation(Reservation? existing = null)
    {
        CurrentView = new AddReservationViewModel(_reservationService, this, existing);
    }
}
