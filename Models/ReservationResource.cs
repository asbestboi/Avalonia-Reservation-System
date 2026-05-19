using System;

namespace ReservationSystem.Models;

public class ReservationResource
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public ReservationType Type { get; set; }
    public string Description { get; set; } = string.Empty;

    public string ToFileString() => $"{Id}|{Name}|{(int)Type}|{Description}";

    public static ReservationResource? FromFileString(string line)
    {
        var parts = line.Split('|');
        if (parts.Length < 4) return null;
        return new ReservationResource
        {
            Id = parts[0],
            Name = parts[1],
            Type = (ReservationType)int.Parse(parts[2]),
            Description = parts[3]
        };
    }

    public override string ToString() => Name;
}
