using System.Globalization;

namespace BiblioScope.Converter;

/// <summary>Converter helping convert 0 to bool in xaml bindings /// </summary>
public class ZeroToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int count)
            return count == 0;
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        => throw new NotImplementedException();
}