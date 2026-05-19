using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReservationSystem.Models;
using ReservationSystem.Services;

namespace ReservationSystem.ViewModels;

public partial class ReservationListViewModel : ViewModelBase
{
    private readonly ReservationService _reservationService;
    private readonly MainWindowViewModel _mainVm;

    [ObservableProperty]
    private ObservableCollection<Reservation> _reservations = [];

    [ObservableProperty]
    private Reservation? _selectedReservation;

    [ObservableProperty]
    private string _filterText = string.Empty;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public ReservationListViewModel(ReservationService reservationService, MainWindowViewModel mainVm)
    {
        _reservationService = reservationService;
        _mainVm = mainVm;
        LoadReservations();
    }

    public void LoadReservations()
    {
        var all = _reservationService.GetAllReservations();
        var filtered = string.IsNullOrWhiteSpace(FilterText)
            ? all
            : all.Where(r =>
                r.Title.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                r.Description.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                r.TypeDisplayName.Contains(FilterText, StringComparison.OrdinalIgnoreCase));
        Reservations = new ObservableCollection<Reservation>(filtered.OrderBy(r => r.StartTime));
    }

    partial void OnFilterTextChanged(string value) => LoadReservations();

    [RelayCommand]
    private void AddReservation() => _mainVm.ShowAddReservation();

    [RelayCommand]
    private void EditReservation()
    {
        if (SelectedReservation != null)
            _mainVm.ShowAddReservation(SelectedReservation);
    }

    [RelayCommand]
    private void DeleteReservation()
    {
        if (SelectedReservation == null) return;
        _reservationService.DeleteReservation(SelectedReservation.Id);
        LoadReservations();
        StatusMessage = "Rezervace byla smazána.";
    }

    [RelayCommand]
    private void ExportToTxt()
    {
        var path = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            $"rezervace_{DateTime.Now:yyyyMMdd_HHmm}.txt");
        _reservationService.ExportToTxt(path);
        StatusMessage = $"Exportováno: {path}";
    }
}
