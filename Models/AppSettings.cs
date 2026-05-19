namespace ReservationSystem.Models;

public class AppSettings
{
    public string DefaultUserId { get; set; } = string.Empty;
    public int DefaultReservationDurationMinutes { get; set; } = 60;

    public string ToFileString() => $"{DefaultUserId}|{DefaultReservationDurationMinutes}";

    public static AppSettings FromFileString(string line)
    {
        var parts = line.Split('|');
        if (parts.Length < 2) return new AppSettings();
        return new AppSettings
        {
            DefaultUserId = parts[0],
            DefaultReservationDurationMinutes = int.TryParse(parts[1], out var d) ? d : 60
        };
    }
}
