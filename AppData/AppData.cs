// Slozku AppData spravuje Services.AppDataService.
// Na Windows  : %APPDATA%\ReservationSystem\
// Na Linux    : ~/.config/ReservationSystem\   (nebo XDG_CONFIG_HOME)
// Na macOS    : ~/Library/Application Support\ReservationSystem\
//
// Ulozene soubory:
//   reservations.txt  - vsechny rezervace (oddelene svislitkem)
//   resources.txt     - rezervovatelne prostredky (mistnosti, zarizeni, vybaveni)
//   users.txt         - seznam uzivatelu
//   settings.txt      - nastaveni aplikace

namespace ReservationSystem;

// Znackovaci trida - udrzuje tuto slozku viditelnou v prehledu reseni.
internal static class AppDataFolder { }
