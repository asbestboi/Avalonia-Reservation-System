using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReservationSystem.Models;

namespace ReservationSystem.Services;

public class FileService
{
    private readonly AppDataService _appDataService;

    private const string ReservationsFile = "reservations.txt";
    private const string ResourcesFile = "resources.txt";
    private const string UsersFile = "users.txt";
    private const string SettingsFile = "settings.txt";

    public string AppDataPath => _appDataService.AppDataPath;

    public FileService(AppDataService appDataService)
    {
        _appDataService = appDataService;
        EnsureDefaultData();
    }

    public List<Reservation> LoadReservations()
    {
        var path = _appDataService.GetFilePath(ReservationsFile);
        if (!File.Exists(path)) return [];
        return File.ReadAllLines(path)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(Reservation.FromFileString)
            .OfType<Reservation>()
            .ToList();
    }

    public void SaveReservations(IEnumerable<Reservation> reservations)
    {
        var path = _appDataService.GetFilePath(ReservationsFile);
        File.WriteAllLines(path, reservations.Select(r => r.ToFileString()));
    }

    public List<ReservationResource> LoadResources()
    {
        var path = _appDataService.GetFilePath(ResourcesFile);
        if (!File.Exists(path)) return [];
        return File.ReadAllLines(path)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(ReservationResource.FromFileString)
            .OfType<ReservationResource>()
            .ToList();
    }

    public void SaveResources(IEnumerable<ReservationResource> resources)
    {
        var path = _appDataService.GetFilePath(ResourcesFile);
        File.WriteAllLines(path, resources.Select(r => r.ToFileString()));
    }

    public List<User> LoadUsers()
    {
        var path = _appDataService.GetFilePath(UsersFile);
        if (!File.Exists(path)) return [];
        return File.ReadAllLines(path)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(User.FromFileString)
            .OfType<User>()
            .ToList();
    }

    public void SaveUsers(IEnumerable<User> users)
    {
        var path = _appDataService.GetFilePath(UsersFile);
        File.WriteAllLines(path, users.Select(u => u.ToFileString()));
    }

    public AppSettings LoadSettings()
    {
        var path = _appDataService.GetFilePath(SettingsFile);
        if (!File.Exists(path)) return new AppSettings();
        var line = File.ReadAllLines(path).FirstOrDefault();
        return line != null ? AppSettings.FromFileString(line) : new AppSettings();
    }

    public void SaveSettings(AppSettings settings)
    {
        var path = _appDataService.GetFilePath(SettingsFile);
        File.WriteAllText(path, settings.ToFileString());
    }

    private void EnsureDefaultData()
    {
        if (LoadResources().Count == 0)
        {
            SaveResources([
                new() { Name = "Učebna 101", Type = ReservationType.Room, Description = "Kapacita 30 míst" },
                new() { Name = "Učebna 102", Type = ReservationType.Room, Description = "Kapacita 25 míst" },
                new() { Name = "Tělocvična", Type = ReservationType.SportsFacility, Description = "Hlavní tělocvična" },
                new() { Name = "Projektor A", Type = ReservationType.Equipment, Description = "Přenosný projektor" }
            ]);
        }

        var users = LoadUsers();
        if (users.Count == 0)
        {
            users =
            [
                new() { Name = "Adam Bauer", Email = "adam.bauer@skola.cz" },
                new() { Name = "Jan Novák", Email = "jan.novak@skola.cz" },
                new() { Name = "Marie Dvořáková", Email = "marie.dvorakova@skola.cz" }
            ];
            SaveUsers(users);
        }
        else if (!users.Any(u => u.Name == "Adam Bauer"))
        {
            users.Add(new() { Name = "Adam Bauer", Email = "adam.bauer@skola.cz" });
            SaveUsers(users);
        }
    }
}
