using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ReservationSystem.Models;

namespace ReservationSystem.Converters;

/// <summary>
/// Převádí typ rezervace na barvu pro barevné označení v seznamu a kalendáři.
/// </summary>
public class TypeToBrushConverter : IValueConverter
{
    public static readonly TypeToBrushConverter Instance = new();

    private static readonly SolidColorBrush Room = new(Color.Parse("#1976D2"));        // modrá
    private static readonly SolidColorBrush Sports = new(Color.Parse("#2E7D32"));      // zelená
    private static readonly SolidColorBrush Equipment = new(Color.Parse("#F57C00"));   // oranžová
    private static readonly SolidColorBrush Fallback = new(Color.Parse("#9AA5B1"));    // šedá

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ReservationType type) return Fallback;
        return type switch
        {
            ReservationType.Room => Room,
            ReservationType.SportsFacility => Sports,
            ReservationType.Equipment => Equipment,
            _ => Fallback
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
