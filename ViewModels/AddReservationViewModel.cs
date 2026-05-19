using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReservationSystem.Models;
using ReservationSystem.Services;

namespace ReservationSystem.ViewModels;

public partial class AddReservationViewModel : ViewModelBase
{
    private readonly ReservationService _reservationService;
    private readonly MainWindowViewModel _mainVm;
    private readonly string? _editId;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private ReservationResource? _selectedResource;

    [ObservableProperty]
    private User? _selectedUser;

    [ObservableProperty]
    private DateTimeOffset? _startDate = DateTimeOffset.Now;

    [ObservableProperty]
    private TimeSpan? _startTime = TimeSpan.FromHours(9);

    [ObservableProperty]
    private TimeSpan? _endTime = TimeSpan.FromHours(10);

    [ObservableProperty]
    private ReservationType _selectedType = ReservationType.Room;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isEditing;

    public string WindowTitle => IsEditing ? "Upravit rezervaci" : "Nová rezervace";

    public IList<ReservationResource> Resources { get; }
    public IList<User> Users { get; }
    public IList<ReservationType> Types { get; } = Enum.GetValues<ReservationType>();

    public AddReservationViewModel(ReservationService reservationService, MainWindowViewModel mainVm, Reservation? existing = null)
    {
        _reservationService = reservationService;
        _mainVm = mainVm;
        Resources = reservationService.GetAllResources().ToList();
        Users = reservationService.GetAllUsers().ToList();

        if (existing != null)
        {
            _editId = existing.Id;
            IsEditing = true;
            Title = existing.Title;
            Description = existing.Description;
            SelectedResource = Resources.FirstOrDefault(r => r.Id == existing.ResourceId);
            SelectedUser = Users.FirstOrDefault(u => u.Id == existing.UserId);
            StartDate = new DateTimeOffset(existing.StartTime);
            StartTime = existing.StartTime.TimeOfDay;
            EndTime = existing.EndTime.TimeOfDay;
            SelectedType = existing.Type;
        }
        else
        {
            SelectedResource = Resources.FirstOrDefault();
            SelectedUser = Users.FirstOrDefault();
        }
    }

    [RelayCommand]
    private void Save()
    {
        ErrorMessage = string.Empty;

        var date = StartDate?.Date ?? DateTime.Today;
        var reservation = new Reservation
        {
            Id = _editId ?? Guid.NewGuid().ToString(),
            Title = Title.Trim(),
            Description = Description.Trim(),
            ResourceId = SelectedResource?.Id ?? string.Empty,
            UserId = SelectedUser?.Id ?? string.Empty,
            StartTime = date + (StartTime ?? TimeSpan.FromHours(9)),
            EndTime = date + (EndTime ?? TimeSpan.FromHours(10)),
            Type = SelectedType
        };

        var (success, error) = IsEditing
            ? _reservationService.UpdateReservation(reservation)
            : _reservationService.AddReservation(reservation);

        if (!success)
        {
            ErrorMessage = error;
            return;
        }

        _mainVm.ReservationListVM.LoadReservations();
        _mainVm.ShowReservationListCommand.Execute(null);
    }

    [RelayCommand]
    private void Cancel() => _mainVm.ShowReservationListCommand.Execute(null);
}
