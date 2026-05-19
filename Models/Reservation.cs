using System;

namespace ReservationSystem.Models;

public class Reservation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string ResourceId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; } = string.Empty;
    public ReservationType Type { get; set; } = ReservationType.Room;

    public string TimeRange => $"{StartTime:dd.MM.yyyy HH:mm} – {EndTime:HH:mm}";

    public string TypeDisplayName => Type switch
    {
        ReservationType.Room => "Učebna",
        ReservationType.SportsFacility => "Sportoviště",
        ReservationType.Equipment => "Vybavení",
        _ => Type.ToString()
    };

    public bool ConflictsWith(Reservation other)
    {
        if (Id == other.Id) return false;
        if (ResourceId != other.ResourceId) return false;
        return StartTime < other.EndTime && EndTime > other.StartTime;
    }

    public string ToFileString() =>
        $"{Id}|{Title}|{ResourceId}|{UserId}|{StartTime:O}|{EndTime:O}|{Description}|{(int)Type}";

    public static Reservation? FromFileString(string line)
    {
        var parts = line.Split('|');
        if (parts.Length < 8) return null;
        return new Reservation
        {
            Id = parts[0],
            Title = parts[1],
            ResourceId = parts[2],
            UserId = parts[3],
            StartTime = DateTime.Parse(parts[4]),
            EndTime = DateTime.Parse(parts[5]),
            Description = parts[6],
            Type = (ReservationType)int.Parse(parts[7])
        };
    }
}

public enum ReservationType
{
    Room,
    SportsFacility,
    Equipment
}
