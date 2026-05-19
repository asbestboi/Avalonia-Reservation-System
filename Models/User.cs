using System;

namespace ReservationSystem.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string ToFileString() => $"{Id}|{Name}|{Email}";

    public static User? FromFileString(string line)
    {
        var parts = line.Split('|');
        if (parts.Length < 3) return null;
        return new User { Id = parts[0], Name = parts[1], Email = parts[2] };
    }

    public override string ToString() => Name;
}
