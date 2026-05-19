// AppData folder is managed by Services.AppDataService.
// On Windows  : %APPDATA%\ReservationSystem\
// On Linux    : ~/.config/ReservationSystem\   (or XDG_CONFIG_HOME)
// On macOS    : ~/Library/Application Support\ReservationSystem\
//
// Files stored there:
//   reservations.txt  – all reservations (pipe-separated)
//   resources.txt     – reservable resources (rooms, facilities, equipment)
//   users.txt         – user list
//   settings.txt      – application settings

namespace ReservationSystem;

// Marker class — keeps this folder visible in solution explorers.
internal static class AppDataFolder { }
