using System;
using System.IO;

namespace ReservationSystem.Services;

public class AppDataService
{
    public string AppDataPath { get; }

    public AppDataService()
    {
        AppDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ReservationSystem");
        Directory.CreateDirectory(AppDataPath);
    }

    public string GetFilePath(string fileName) => Path.Combine(AppDataPath, fileName);
}
