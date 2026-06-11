using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReservationSystem.Models;
using ReservationSystem.Services;

namespace ReservationSystem.ViewModels;

public partial class CalendarViewModel : ViewModelBase
{
    private readonly ReservationService _reservationService;
    private readonly MainWindowViewModel _mainVm;

    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;

    [ObservableProperty]
    private ObservableCollection<Reservation> _dailyReservations = [];

    [ObservableProperty]
    private DateTime _currentMonth;

    [ObservableProperty]
    private ObservableCollection<CalendarDay> _calendarDays = [];

    public CalendarViewModel(ReservationService reservationService, MainWindowViewModel mainVm)
    {
        _reservationService = reservationService;
        _mainVm = mainVm;
        _currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        Refresh();
    }

    public void Refresh()
    {
        BuildCalendar();
        LoadDailyReservations();
    }

    partial void OnSelectedDateChanged(DateTime value) => LoadDailyReservations();

    private void BuildCalendar()
    {
        CalendarDays.Clear();

        // Tyden zacinajici pondelim: pondeli=0 offset
        var firstDay = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month);

        // ISO: pondeli = 1 ... nedele = 7
        var startOffset = (int)firstDay.DayOfWeek;
        if (startOffset == 0) startOffset = 7;
        startOffset--;

        for (int i = 0; i < startOffset; i++)
            CalendarDays.Add(new CalendarDay { IsEmpty = true });

        var allReservations = _reservationService.GetAllReservations();

        for (int d = 1; d <= daysInMonth; d++)
        {
            var date = new DateTime(CurrentMonth.Year, CurrentMonth.Month, d);
            CalendarDays.Add(new CalendarDay
            {
                Date = date,
                DayNumber = d,
                IsToday = date == DateTime.Today,
                IsSelected = date == SelectedDate,
                HasReservations = allReservations.Any(r => r.StartTime.Date == date)
            });
        }
    }

    private void LoadDailyReservations()
    {
        DailyReservations = new ObservableCollection<Reservation>(
            _reservationService.GetReservationsForDate(SelectedDate));
    }

    [RelayCommand]
    private void PreviousMonth()
    {
        CurrentMonth = CurrentMonth.AddMonths(-1);
        BuildCalendar();
    }

    [RelayCommand]
    private void NextMonth()
    {
        CurrentMonth = CurrentMonth.AddMonths(1);
        BuildCalendar();
    }

    [RelayCommand]
    private void SelectDay(CalendarDay? day)
    {
        if (day == null || day.IsEmpty) return;
        SelectedDate = day.Date;
        foreach (var d in CalendarDays)
            d.IsSelected = !d.IsEmpty && d.Date == SelectedDate;
    }

    [RelayCommand]
    private void AddReservationForDay() => _mainVm.ShowAddReservation();
}

public partial class CalendarDay : ObservableObject
{
    public bool IsEmpty { get; init; }
    public bool IsNotEmpty => !IsEmpty;
    public DateTime Date { get; init; }
    public int DayNumber { get; init; }
    public bool HasReservations { get; init; }
    public bool IsToday { get; init; }

    [ObservableProperty]
    private bool _isSelected;
}
