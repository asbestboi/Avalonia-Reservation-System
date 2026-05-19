using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReservationSystem.Models;

namespace ReservationSystem.Services;

public class ReservationService
{
    private readonly FileService _fileService;
    private List<Reservation> _reservations;
    private readonly List<ReservationResource> _resources;
    private readonly List<User> _users;

    public string AppDataPath => _fileService.AppDataPath;

    public ReservationService(FileService fileService)
    {
        _fileService = fileService;
        _reservations = _fileService.LoadReservations();
        _resources = _fileService.LoadResources();
        _users = _fileService.LoadUsers();
    }

    public IReadOnlyList<Reservation> GetAllReservations() => _reservations.AsReadOnly();
    public IReadOnlyList<ReservationResource> GetAllResources() => _resources.AsReadOnly();
    public IReadOnlyList<User> GetAllUsers() => _users.AsReadOnly();

    public IEnumerable<Reservation> GetReservationsForDate(DateTime date) =>
        _reservations.Where(r => r.StartTime.Date == date.Date).OrderBy(r => r.StartTime);

    public bool HasConflict(Reservation reservation) =>
        _reservations.Any(r => r.ConflictsWith(reservation));

    public (bool Success, string Error) AddReservation(Reservation reservation)
    {
        if (string.IsNullOrWhiteSpace(reservation.Title))
            return (false, "Název rezervace nesmí být prázdný.");
        if (string.IsNullOrWhiteSpace(reservation.ResourceId))
            return (false, "Musíte vybrat prostředek.");
        if (string.IsNullOrWhiteSpace(reservation.UserId))
            return (false, "Musíte vybrat uživatele.");
        if (reservation.StartTime >= reservation.EndTime)
            return (false, "Čas konce musí být po čase začátku.");
        if (HasConflict(reservation))
            return (false, "Termín koliduje s existující rezervací stejného prostředku.");

        _reservations.Add(reservation);
        _fileService.SaveReservations(_reservations);
        return (true, string.Empty);
    }

    public (bool Success, string Error) UpdateReservation(Reservation reservation)
    {
        if (string.IsNullOrWhiteSpace(reservation.Title))
            return (false, "Název rezervace nesmí být prázdný.");
        if (reservation.StartTime >= reservation.EndTime)
            return (false, "Čas konce musí být po čase začátku.");
        if (HasConflict(reservation))
            return (false, "Termín koliduje s existující rezervací stejného prostředku.");

        var index = _reservations.FindIndex(r => r.Id == reservation.Id);
        if (index < 0) return (false, "Rezervace nenalezena.");
        _reservations[index] = reservation;
        _fileService.SaveReservations(_reservations);
        return (true, string.Empty);
    }

    public void DeleteReservation(string id)
    {
        _reservations.RemoveAll(r => r.Id == id);
        _fileService.SaveReservations(_reservations);
    }

    public void ExportToTxt(string filePath)
    {
        var resourceNames = _resources.ToDictionary(r => r.Id, r => r.Name);
        var userNames = _users.ToDictionary(u => u.Id, u => u.Name);

        var lines = new List<string>
        {
            "=== PŘEHLED REZERVACÍ ===",
            $"Datum exportu: {DateTime.Now:dd.MM.yyyy HH:mm}",
            $"Celkem rezervací: {_reservations.Count}",
            string.Empty
        };

        foreach (var r in _reservations.OrderBy(x => x.StartTime))
        {
            lines.Add($"[{r.TypeDisplayName}] {r.Title}");
            lines.Add($"  Prostředek : {resourceNames.GetValueOrDefault(r.ResourceId, r.ResourceId)}");
            lines.Add($"  Datum      : {r.StartTime:dd.MM.yyyy HH:mm} – {r.EndTime:HH:mm}");
            lines.Add($"  Rezervoval : {userNames.GetValueOrDefault(r.UserId, r.UserId)}");
            if (!string.IsNullOrEmpty(r.Description))
                lines.Add($"  Poznámka   : {r.Description}");
            lines.Add(string.Empty);
        }

        File.WriteAllLines(filePath, lines);
    }

    public AppSettings LoadSettings() => _fileService.LoadSettings();
    public void SaveSettings(AppSettings settings) => _fileService.SaveSettings(settings);
}
