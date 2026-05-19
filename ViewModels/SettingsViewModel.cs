using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReservationSystem.Models;
using ReservationSystem.Services;

namespace ReservationSystem.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly ReservationService _reservationService;
    private readonly MainWindowViewModel _mainVm;

    [ObservableProperty]
    private User? _selectedDefaultUser;

    [ObservableProperty]
    private int _defaultDuration;

    [ObservableProperty]
    private string _appDataPath;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public IList<User> Users { get; }

    public SettingsViewModel(ReservationService reservationService, MainWindowViewModel mainVm)
    {
        _reservationService = reservationService;
        _mainVm = mainVm;
        Users = reservationService.GetAllUsers().ToList();

        var settings = reservationService.LoadSettings();
        SelectedDefaultUser = Users.FirstOrDefault(u => u.Id == settings.DefaultUserId);
        DefaultDuration = settings.DefaultReservationDurationMinutes;
        _appDataPath = reservationService.AppDataPath;
    }

    [RelayCommand]
    private void Save()
    {
        _reservationService.SaveSettings(new AppSettings
        {
            DefaultUserId = SelectedDefaultUser?.Id ?? string.Empty,
            DefaultReservationDurationMinutes = DefaultDuration
        });
        StatusMessage = "Nastavení bylo uloženo.";
    }

    [RelayCommand]
    private void Back() => _mainVm.ShowReservationListCommand.Execute(null);
}
