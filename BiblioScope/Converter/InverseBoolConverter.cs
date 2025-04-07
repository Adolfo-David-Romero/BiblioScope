using System.Globalization;

namespace BiblioScope.Converter;

/// <summary> InverseBoolConverter is used when inverting a boolean value in XAML bindings.</summary>
public class InverseBoolConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is bool b ? !b : false;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}