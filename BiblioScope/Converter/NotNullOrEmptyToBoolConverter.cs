using System.Globalization;

namespace BiblioScope.Converter;

/// <summary> Helper converting null to bool in xaml bindings</summary>
public class NotNullOrEmptyToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string str && !string.IsNullOrWhiteSpace(str);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}